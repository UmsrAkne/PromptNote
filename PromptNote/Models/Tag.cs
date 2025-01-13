using System.Windows.Media;
using Prism.Mvvm;

namespace PromptNote.Models
{
    public class Tag : BindableBase
    {
        private string val;
        private string colorName = nameof(Colors.Black);

        public string Value { get => val; set => SetProperty(ref val, value); }

        public string ColorName { get => colorName; set => SetProperty(ref colorName, value); }

        public override string ToString()
        {
            return Value;
        }
    }
}