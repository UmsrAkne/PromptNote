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
        private ObservableCollection<Prompt> prompts;

        public PromptsViewModel()
        {
            Prompts = new ObservableCollection<Prompt>();
        }

        public ObservableCollection<Prompt> Prompts
        {
            get => prompts;
            set
            {
                CursorManager.Items = value;
                SetProperty(ref prompts, value);
            }
        }

        public CursorManager CursorManager { get; } = new ();

        /// <summary>
        /// このビューモデルが持つ Prompts から、実際に入力可能な形式のテキストを生成してクリップボードにコピーします。
        /// </summary>
        public DelegateCommand GeneratePromptCommand => new DelegateCommand(() =>
        {
            var prs = Prompts.Where(p => p.ContainsOutput).Select(p => p.ToString());
            Clipboard.SetText(string.Join(string.Empty, prs));
        });
    }
}