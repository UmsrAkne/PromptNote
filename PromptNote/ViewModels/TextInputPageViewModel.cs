using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PromptNote.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TextInputPageViewModel : BindableBase, IDialogAware
    {
        private string text = string.Empty;

        public event Action<IDialogResult> RequestClose;

        public string Title => string.Empty;

        public string Text { get => text; set => SetProperty(ref text, value); }

        public DelegateCommand CloseCommand => new (() =>
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        });

        public DelegateCommand ConfirmCommand => new (() =>
        {
            var result = new DialogResult(ButtonResult.OK);
            result.Parameters.Add(nameof(Text), Text);
            RequestClose?.Invoke(result);
        });

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(nameof(Text)))
            {
                Text = parameters.GetValue<string>(nameof(Text));
            }
        }
    }
}