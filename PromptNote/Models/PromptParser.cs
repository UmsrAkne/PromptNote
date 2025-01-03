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

            foreach (var c in text)
            {
                counter++;

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

            return new Prompt() { Phrase = prompt.Trim(), };
        }
    }
}