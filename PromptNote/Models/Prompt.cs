using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Prism.Mvvm;

namespace PromptNote.Models
{
    public class Prompt : BindableBase
    {
        private Phrase phrase = new ();
        private double strength = 1.0;
        private bool containsOutput = true;
        private int lineNumber;
        private int groupId;
        private int index;

        public Prompt(string phrase)
        {
            if (phrase is " " or "_")
            {
                Phrase = new Phrase(string.Empty);
                Type = PromptType.Empty;
                return;
            }

            if (Regex.IsMatch(phrase, "{.*|.*}"))
            {
                // ダイナミックプロンプトと判別された場合は、フレーズのフォーマットを整えて、ワードをソートします。
                // これは、フレーズの無駄な重複登録を防ぐための処理です。
                Type = PromptType.DynamicPrompt;
                var variableStr = Regex.Match(phrase, "{(.*)}").Groups[1].Value;
                var variables = variableStr
                    .ToLower()
                    .Split('|')
                    .Select(s => s.Trim())
                    .Distinct()
                    .OrderBy(s => s);

                var result = string.Join('|', variables);

                var p = Regex.Replace(phrase, "{.*}", $"{{{result}}}");
                Phrase = new Phrase(p);

                return;
            }

            Phrase = new Phrase(phrase);

            if (Regex.IsMatch(phrase, "<lora:.*>"))
            {
                Type = PromptType.Lora;
                return;
            }

            if (new[] { "\r\n", "\n", "\r", }.Contains(phrase))
            {
                Type = PromptType.LineBreak;
            }
        }

        public Prompt()
        {
        }

        public int Id { get; set; }

        public Phrase Phrase { get => phrase; set => SetProperty(ref phrase, value); }

        public double Strength { get => strength; set => SetProperty(ref strength, value); }

        public List<Tag> Tags { get; set; } = new ();

        public int GroupId { get => groupId; set => SetProperty(ref groupId, value); }

        public int Index { get => index; set => SetProperty(ref index, value); }

        public bool ContainsOutput { get => containsOutput; set => SetProperty(ref containsOutput, value); }

        public PromptType Type { get; set; } = PromptType.Normal;

        public int LineNumber { get => lineNumber; set => SetProperty(ref lineNumber, value); }

        public override string ToString()
        {
            if (Type == PromptType.Lora)
            {
                var p = Phrase.Value.Replace(">", string.Empty);
                return $"{p}:{Strength}>";
            }

            if (Type == PromptType.LineBreak)
            {
                return Environment.NewLine;
            }

            if (Type == PromptType.Empty)
            {
                return string.Empty;
            }

            return Phrase.Value;
        }
    }
}