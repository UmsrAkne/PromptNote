using System.Windows;
using Prism.Ioc;
using PromptNote.Models.Dbs;
using PromptNote.ViewModels;
using PromptNote.Views;

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
            containerRegistry.RegisterInstance<IPromptGroupRepository>(new JsonPromptGroupRepository("PromptGroups.json"));
        }
    }
}