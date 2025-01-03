﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Prism.Commands;
using Prism.Mvvm;
using PromptNote.Models;

namespace PromptNote.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private Prompt inputPrompt = new ();

        public TextWrapper TextWrapper { get; set; } = new ();

        public PromptsViewModel PromptsViewModel { get; set; } = new ();

        public PromptGroupViewModel PromptGroupViewModel { get; set; } = new ();

        public Prompt InputPrompt { get => inputPrompt; set => SetProperty(ref inputPrompt, value); }

        public MainWindowViewModel()
        {
            SetDummies();
        }

        /// <summary>
        /// 入力中のプロンプトを PromptsViewModel のリストに追加します。追加に成功した場合は入力済みのプロンプトの情報を初期化します。
        /// </summary>
        public DelegateCommand AddPromptCommand => new DelegateCommand(() =>
        {
            if (InputPrompt == null || string.IsNullOrWhiteSpace(InputPrompt.Phrase))
            {
                return;
            }

            PromptsViewModel.Prompts.Add(InputPrompt);
            InputPrompt = new Prompt();
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
            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = "test1", });

            PromptsViewModel.Prompts.Add(new Prompt()
                { Phrase = "test2", Tags = new List<string>() { "Tag1", }, });

            PromptsViewModel.Prompts.Add(new Prompt()
                { Phrase = "test3", Tags = new List<string>() { "Tag1", "Tag2", }, ContainsOutput = false, });

            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = "test4", });
            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = "test5longLongLongLongLongLongLongText", });
            PromptsViewModel.Prompts.Add(new Prompt() { Phrase = "test6", });

            PromptGroupViewModel.PromptGroups.Add(new PromptGroup() {Name = "Test Group1", });
            PromptGroupViewModel.PromptGroups.Add(
                new PromptGroup()
                {
                    Name = "Test Group2", Prompts = new List<Prompt>()
                    {
                        new ()
                        {
                            Phrase = "PromptGroup text1", Tags = new List<string>() { "Tag1", "Tag2", },
                            ContainsOutput = false,
                        },

                        new ()
                        {
                            Phrase = "PromptGroup text2", Tags = new List<string>() { "Tag1", "Tag2", },
                            ContainsOutput = false,
                        },
                    },
                });

            PromptGroupViewModel.PromptGroups.Add(new PromptGroup() {Name = "Test Group3", });
        }
    }
}