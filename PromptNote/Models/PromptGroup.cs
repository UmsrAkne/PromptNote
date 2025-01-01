using System.Collections.Generic;
using Prism.Mvvm;

namespace PromptNote.Models
{
    public class PromptGroup : BindableBase
    {
        public string Name { get; set; }
        
        public IEnumerable<Prompt> Prompts { get; set; }
    }
}