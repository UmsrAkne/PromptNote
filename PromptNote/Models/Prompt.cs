using System.Collections.Generic;
using Prism.Mvvm;

namespace PromptNote.Models
{
    public class Prompt : BindableBase
    {
        private string phrase = string.Empty;
        private double strength = 1.0;
        private bool containsOutput = true;

        public string Phrase { get => phrase; set => SetProperty(ref phrase, value); }

        public double Strength { get => strength; set => SetProperty(ref strength, value); }

        public List<string> Tags { get; set; } = new ();

        public bool ContainsOutput { get => containsOutput; set => SetProperty(ref containsOutput, value); }

        public PromptType Type { get; set; } = PromptType.Normal;
    }
}