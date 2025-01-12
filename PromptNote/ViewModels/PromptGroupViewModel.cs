using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using PromptNote.Models;
using PromptNote.Models.Dbs;
using PromptNote.Views;

namespace PromptNote.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PromptGroupViewModel : BindableBase
    {
        private readonly IDialogService dialogService;
        private PromptGroup selectedItem;
        private string inputName;

        public PromptGroupViewModel(IDialogService dialogService, IContainerProvider containerProvider)
        {
            this.dialogService = dialogService;

            // xaml プレビューで使われるデフォルトコンストラクタで作られた、当インスタンスは containerProvider に null が入力される。
            if (containerProvider != null)
            {
                PromptGroupRepository = containerProvider.Resolve<IPromptGroupRepository>();
            }
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

        public DelegateCommand ShowRenameDialogCommand => new DelegateCommand(() =>
        {
            if (SelectedItem == null)
            {
                return;
            }

            var param = new DialogParameters { { nameof(TextInputPageViewModel.Text), SelectedItem.Name }, };
            dialogService.ShowDialog(nameof(TextInputPage), param, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedItem.Name = result.Parameters.GetValue<string>(nameof(TextInputPageViewModel.Text));
                }
            });
        });

        private IPromptGroupRepository PromptGroupRepository { get; }
    }
}