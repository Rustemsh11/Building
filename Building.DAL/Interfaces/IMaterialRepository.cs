using Building.Domain.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.DAL.Interfaces
{
   public interface IMaterialRepository:IBaseRepository<Material>
   {
        IEnumerable<string> GetMaterialName();
        IEnumerable<string> GetMaterial(string term);
        IEnumerable<Catalog> GetCatalogs();
        IEnumerable<Material> GetMaterials(int id);

        Task<Material> GetIdByName(string name);
        //IEnumerable<MaterialCharacteristic> GetMaterialCharacteristic(string materialName); возможно удалить


    }
}
