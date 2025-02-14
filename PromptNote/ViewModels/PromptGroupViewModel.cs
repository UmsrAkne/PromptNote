using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
        private ReadOnlyObservableCollection<PromptGroup> promptGroups = new (new ObservableCollection<PromptGroup>());

        public PromptGroupViewModel(IDialogService dialogService, IContainerProvider containerProvider)
        {
            this.dialogService = dialogService;

            // xaml プレビューで使われるデフォルトコンストラクタで作られた、当インスタンスは containerProvider に null が入力される。
            if (containerProvider != null)
            {
                PromptGroupService = containerProvider.Resolve<PromptGroupService>();
                PromptGroups = new ReadOnlyObservableCollection<PromptGroup>(OriginalPromptGroups);
            }
        }

        public ReadOnlyObservableCollection<PromptGroup> PromptGroups
        {
            get => promptGroups;
            private set => SetProperty(ref promptGroups, value);
        }

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

        public PromptGroupService PromptGroupService { get; set; }

        /// <summary>
        /// InputName に入力中のテキストを名前とした PromptGroup をリストに追加し、InputName を空文字で初期化します。
        /// </summary>
        public AsyncDelegateCommand AddGroupAsyncCommand => new AsyncDelegateCommand(async () =>
        {
            var pg = new PromptGroup() { Name = InputName, CreatedAt = DateTime.Now, };
            await AddGroupAsync(pg);
            InputName = string.Empty;
            await LoadGroupsAsyncCommand.ExecuteAsync();
        });

        public DelegateCommand<string> LoadImageCommand => new DelegateCommand<string>(path =>
        {
            if (SelectedItem == null)
            {
                return;
            }

            SelectedItem.SampleImagePath = path;
            _ = SaveAsyncCommand.ExecuteAsync();
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

        public AsyncDelegateCommand SaveAsyncCommand => new AsyncDelegateCommand(() => Task.CompletedTask);

        public AsyncDelegateCommand LoadGroupsAsyncCommand => new AsyncDelegateCommand(async () =>
        {
            if (PromptGroupService != null)
            {
                var r = await PromptGroupService.GetAllAsync();
                OriginalPromptGroups = new ObservableCollection<PromptGroup>(r);
                PromptGroups = new ReadOnlyObservableCollection<PromptGroup>(OriginalPromptGroups);
            }
        });

        private ObservableCollection<PromptGroup> OriginalPromptGroups { get; set; } = new ();

        public async Task AddGroupAsync(PromptGroup group)
        {
            group.CreatedAt = DateTime.Now;
            OriginalPromptGroups.Add(group);
            await PromptGroupService.AddAsync(group);
        }

        public async Task AddGroupsAsync(IEnumerable<PromptGroup> groups)
        {
            var list = groups.ToList();
            OriginalPromptGroups.AddRange(list);
            await PromptGroupService.AddRangeAsync(list);
        }
    }
}