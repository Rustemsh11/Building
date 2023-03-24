using Building.BLL.Services.Interfaces;
using Building.DAL.Interfaces;
using Building.DAL.Repositories;
using Building.Domain.Entity;
using Building.Domain.Enum;
using Building.Domain.Response;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.BLL.Services.Implementations
{
    public class MaterialService:IMaterialService 
    {
        private readonly IMaterialRepository materialRepository;
        public MaterialService(IMaterialRepository materialRepository)
        {
            this.materialRepository = materialRepository;

        }

        public BaseResponse<IEnumerable<Catalog>> GetCatalog()
        {
            try
            {
                var catalogName = materialRepository.GetCatalogs();
                if (catalogName != null)
                {

                    var baseResponse = new BaseResponse<IEnumerable<Catalog>>()
                    {
                        Data = catalogName,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return baseResponse;
                }

                return new BaseResponse<IEnumerable<Catalog>>()
                {
                    Description = "Not found name",
                    StatusCode = Domain.Enum.StatusCode.ServerError
                };



            }
            catch (Exception ex)
            {

                return new BaseResponse<IEnumerable<Catalog>>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.ServerError
                };
            }
        }

        public BaseResponse<IEnumerable<string>> GetMaterial(string term)
        {
            try
            {
                var materialName = materialRepository.GetMaterial(term);
                if (materialName != null)
                {

                    var baseResponse = new BaseResponse<IEnumerable<string>>()
                    {
                        Data = materialName,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return baseResponse;
                }

                return new BaseResponse<IEnumerable<string>>()
                {
                    Description = "Not found name",
                    StatusCode = Domain.Enum.StatusCode.ServerError
                };



            }
            catch (Exception ex)
            {

                return new BaseResponse<IEnumerable<string>>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.ServerError
                };
            }
        }

        public BaseResponse<IEnumerable<Material>> GetMaterials(int id)
        {
            try
            {
                var materials = materialRepository.GetMaterials(id);
                if (materials != null)
                {

                    var baseResponse = new BaseResponse<IEnumerable<Material>>()
                    {
                        Data = materials,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return baseResponse;
                }

                return new BaseResponse<IEnumerable<Material>>()
                {
                    Description = "Not found name",
                    StatusCode = Domain.Enum.StatusCode.ServerError
                };



            }
            catch (Exception ex)
            {

                return new BaseResponse<IEnumerable<Material>>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.ServerError
                };
            }
        }

        //public BaseResponse<IEnumerable<MaterialCharacteristic>> GetCharactristics(string materialName)
        //{
        //    try
        //    {
        //        var materialChara = materialRepository.GetMaterialCharacteristic(materialName).ToList();

        //        if (materialChara != null)
        //        {
        //            return new BaseResponse<IEnumerable<MaterialCharacteristic>>()
        //            {
        //                Data = materialChara,
        //                StatusCode = Domain.Enum.StatusCode.OK
        //            };
        //        }
        //        return new BaseResponse<IEnumerable<MaterialCharacteristic>>()
        //        {
        //            Description = "Not found characteristic",
        //            StatusCode = Domain.Enum.StatusCode.ServerError
        //        };
        //    }
        //    catch (Exception ex)
        //    {

        //        return new BaseResponse<IEnumerable<MaterialCharacteristic>>()
        //        {
        //            Description = $"{ex.Message}",
        //            StatusCode = Domain.Enum.StatusCode.ServerError
        //        };
        //    }
        //}

        public BaseResponse<IEnumerable<string>> GetMaterialName()
        {
            try
            {
                var materialName = materialRepository.GetMaterialName();
                if (materialName != null)
                {

                    var baseResponse = new BaseResponse<IEnumerable<string>>()
                    {
                        Data = materialName,
                        StatusCode=Domain.Enum.StatusCode.OK
                    };
                    return baseResponse;
                }

                return new BaseResponse<IEnumerable<string>>()
                {
                    Description = "Not found name",
                    StatusCode = Domain.Enum.StatusCode.ServerError
                };
              


            }            
            catch (Exception ex)
            {

                return new BaseResponse<IEnumerable<string>>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = Domain.Enum.StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse<Material>> GetIdByName(string name)
        {
            try
            {
                var id = await materialRepository.GetIdByName(name);
                if (id != null)
                {
                    var response = new BaseResponse<Material>()
                    {
                        Data = id,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<Material>()
                    {
                        Description = "Employee not found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<Material>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }
    }
}
