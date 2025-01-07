using Prism.Ioc;
using PromptNote.Views;
using System.Windows;
using PromptNote.ViewModels;

namespace PromptNote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<TextInputPage, TextInputPageViewModel>();
        }
    }
}