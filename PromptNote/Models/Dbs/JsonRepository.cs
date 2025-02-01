using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PromptNote.Models.Dbs
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class JsonRepository<T> : IRepository<T>, IDisposable
    where T : class, IEntity
    {
        private readonly string filePath;

        private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

        public JsonRepository(string filePath)
        {
            this.filePath = filePath;

            if (!File.Exists(this.filePath))
            {
                var path = this.filePath;

                if (path != null)
                {
                    File.WriteAllText(path, "[]");
                }
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var data = await LoadDataAsync();
            return data.FirstOrDefault(e => GetId(e) == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await LoadDataAsync();
        }

        public async Task AddAsync(T entity)
        {
            Debug.WriteLine($"{GetId(entity)}, {entity}");

            var newId = GetId(entity);
            await semaphoreSlim.WaitAsync();

            try
            {
                var data = await LoadDataAsync();

                if (data.Any(d => GetId(d) == newId))
                {
                    Debug.WriteLine($"入力したアイテムの ID が重複しています。ID={newId}");
                    return;
                }

                data.Add(entity);
                await SaveDataAsync(data);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task UpdateAsync(T entity)
        {
            var data = await LoadDataAsync();
            var id = GetId(entity);
            var existing = data.FirstOrDefault(e => GetId(e) == id);
            if (existing != null)
            {
                data.Remove(existing);
                data.Add(entity);
                await SaveDataAsync(data);
            }
        }

        public async Task DeleteAsync(T entity)
        {
            var data = await LoadDataAsync();
            data.RemoveAll(e => GetId(e) == GetId(entity));
            await SaveDataAsync(data);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            semaphoreSlim.Dispose();
        }

        private int GetId(T entity)
        {
            var property = typeof(T).GetProperty("Id");
            if (property != null && property.PropertyType == typeof(int))
            {
                return (int)property.GetValue(entity)!;
            }

            throw new InvalidOperationException("Entity must have an Id property of type int.");
        }

        private async Task<List<T>> LoadDataAsync()
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            try
            {
                // return new List<T>();
                var json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        private async Task SaveDataAsync(List<T> data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true, });
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}