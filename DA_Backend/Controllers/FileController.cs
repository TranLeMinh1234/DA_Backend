using BL.Interface;
using ClassModel;
using ClassModel.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using static ClassModel.Enumeration;
using File = System.IO.File;

namespace DA_Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : BaseController<FileAttachment>
    {
        private IBLFileAttachment _iBLFileAttachment;
        private IConfiguration _configuration;
        private ContextRequest _contextRequest;
        public FileController(IBLFileAttachment bLFileAttachment, IConfiguration configuration, ContextRequest contextRequest) : base(bLFileAttachment, configuration)
        {
            _iBLFileAttachment = bLFileAttachment;
            _configuration = configuration;
            _contextRequest = contextRequest;
        }

        private string fileExtensionAllow = ".jpg,.xlsx,.png,.jfif";
        private string defautPathFileSave = "\\FileUpload";

        [HttpPost("upload")]
        public ServiceResult UploadFile([FromForm] IFormFileCollection formFiles, [FromForm] EnumAttachment typeAttachment = EnumAttachment.AttachAvatar, [FromForm] Guid? attachmentId = null)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                if (formFiles != null && formFiles.Count > 0)
                {
                    foreach (var formFile in formFiles)
                    {
                        string extensionFile = Path.GetExtension(formFile.FileName);
                        string pathFile = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + "\\FileUpload";
                        string pathFileExtension = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + $"\\FileUpload\\{extensionFile}";
                        bool isExistsFolder = Directory.Exists(pathFile);
                        bool isExistsFolderExtension = Directory.Exists(pathFileExtension);

                        if (!isExistsFolder)
                        {
                            Directory.CreateDirectory(pathFile);
                        }

                        if (!isExistsFolderExtension)
                        {
                            Directory.CreateDirectory(pathFileExtension);
                        }

                        if (fileExtensionAllow.Contains(extensionFile))
                        {
                            Guid newID = Guid.NewGuid();
                            string saveFileName = pathFileExtension + $"\\{newID}_{formFile.FileName}";
                            FileAttachment attachment = new FileAttachment() {
                                FileId = newID,
                                ExtensionOfFile = extensionFile,
                                FileName = $"{newID.ToString()}_{formFile.FileName}",
                                FilePath = saveFileName,
                                AttachmentId = attachmentId,
                                TypeAttachment = (int)typeAttachment,
                                CreatedTime = DateTime.Now,
                                CreatedByEmail = _contextRequest.GetEmailCurrentUser()
                            };
                            _iBLFileAttachment.Insert(attachment);

                            using (FileStream fileStream = System.IO.File.Create(saveFileName))
                            {
                                formFile.CopyTo(fileStream);
                                fileStream.Flush();
                            }

                            serviceResult.Data = attachment;
                        }
                        else
                        {
                            serviceResult.Success = false;
                            serviceResult.ErrorCode.Add("InvalidExtension");
                        }
                    }
                }
                else
                {
                    serviceResult.Success = false;
                    serviceResult.ErrorCode.Add("EmptyDataUpload");
                }
            }
            catch (Exception ex) {
                System.Console.WriteLine(ex);
            }
            return serviceResult;
        }

        [AllowAnonymous]
        [HttpGet("download/{fileName}")]
        public FileContentResult DownloadFile(string fileName)
        {
            string filePath =
                Path.GetDirectoryName(Directory.GetCurrentDirectory()) +
                defautPathFileSave +
                $"\\{Path.GetExtension(fileName)}\\{fileName}";
            byte[] byteOfFile = System.IO.File.ReadAllBytes(filePath);
            return File(byteOfFile, "application/octet-stream");

        }

        [AllowAnonymous]
        [HttpGet("img/{fileName}")]
        public FileContentResult DownloadFileImage(string fileName)
        {
            string filePath =
                Path.GetDirectoryName(Directory.GetCurrentDirectory()) +
                defautPathFileSave +
                $"\\{Path.GetExtension(fileName)}\\{fileName}";
            byte[] byteOfFile = System.IO.File.ReadAllBytes(filePath);
            return File(byteOfFile, "image/jpeg");

        }

        [HttpGet("getattachfile/{taskId}")]
        public IActionResult GetAttachFile(Guid taskId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLFileAttachment.GetAttachFile(taskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
