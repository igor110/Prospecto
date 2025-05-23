using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Prospecto.Models.Infos.Base;
using Prospecto.Respository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Prospecto.Repository
{
    public abstract class AbstractRepositoryBase<T> : IRepositoryBase<T> where T : BaseInfo
    {
        internal readonly DbSet<T> _dbSet;
        internal readonly DbContext _context;

        public AbstractRepositoryBase(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual void Delete(T info)
        {
            _context.Entry(info).State = EntityState.Deleted;
            _dbSet.Remove(info);
            _context.SaveChanges();
        }

        public virtual async Task<T> Get(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public virtual async Task Update(T info)
        {
            _context.Entry(info).State = EntityState.Modified;
            _dbSet.Update(info);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<int> Insert(T info)
        {
            await _dbSet.AddAsync(info);
            await _context.SaveChangesAsync();
            Type type = typeof(T);
            PropertyInfo prop = type.GetProperty("Id");
            return Convert.ToInt32(prop.GetValue(info));
        }

        public virtual async Task<List<T>> ListAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual IQueryable<T> GetQuery(int id)
        {
            return _dbSet.Where(x => x.Id == id);
        }

        public IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Always use this method, with using (var trans = StartTransaction())
        /// </summary>
        /// <returns>context.Transaction</returns>
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Dispose() => _context?.Dispose();
    }
}
