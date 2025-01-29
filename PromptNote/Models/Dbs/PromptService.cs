using System.Collections.Generic;

namespace PromptNote.Models.Dbs
{
    public class PromptService
    {
        public IRepository<Prompt> Repository { get; set; } = new JsonRepository<Prompt>("prompts.json");

        public void SaveChanges()
        {
        }

        public IEnumerable<Prompt> LoadPromptsByGroupId()
        {
            return new List<Prompt>();
        }
    }
}