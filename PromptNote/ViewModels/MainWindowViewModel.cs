using Prism.Mvvm;
using PromptNote.Models;

namespace PromptNote.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public TextWrapper TextWrapper { get; set; } = new ();

        public MainWindowViewModel()
        {
        }
    }
}