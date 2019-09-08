using COF.API.Controllers.Core;
using COF.BusinessLogic.Models.Common;
using COF.BusinessLogic.Services;
using COF.BusinessLogic.Services.AzureBlob;
using COF.Common.Helper;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COF.API.Controllers
{
    [Authorize]
    public class CommonController : MvcControllerBase
    {
        #region fields
        private readonly ISizeService _commonService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IAzureBlobSavingService _azureBlobSavingService;


        #endregion
        #region ctor
        public CommonController(
            ISizeService commonService,
            IRoleService roleService,
            IAzureBlobSavingService azureBlobSavingService)
        {
            _commonService = commonService;
            _roleService = roleService;
            _azureBlobSavingService = azureBlobSavingService;
        }
        #endregion

        [HttpGet]
        public async Task<JsonResult> GetAllSizes()
        {
            try
            {
                var res = await _commonService.GetAllSizesAsync();
                return HttpGetSuccessResponse(res);
            }
            catch (Exception ex)
            {
                return HttpGetErrorResponse(ex.Message);
            }
           
        }

        [HttpGet]
        public async Task<JsonResult> GetAllRoles()
        {
            try
            {
                var allRoles = await _roleService.GetAllRoles();
                var result = allRoles.Select(x => new
                {
                    RoleId = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();
                return HttpGetSuccessResponse(result);
            }
            catch (Exception ex)
            {
                return HttpGetErrorResponse(ex.Message);
            }

        }

        [HttpPost]
        public async Task<JsonResult> UploadImage(UploadFileType type)
        {
           
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    var file = files[0];
                    
                    byte[] data = null;
                    var fileName = file.FileName;
                    using (var reader = new BinaryReader(file.InputStream))
                    {
                        data = reader.ReadBytes(file.ContentLength);
                    }
                    var contenType = MimeMapping.GetMimeMapping(fileName);
                    var uploadFileName = "";
                    CloudBlobContainer blobContainer = null;
                    var folder = "";

                    switch (type)
                    {
                        case UploadFileType.Product:
                            blobContainer = AzureHelper.ProductImageContainer;
                            folder = ConfigurationManager.AppSettings["Product"];

                            break;
                        case UploadFileType.Category:
                            blobContainer = AzureHelper.CategoryImageContainer;
                            folder = ConfigurationManager.AppSettings["Category"];

                            break;
                        default:
                            return HttpPostErrorResponse("Upload hình ảnh không thành công.");
                    }
                    uploadFileName = await _azureBlobSavingService.SavingFileToAzureBlobAsync(data, fileName, contenType, blobContainer);
                    var result = new UploadImageResult
                    {
                        Url = $"{ConfigurationManager.AppSettings["ServerImage"]}/{folder}/{uploadFileName}",
                    };
                    return HttpPostSuccessResponse(result);
                }
                catch (Exception ex)
                {
                    return HttpPostErrorResponse(ex.Message);
                }
            }
            else
            {
                return HttpPostErrorResponse("Choose file as requried");
            }
        }

    }
}