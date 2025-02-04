using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromptNote.Models.Dbs
{
    public interface IRepository<T>
    where T : class
    {
        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}