using Building.BLL.Services.Interfaces;
using Building.DAL.Interfaces;
using Building.DAL.Repositories;
using Building.Domain.Entity;
using Building.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.BLL.Services.Implementations
{
    public class ImageServise : IImageService
    {
        private readonly IImageRepository imageRepository;
        public ImageServise(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        public BaseResponse<bool> Create(Image image)
        {
            try
            {

                var result = imageRepository.Create(image);
                if (result != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = result,
                        StatusCode = Domain.Enum.StatusCode.OK
                    };
                }
                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = "Failed to create image"
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = Domain.Enum.StatusCode.ServerError,
                    Description = ex.Message
                };
            }
        }
    }
}
