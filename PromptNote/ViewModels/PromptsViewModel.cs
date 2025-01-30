using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using PromptNote.Models;
using PromptNote.Models.Dbs;

namespace PromptNote.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PromptsViewModel : BindableBase
    {
        private ReadOnlyObservableCollection<Prompt> prompts;

        public PromptsViewModel()
        {
            Prompts = new ReadOnlyObservableCollection<Prompt>(OriginalItems);
        }

        public ReadOnlyObservableCollection<Prompt> Prompts
        {
            get => prompts;
            private set
            {
                CursorManager.Items = OriginalItems;
                SetProperty(ref prompts, value);
            }
        }

        public CursorManager CursorManager { get; } = new ();

        /// <summary>
        /// このビューモデルが持つ Prompts から、実際に入力可能な形式のテキストを生成してクリップボードにコピーします。
        /// </summary>
        public DelegateCommand GeneratePromptCommand => new DelegateCommand(() =>
        {
            var prs = Prompts.Where(p => p.ContainsOutput);
            Clipboard.SetText(PromptsFormatter.Format(prs.ToList()));
        });

        /// <summary>
        /// Prompt のリストを改行で区切り、その区間内で ContainsOutput の値に応じて並び替えます。<br/>
        /// 具体的には出力する値は行頭側に、出力されない値は行末側に安定ソートします。
        /// </summary>
        public DelegateCommand SortByOutputSettingCommand => new DelegateCommand(() =>
        {
            var lists = PromptMerger.SplitPrompts(Prompts.ToList());
            var results = new List<Prompt>();
            foreach (var ps in lists)
            {
                var existsLineBreak = ps.Any(p => p.Type == PromptType.LineBreak);
                var withoutLineBreak = ps.Where(p => p.Type != PromptType.LineBreak)
                    .OrderByDescending(p => p.ContainsOutput);

                results.AddRange(withoutLineBreak);
                if (existsLineBreak)
                {
                    results.Add(new Prompt(Environment.NewLine));
                }
            }

            SetItems(new ObservableCollection<Prompt>(results));
        });

        private ObservableCollection<Prompt> OriginalItems { get; set; } = new ();

        private PromptService PromptService { get; set; } = new ();

        public void SetItems(ObservableCollection<Prompt> items)
        {
            OriginalItems = items;
            Prompts = new ReadOnlyObservableCollection<Prompt>(OriginalItems);
            ReIndex();
        }

        public void AddItem(Prompt item)
        {
            OriginalItems.Add(item);
            ReIndex();
        }

        public void ReIndex()
        {
            for (var i = 0; i < Prompts.Count; i++)
            {
                Prompts[i].LineNumber = i + 1;
            }
        }
    }
}