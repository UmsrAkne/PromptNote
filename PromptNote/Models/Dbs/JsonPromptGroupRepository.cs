using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PromptNote.Models.Dbs
{
    public class JsonPromptGroupRepository : IPromptGroupRepository
    {
        private readonly string filePath;
        private readonly List<PromptGroup> items;

        public JsonPromptGroupRepository(string filePath)
        {
            this.filePath = filePath;
            items = LoadFromFile();
        }

        public Task<PromptGroup> GetByIdAsync(int id)
        {
            return Task.FromResult(items.FirstOrDefault(pg => pg.Id == id));
        }

        public Task<IEnumerable<PromptGroup>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<PromptGroup>>(items);
        }

        public Task AddAsync(PromptGroup promptGroup)
        {
            items.Add(promptGroup);
            SaveToFile();
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            SaveToFile();
            return Task.CompletedTask;
        }

        private List<PromptGroup> LoadFromFile()
        {
            if (!File.Exists(filePath))
            {
                return new List<PromptGroup>();
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<PromptGroup>>(json) ?? new List<PromptGroup>();
        }

        private void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}