using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Infrastructure.Interfaces
{
    // Generički CRUD interfejs za entitete
    public interface IBaseRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken token = default);
        Task<T?> GetByIdAsync(int id, CancellationToken token = default);
        Task AddAsync(T entity, CancellationToken token = default);
        Task UpdateAsync(T entity, CancellationToken token = default);
        Task DeleteAsync(int id, CancellationToken token = default);
    }
}
