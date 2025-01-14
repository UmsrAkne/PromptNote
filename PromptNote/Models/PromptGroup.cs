using System.Collections.Generic;
using Prism.Mvvm;

namespace PromptNote.Models
{
    public class PromptGroup : BindableBase
    {
        private string name;
        private int id;
        private string sampleImagePath;

        public int Id { get => id; set => SetProperty(ref id, value); }

        public string Name { get => name; set => SetProperty(ref name, value); }

        public string SampleImagePath
        {
            get => sampleImagePath;
            set => SetProperty(ref sampleImagePath, value);
        }

        public List<Prompt> Prompts { get; set; } = new ();
    }
}