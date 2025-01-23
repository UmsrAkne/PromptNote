using System;
using System.Collections.Generic;
using System.Linq;

namespace PromptNote.Models
{
    public static class PromptsFormatter
    {
        public static string Format(List<Prompt> prompts)
        {
            var inParentheses = new List<Prompt>();
            var output = string.Empty;

            foreach (var prompt in prompts.Where(prompt => prompt is { ContainsOutput: true, }))
            {
                if (prompt.Type == PromptType.Lora)
                {
                    output += FormatPromptsGroup(inParentheses);
                    inParentheses.Clear();
                    output += $"{prompt}, ";
                    continue;
                }

                if (Math.Abs(prompt.Strength - 1.0) < 0.01)
                {
                    if (inParentheses.Count != 0)
                    {
                        output += FormatPromptsGroup(inParentheses);
                        inParentheses.Clear();
                    }

                    output += $"{prompt.Phrase.Value}, ";
                }
                else
                {
                    inParentheses.Add(prompt);
                }
            }

            if (inParentheses.Count != 0)
            {
                output += FormatPromptsGroup(inParentheses);
            }

            if (string.IsNullOrWhiteSpace(output))
            {
                return string.Empty;
            }

            return output.Substring(0, output.Length - 2);
        }

        private static string FormatPromptsGroup(List<Prompt> prompts)
        {
            if (prompts.Count == 0)
            {
                return string.Empty;
            }

            var str = string.Join(", ", prompts.Select(p => p.Phrase.Value));
            return $"({str}:{prompts.First().Strength}), ";
        }
    }
}