using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PromptNote.Models;

namespace PromptNote.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PromptGroupViewModel : BindableBase
    {
        private PromptGroup selectedItem;
        private string inputName;
        private readonly IDialogService dialogService;

        public PromptGroupViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public ObservableCollection<PromptGroup> PromptGroups { get; set; } = new ();

        public PromptGroup SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }

        public string InputName
        {
            get => inputName;
            set
            {
                if (SetProperty(ref inputName, value))
                {
                    RaisePropertyChanged(nameof(CanGroupAddition));
                }
            }
        }

        public bool CanGroupAddition => !string.IsNullOrWhiteSpace(InputName);

        /// <summary>
        /// InputName に入力中のテキストを名前とした PromptGroup をリストに追加し、InputName を空文字で初期化します。
        /// </summary>
        public DelegateCommand AddGroupCommand => new (() =>
        {
            PromptGroups.Add(new PromptGroup() { Name = InputName, });
            InputName = string.Empty;
        });
    }
}