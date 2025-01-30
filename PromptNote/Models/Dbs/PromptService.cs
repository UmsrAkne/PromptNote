using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromptNote.Models.Dbs
{
    public class PromptService
    {
        private IRepository<Prompt> Repository { get; set; } = new JsonRepository<Prompt>("prompts.json");

        public void SaveChanges()
        {
        }

        public async Task AddAsync(Prompt item)
        {
            await Repository.AddAsync(item);
        }

        public async Task<IEnumerable<Prompt>> LoadPromptsByGroupId(int groupId)
        {
            var all = await Repository.GetAllAsync();
            var filtering = all.Where(p => p.GroupId == groupId);
            return filtering;
        }
    }
}