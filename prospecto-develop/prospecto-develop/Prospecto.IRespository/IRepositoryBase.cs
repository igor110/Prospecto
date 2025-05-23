using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Prospecto.Respository.Interface
{
    public interface IRepositoryBase<T> : IDisposable
    {
        Task<List<T>> ListAsync();

        Task<T> Get(int id);

        IQueryable<T> GetQuery(int id);

        IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate);

        Task<int> Insert(T info);

        Task Update(T info);

        void Delete(T info);

        Task<int> Count(Expression<Func<T, bool>> predicate);

        IDbContextTransaction BeginTransaction();
    }
}
