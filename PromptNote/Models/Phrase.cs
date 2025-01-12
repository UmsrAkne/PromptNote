using Prism.Mvvm;

namespace PromptNote.Models
{
    public class Phrase : BindableBase
    {
        private string val = string.Empty;

        public Phrase(string phrase)
        {
            Value = phrase;
        }

        public Phrase()
        {
        }

        public string Value
        {
            get => val;
            set => SetProperty(ref val, value);
        }
    }
}