using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetItemAsync(int id);
        Task AddItemAsync(T item);
        Task DeleteItemAsync(int id);
        Task EditItemAsync(int id,T item);

        Task<T?> FirstOrderAsync(Expression<Func<T, bool>>? predicate);
        Task<T> FilterByWhereAsync(Expression<Func<T,bool>>? predicate=null);


    }
}
