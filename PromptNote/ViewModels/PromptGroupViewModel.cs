using System.Collections.ObjectModel;
using Prism.Mvvm;
using PromptNote.Models;

namespace PromptNote.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PromptGroupViewModel : BindableBase
    {
        private PromptGroup selectedItem;

        public ObservableCollection<PromptGroup> PromptGroups { get; set; } = new ();

        public PromptGroup SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }
    }
}