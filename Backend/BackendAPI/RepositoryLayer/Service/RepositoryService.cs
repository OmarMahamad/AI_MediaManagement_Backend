using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class RepositoryService<T> : IRepository<T> where T : class
    {
        private readonly DbApp _dbApp;
        private readonly DbSet<T> _dbSet;
        public RepositoryService(DbApp dbApp)
        {
            _dbApp=dbApp;
            _dbSet=_dbApp.Set<T>();
        }

        public async Task AddItemAsync(T item)
        {
            await _dbSet.AddAsync(item);
            await _dbApp.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            var item= await GetItemAsync(id);
            if (item == null)
                return ;
             _dbSet.Remove(item);
            await _dbApp.SaveChangesAsync();
        }

        public async Task EditItemAsync(int id, T item)
        {
            var itemexit = await GetItemAsync(id);
            if (itemexit == null)
                return;

            _dbApp.Entry(itemexit).CurrentValues.SetValues(item);
            await _dbApp.SaveChangesAsync();
        }


        public Task<T> FilterByWhereAsync(Expression<Func<T, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> FirstOrderAsync(Expression<Func<T, bool>>? predicate)
        {
            var query = _dbSet.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<T> GetItemAsync(int id)=> await _dbSet.FindAsync(id);





    }
}
