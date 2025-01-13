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
        public DelegateCommand AddPromptCommand => new DelegateCommand(() =>
        {
            if (InputPrompt == null || string.IsNullOrWhiteSpace(InputPrompt.Phrase.Value))
            {
                return;
            }

            var p = PromptParser.Parse(InputPrompt.Phrase.Value).FirstOrDefault();

            if (p != null)
            {
                PromptsViewModel.Prompts.Add(p);
            }

            InputPrompt = new Prompt();
        });

        public DelegateCommand MergePromptsCommand => new DelegateCommand(() =>
        {
            if (string.IsNullOrWhiteSpace(InputText))
            {
                return;
            }

            var newList = PromptParser.Parse(InputText);
            var oldList = PromptsViewModel.Prompts.ToList();

            PromptsViewModel.Prompts
                = new ObservableCollection<Prompt>(PromptMerger.MergePrompts(oldList, newList));

            InputText = string.Empty;
        });

        /// <summary>
        /// PromptsViewModel.Prompts をアップデートします。<br/>
        /// このコマンドはプロンプトグループのリストのセレクションが変更された時(イベントトリガー)に実行します。
        /// </summary>
        public DelegateCommand UpdatePromptsCommand => new DelegateCommand(() =>
        {
            if (PromptGroupViewModel.SelectedItem != null)
            {
                PromptsViewModel.Prompts = new ObservableCollection<Prompt>(PromptGroupViewModel.SelectedItem.Prompts);
            }
        });

        [Conditional("DEBUG")]
        private void SetDummies()
        {
            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = new Phrase("test1"), });

            PromptsViewModel.Prompts.Add(new Prompt()
                { Phrase = new Phrase("test2"), Tags = new List<Tag>() { new () { Value = "Tag1", }, }, });

            PromptsViewModel.Prompts.Add(new Prompt()
            {
                Phrase = new Phrase("test3"), Tags = new List<Tag>() { new () { Value = "Tag1", }, new () { Value = "Tag2", }, },
                ContainsOutput = false,
            });
            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = new Phrase("test4"), });
            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = new Phrase("test5longLongLongLongLongLongLongText"), });
            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = new Phrase("test6"), });

            PromptsViewModel.Prompts.Add(new Prompt()
            {
                Phrase = new Phrase("test7"), Tags = new List<Tag>() { new () { Value = "RedTag1", ColorName = "Red", }, new () { Value = "Tag2", }, },
                ContainsOutput = false,
            });

            PromptGroupViewModel.PromptGroups.Add(new PromptGroup() { Name = "Test Group1", });
            PromptGroupViewModel.PromptGroups.Add(
                new PromptGroup()
                {
                    Name = "Test Group2", Prompts = new List<Prompt>()
                    {
                        new ()
                        {
                            Phrase = new Phrase("PromptGroup text1"), Tags = new List<Tag>() { new () { Value = "Tag1", }, new () { Value = "Tag2", }, },
                            ContainsOutput = false,
                        },

                        new ()
                        {
                            Phrase = new Phrase("PromptGroup text2"), Tags = new List<Tag>() { new () { Value = "Tag1", }, new () { Value = "Tag2", }, },
                            ContainsOutput = false,
                        },
                    },
                });

            PromptGroupViewModel.PromptGroups.Add(new PromptGroup() { Name = "Test Group3", });
        }
    }
}