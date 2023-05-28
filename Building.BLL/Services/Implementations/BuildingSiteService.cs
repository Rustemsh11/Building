using Building.BLL.Services.Interfaces;
using Building.DAL.Interfaces;
using Building.Domain.Entity;
using Building.Domain.Enum;
using Building.Domain.Response;
using Building.Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.BLL.Services.Implementations
{
    public class BuildingSiteService : IBuildingSiteService
    {
        private readonly IBuildingSiteRepository buildingSiteRepository;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeBuildingRepository employeeBuildingRepository;
        public BuildingSiteService(IBuildingSiteRepository buildingSiteRepository, IEmployeeService employeeService, IEmployeeBuildingRepository buildingRepository)
        {
            this.buildingSiteRepository = buildingSiteRepository;
            this.employeeService = employeeService;
            this.employeeBuildingRepository = buildingRepository;
        }


        private async Task<bool> CreateSiteResponsibility(BuildingSiteViewModel siteViewModel)
        {
            var prorabName = siteViewModel.ProrabName.Split(" ");
            var prorab = await employeeService.GetAllProrab();

            var emplBuilding = new EmployeesBuilding();
            emplBuilding.BuildingId = buildingSiteRepository.GetIdByName(siteViewModel.Name);
            emplBuilding.EmployeeId = prorab.Data.FirstOrDefault(c => c.SecondName == prorabName[0]).EmployeeId;


            return employeeBuildingRepository.Create(emplBuilding);
        }

        public async Task<BaseResponse<bool>> AddNewSite(BuildingSiteViewModel siteViewModel, List<IFormFile> postedFiles)
        {
            try
            {
                if (siteViewModel != null)
                {
                    var site = new BuildingSite();
                    site.Name = siteViewModel.Name;
                    site.Address = siteViewModel.Address;
                    site.Deadline = siteViewModel.Deadline;

                    if (postedFiles.Count > 0)
                    {
                        foreach (var item in postedFiles)
                        {
                            byte[] image = null;
                            using (var fs1 = item.OpenReadStream())
                            using (var ms1 = new MemoryStream())
                            {
                                fs1.CopyTo(ms1);
                                image = ms1.ToArray();
                            }
                            site.Photo = image;

                        }
                    }

                    bool result = buildingSiteRepository.Create(site);
                    bool isCreated = await CreateSiteResponsibility(siteViewModel);
                    if (result && isCreated)
                    {
                        var response = new BaseResponse<bool>()
                        {
                            Data = result,
                            StatusCode = Domain.Enum.StatusCode.OK
                        };
                        return response;
                    }

                    

                    else
                    {
                        var response = new BaseResponse<bool>()
                        {
                            Description = "Building sites not created"
                        };
                        return response;
                    }
                }
                else
                {
                    var response = new BaseResponse<bool>()
                    {
                        Description = "Argument is null"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<bool> ()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<BuildingSite>>> GetAll()
        {
            try
            {
                var buildingSite = await buildingSiteRepository.GetAll();
                if (buildingSite != null)
                {
                    var response = new BaseResponse<IEnumerable<BuildingSite>> ()
                    {
                        Data = buildingSite,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<IEnumerable<BuildingSite>>()
                    {
                        Description = "Building sites not found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<BuildingSite>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse<BuildingSite>> GetBuildingInfo(int? id)
        {
            try
            {
                var buildingSite = await buildingSiteRepository.Get(id);
                if (buildingSite != null)
                {
                    var response = new BaseResponse<BuildingSite>()
                    {
                        Data = buildingSite,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<BuildingSite>()
                    {
                        Description = "Building site not found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<BuildingSite>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public BaseResponse<int> GetIdByName(string siteName)
        {
            try
            {
                var buildingSite =  buildingSiteRepository.GetIdByName(siteName);
                if (buildingSite != null)
                {
                    var response = new BaseResponse<int>()
                    {
                        Data = buildingSite,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                    return response;
                }
                else
                {
                    var response = new BaseResponse<int>()
                    {
                        Description = "Building sites not found"
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<int>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.ServerError
                };
            }
        }
    }
}
