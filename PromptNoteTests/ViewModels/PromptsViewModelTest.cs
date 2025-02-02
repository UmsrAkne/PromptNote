using System.Collections.ObjectModel;
using PromptNote.Models;
using PromptNote.ViewModels;

namespace PromptNoteTests.ViewModels
{
    [TestFixture]
    public class PromptsViewModelTest
    {
        [Test]
        public void SortByOutputSettingCommandTest()
        {
            var vm = new PromptsViewModel();
            var list = new List<Prompt>()
            {
                new ("a"),
                new ("b") { ContainsOutput = false, },
                new ("c"),
                new (Environment.NewLine),
                new ("d"),
                new ("e") { ContainsOutput = false, },
                new ("f") { ContainsOutput = false, },
                new ("g"),
            };

            vm.SetItems(new ObservableCollection<Prompt>(list));
            vm.SortByOutputSettingCommand.Execute();
            var actual = vm.Prompts.Select(p => p.ToString());
            var excepted = new[] { "a", "c", "b", Environment.NewLine, "d", "g", "e", "f", };

            CollectionAssert.AreEqual(excepted, actual);
        }
    }
}