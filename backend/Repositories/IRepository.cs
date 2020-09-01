using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace backend.Data
{
    public interface IRepository
    {
        Task<EntityEntry<T>> AddAsync<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAllAsync();
    }
}