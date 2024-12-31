using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using PromptNote.Models;

namespace PromptNote.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PromptsViewModel : BindableBase
    {
        public ObservableCollection<Prompt> Prompts { get; set; } = new ();

        /// <summary>
        /// このビューモデルが持つ Prompts から、実際に入力可能な形式のテキストを生成してクリップボードにコピーします。
        /// </summary>
        public DelegateCommand GeneratePromptCommand => new DelegateCommand(() =>
        {
            var prs = Prompts.Select(p => p.Phrase);
            Clipboard.SetText(string.Join(',', prs).Replace(",", ", "));
        });
    }
}