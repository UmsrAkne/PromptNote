using System.Collections.Generic;
using System.Linq;
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
            var list = await Repository.GetAllAsync();
            if (promptGroup.Id == 0)
            {
                var maxId = list.OrderByDescending(g => g.Id).Select(g => g.Id).FirstOrDefault();
                promptGroup.Id = maxId + 1;
            }

            await Repository.AddAsync(promptGroup);
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