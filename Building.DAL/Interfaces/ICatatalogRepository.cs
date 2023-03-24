using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.DAL.Interfaces
{
    public interface ICatatalogRepository
    {
        Task<IQueryable<int>> GetIdByName(string name);
    }
}
