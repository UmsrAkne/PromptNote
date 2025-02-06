using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using PromptNote.Models;

namespace PromptNote.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private Prompt inputPrompt = new ();
        private string inputText = string.Empty;

        public MainWindowViewModel(IContainerProvider containerProvider)
        {
            PromptGroupViewModel = containerProvider.Resolve<PromptGroupViewModel>();
            SetDummies();
        }

        [Obsolete("Xaml のプレビューを正しく表示するためのデフォルトコンストラクタです。それ以外の用途で使わないでください。")]
        public MainWindowViewModel()
        {
            PromptGroupViewModel = new PromptGroupViewModel(null, null);
            SetDummies();
        }

        public TextWrapper TextWrapper { get; set; } = new ();

        public PromptsViewModel PromptsViewModel { get; set; } = new ();

        public PromptGroupViewModel PromptGroupViewModel { get; set; }

        public Prompt InputPrompt { get => inputPrompt; set => SetProperty(ref inputPrompt, value); }

        public string InputText { get => inputText; set => SetProperty(ref inputText, value); }

        /// <summary>
        /// 入力中のプロンプトを PromptsViewModel のリストに追加します。追加に成功した場合は入力済みのプロンプトの情報を初期化します。
        /// </summary>
        public AsyncDelegateCommand AddPromptCommand => new (async () =>
        {
            if (InputPrompt == null || string.IsNullOrWhiteSpace(InputPrompt.Phrase.Value))
            {
                return;
            }

            var p = PromptParser.ParseWithLineBreaks(InputPrompt.Phrase.Value).FirstOrDefault();

            if (p != null)
            {
                if (PromptGroupViewModel.SelectedItem != null)
                {
                    p.GroupId = PromptGroupViewModel.SelectedItem.Id;
                }

                await PromptsViewModel.AddItemAsync(p);
            }

            InputPrompt = new Prompt();
        });

        public AsyncDelegateCommand MergePromptsCommand => new AsyncDelegateCommand(async () =>
        {
            if (string.IsNullOrWhiteSpace(InputText))
            {
                return;
            }

            var newList = PromptParser.ParseWithLineBreaks(InputText);
            var oldList = PromptsViewModel.Prompts.ToList();
            var merged = PromptMerger.MergePrompts(oldList, newList);
            var id = PromptGroupViewModel.SelectedItem?.Id ?? 0;

            foreach (var m in merged)
            {
                m.GroupId = id;
            }

            await PromptsViewModel.SetItemsAsync(
                new ObservableCollection<Prompt>(merged));

            InputText = string.Empty;
        });

        /// <summary>
        /// PromptsViewModel.Prompts をアップデートします。<br/>
        /// このコマンドはプロンプトグループのリストのセレクションが変更された時(イベントトリガー)に実行します。
        /// </summary>
        public AsyncDelegateCommand UpdatePromptsAsyncCommand => new (async () =>
        {
            if (PromptGroupViewModel.SelectedItem != null)
            {
                var list = await PromptsViewModel.PromptService.LoadPromptsByGroupId(PromptGroupViewModel.SelectedItem.Id);
                PromptsViewModel.SetItems(new ObservableCollection<Prompt>(list));
            }
        });

        public AsyncDelegateCommand AppInitializeAsyncCommand => new AsyncDelegateCommand(async () =>
        {
            if (PromptGroupViewModel.PromptGroupService != null)
            {
                var r = await PromptGroupViewModel.PromptGroupService.GetAllAsync();
                await PromptGroupViewModel.AddGroupsAsync(r);
            }
        });

        [Conditional("DEBUG")]
        private async void SetDummies()
        {
            await PromptsViewModel.AddItemAsync(new Prompt()
            {
                Id = 1,
                Phrase = new Phrase("test1"),
                GroupId = 1,
            });

            await PromptsViewModel.AddItemAsync(new Prompt()
            {
                Id = 2,
                GroupId = 1,
                Phrase = new Phrase("test2"),
                Tags = new List<Tag>() { new () { Value = "Tag1", }, },
            });

            await PromptsViewModel.AddItemAsync(new Prompt()
            {
                Id = 3,
                GroupId = 1,
                Phrase = new Phrase("test3"),
                Tags = new List<Tag>() { new () { Value = "Tag1", }, new () { Value = "Tag2", }, },
                ContainsOutput = false,
            });

            await PromptsViewModel.AddItemAsync(new Prompt()
            {
                Id = 4,
                GroupId = 1,
                Phrase = new Phrase("test4"),
            });

            await PromptsViewModel.AddItemAsync(new Prompt()
            {
                Id = 5,
                GroupId = 1,
                Phrase = new Phrase("test5longLongLongLongLongLongLongText"),
            });

            await PromptsViewModel.AddItemAsync(new Prompt()
            {
                Id = 6,
                GroupId = 1,
                Phrase = new Phrase("test6"),
            });

            await PromptsViewModel.AddItemAsync(new Prompt()
            {
                Id = 7,
                GroupId = 1,
                Phrase = new Phrase("test7"),
                Tags = new List<Tag>() { new () { Value = "RedTag1", ColorName = "Red", }, new () { Value = "Tag2", }, },
                ContainsOutput = false,
            });

            await PromptsViewModel.AddItemAsync(new Prompt("{dynamic|prompt}") { Id = 8, GroupId = 1, });
            await PromptsViewModel.AddItemAsync(new Prompt("\r\n") { Id = 9, GroupId = 1, });
            await PromptsViewModel.AddItemAsync(new Prompt("{dynamic|prompt2}") { Id = 10, GroupId = 1,  });
            await PromptsViewModel.AddItemAsync(new Prompt("\r\n") { Id = 11, GroupId = 1, });
            await PromptsViewModel.AddItemAsync(new Prompt("\r\n") { Id = 12, GroupId = 1, });
            await PromptsViewModel.AddItemAsync(new Prompt("{dynamic|prompt3}") { Id = 13, GroupId = 1, });

            PromptsViewModel.ReIndex();

            var groups = new List<PromptGroup>();
            groups.Add(new PromptGroup { Name = "Test Group1", Id = 2, });
            groups.Add(new PromptGroup { Name = "Test Group30", Id = 3, });
            groups.Add(new PromptGroup
                {
                    Name = "Test Group2", Id = 4, Prompts = new List<Prompt>()
                    {
                        new ()
                        {
                            Id = 14,
                            Phrase = new Phrase("PromptGroup text1"),
                            Tags = new List<Tag>() { new () { Value = "Tag1", }, new () { Value = "Tag2", }, },
                            ContainsOutput = false,
                        },

                        new ()
                        {
                            Id = 15,
                            Phrase = new Phrase("PromptGroup text2"),
                            Tags = new List<Tag>() { new () { Value = "Tag1", }, new () { Value = "Tag2", }, },
                            ContainsOutput = false,
                        },
                    },
                });

            await PromptGroupViewModel.AddGroupsAsync(groups);
            await PromptGroupViewModel.LoadGroupsAsyncCommand.ExecuteAsync();
            PromptGroupViewModel.SelectedItem = PromptGroupViewModel.PromptGroups[0];

            await UpdatePromptsAsyncCommand.ExecuteAsync();
        }
    }
}