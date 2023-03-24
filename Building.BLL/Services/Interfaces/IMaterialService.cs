using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Building.Domain.Response;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.BLL.Services.Interfaces
{
    public interface IMaterialService
    {
        BaseResponse<IEnumerable<string>> GetMaterialName();
        BaseResponse<IEnumerable<string>> GetMaterial(string term);
        BaseResponse<IEnumerable<Material>> GetMaterials(int id);
        BaseResponse<IEnumerable<Catalog>> GetCatalog();

        Task<BaseResponse<Material>> GetIdByName(string name);
        //BaseResponse<IEnumerable<MaterialCharacteristic>> GetCharactristics(string materialName);

    }
    
}
