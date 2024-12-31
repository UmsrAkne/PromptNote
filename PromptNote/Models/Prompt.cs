using System.Collections.Generic;

namespace PromptNote.Models
{
    public class Prompt
    {
        public string Phrase { get; set; } = string.Empty;

        public double Strength { get; set; } = 1.0;

        public List<string> Tags { get; set; } = new ();
    }
}