using Prism.Mvvm;

namespace PromptNote.Models
{
    public class Phrase : BindableBase
    {
        private string val = string.Empty;
        private string comment;

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

        public string Comment { get => comment; set => SetProperty(ref comment, value); }
    }
}