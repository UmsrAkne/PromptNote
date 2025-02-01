using System;
using System.Collections.Generic;
using Prism.Mvvm;
using PromptNote.Models.Dbs;

namespace PromptNote.Models
{
    public class PromptGroup : BindableBase, IEntity
    {
        private string name;
        private int id;
        private string sampleImagePath;
        private DateTime createdAt;

        public int Id { get => id; set => SetProperty(ref id, value); }

        public string Name { get => name; set => SetProperty(ref name, value); }

        public string SampleImagePath
        {
            get => sampleImagePath;
            set => SetProperty(ref sampleImagePath, value);
        }

        public DateTime CreatedAt { get => createdAt; set => SetProperty(ref createdAt, value); }

        public List<Prompt> Prompts { get; set; } = new ();
    }
}