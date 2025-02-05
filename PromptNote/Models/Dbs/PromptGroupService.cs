using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromptNote.Models.Dbs
{
    public class PromptGroupService
    {
        private IRepository<PromptGroup> Repository { get; set; } = new JsonRepository<PromptGroup>("promptGroups.json");

        public void SaveChanges()
        {
        }

        public async Task AddAsync(PromptGroup promptGroup)
        {
            await Repository.AddAsync(promptGroup);
        }

        public async Task AddRangeAsync(IEnumerable<PromptGroup> promptGroup)
        {
            await Repository.AddRangeAsync(promptGroup);
        }

        public async Task<IEnumerable<PromptGroup>> GetAllAsync()
        {
            return await Repository.GetAllAsync();
        }

        public IEnumerable<PromptGroup> LoadPromptsByGroupId()
        {
            return new List<PromptGroup>();
        }
    }
}