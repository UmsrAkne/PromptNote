using System;
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
            if (item.GroupId == 0)
            {
                throw new ArgumentException($"Group id cannot be 0. Prompt = {item}");
            }

            await Repository.AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<Prompt> items)
        {
            var enumerable = items.ToList();
            if (enumerable.Any(i => i.GroupId == 0))
            {
                throw new ArgumentException($"Group id cannot be 0 (AddRangeAsync)");
            }

            await Repository.AddRangeAsync(enumerable);
        }

        public async Task<IEnumerable<Prompt>> LoadPromptsByGroupId(int groupId)
        {
            var all = await Repository.GetAllAsync();
            var filtering = all.Where(p => p.GroupId == groupId);
            return filtering;
        }
    }
}