using System.Collections.ObjectModel;
using Prism.Mvvm;
using PromptNote.Models;

namespace PromptNote.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PromptsViewModel : BindableBase
    {
        public ObservableCollection<Prompt> Prompts { get; set; } = new ();
    }
}