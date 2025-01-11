using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PromptNote.Models
{
    public static class PromptParser
    {
        public static List<Prompt> Parse(string text)
        {
            var parentheses = new List<char>();
            var inParentheses = string.Empty;
            var prompt = string.Empty;
            var counter = 0;
            var escape = false;

            foreach (var c in text)
            {
                counter++;

                if (escape)
                {
                    if (parentheses.Count > 0)
                    {
                        inParentheses += c;
                    }
                    else
                    {
                        prompt += c;
                    }

                    escape = false;
                    continue;
                }

                if (c == '\\')
                {
                    escape = true; // 次の文字をエスケープとして処理する
                    prompt += c;
                    continue;
                }

                if (c == '(')
                {
                    parentheses.Add(c);
                    inParentheses += c;
                    continue;
                }

                if (parentheses.LastOrDefault() == '(')
                {
                    inParentheses += c;
                }

                if (c == ')')
                {
                    if (parentheses.Count == 0)
                    {
                        throw new FormatException($"括弧の対応が不正です。閉じ括弧が開き括弧よりも前に現れています。{counter}文字目を確認してください。");
                    }

                    parentheses.RemoveAt(parentheses.Count - 1);

                    if (parentheses.Count == 0)
                    {
                        prompt += TransformText(inParentheses);
                        inParentheses = string.Empty;
                        continue;
                    }
                }

                if (parentheses.Count == 0)
                {
                    prompt += c;
                }
            }

            var prompts = prompt.Split(',').Select(GetPrompt).ToList();
            return prompts;
        }

        private static string TransformText(string input)
        {
            // 先頭と末尾の括弧を除去
            input = input.Trim('(', ')');

            // 最後の要素に付与されたスコアを抽出
            var parts = input.Split(", ");
            var score = parts.Last().Contains(':') ? parts.Last().Split(":")[1] : "0.0";

            // スコアを取り除いた文字列を取得
            var elements = parts.Select(p => p.Split(':')[0]).ToArray();

            // 各要素にスコアを付けて変換
            var transformed = string.Join(", ", elements.Select(e => $"({e}:{score})"));

            return transformed;
        }

        private static Prompt GetPrompt(string prompt)
        {
            // 正規表現でマッチして抽出
            var match = Regex.Match(prompt, @"\(([^:]+):([0-9.]+)\)");

            if (match.Success)
            {
                var text = match.Groups[1].Value.Trim(); // テキスト部分
                var numberString = match.Groups[2].Value; // 数値部分（文字列として）
                var number = double.Parse(numberString); // 数値として取得

                return new Prompt() { Phrase = text, Strength = number, };
            }

            var isLora = Regex.Match(prompt, @"<lora:[^:>]+:(\d+(\.\d+)?)>");

            if (isLora.Success)
            {
                var strength = isLora.Groups[1].Value;

                // 末尾のスコア部分を削除し、Lora の名前だけを抽出してプロンプトに登録する。
                // 検出対象に山括弧が含まれるのは、 数値から始まる Lora名をスコアとして誤検出されるのを防ぐためです
                var exceptedScore = Regex.Replace(prompt, @":\d+(\.\d+)?>", string.Empty) + ">";
                if (double.TryParse(strength, out var num))
                {
                    return new Prompt()
                    {
                        Phrase = exceptedScore.Trim(), Strength = num, IsLora = true,
                    };
                }
            }

            return new Prompt() { Phrase = prompt.Trim(), };
        }
    }
}