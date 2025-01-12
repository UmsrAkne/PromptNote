using System;
using System.Collections.Generic;
using Prism.Mvvm;

namespace PromptNote.Models
{
    public class Prompt : BindableBase
    {
        private Phrase phrase = new ();
        private double strength = 1.0;
        private bool containsOutput = true;

        public Phrase Phrase { get => phrase; set => SetProperty(ref phrase, value); }

        public double Strength { get => strength; set => SetProperty(ref strength, value); }

        public List<Tag> Tags { get; set; } = new ();

        public bool ContainsOutput { get => containsOutput; set => SetProperty(ref containsOutput, value); }

        public PromptType Type { get; set; } = PromptType.Normal;

        public override string ToString()
        {
            if (Type == PromptType.Lora)
            {
                var p = Phrase.Value.Replace(">", string.Empty);
                return $"{p}:{Strength}>, ";
            }

            if (Type == PromptType.LineBreak)
            {
                return Environment.NewLine;
            }

            return $"{Phrase.Value}, ";
        }
    }
}