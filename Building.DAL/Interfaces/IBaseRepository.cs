using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// base methods for repo
        /// </summary>
        /// <returns></returns>
        Task<T> Get(int id);               
        bool Create(T entity);
        Task<bool> Update(T entity);
        bool Delete(int id);

    }
}
