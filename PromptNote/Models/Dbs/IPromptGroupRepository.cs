using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromptNote.Models.Dbs
{
    public interface IPromptGroupRepository
    {
        Task<PromptGroup> GetByIdAsync(int id);

        Task<IEnumerable<PromptGroup>> GetAllAsync();

        Task AddAsync(PromptGroup promptGroup);
    }
}