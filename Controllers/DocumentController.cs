using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using documentManagerService.Model;
using documentManagerService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace documentManagerService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentController : ControllerBase
    {
        
        private readonly IFileService _fileService;


        private readonly ILogger<DocumentController> _logger;

        public DocumentController(ILogger<DocumentController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }
        
        
        [HttpGet]
        [Route("files")]
        public IEnumerable<FileItem> GetFiles()
        {
            var rng = new Random();
            return Enumerable.Range(1, 100).Select(index => new FileItem()
                {
                    Id = rng.Next(1, 1000),
                    Name = $"xpot{rng.Next(1, 1000)}.txt"
                })
                .ToArray();
        }
        
        [HttpGet]
        [Route("files/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(FileItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<FileItem>> ItemByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = new FileItem()
            {
                Id = id,
                Name = $"Saving{id}.txt"
            };

            if (item != null)
            {
                return item;
            }

            return NotFound();
        }
        
        //POST api/v1/[controller]/items
        
        [Route("files")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<FileItem>> CreateProductAsync([FromBody]FileItem fileItem)
        {
            var item = new FileItem
            {
                Id = fileItem.Id,
                Name = fileItem.Name
            };
            
            _logger.Log(LogLevel.Information, item.ToString());


            return item;
        }
        
        [Route("upload")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<string>> UploadFileAsync([FromBody]FileItem fileItem)
        {
            if (fileItem.FileBase64 == null || fileItem.Name == null)
            {
                return BadRequest();
            }

            byte[] imageByteArray = Convert.FromBase64String(fileItem.FileBase64);
            Image image = Image.Load<Rgba32>(imageByteArray);
            
            //var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            var uploadFolder = Path.Combine("/files/", "UploadedFiles");
            
            var finalPath = Path.Combine(uploadFolder, fileItem.Name);
            
            try
            {
                await image.SaveAsync(finalPath);
                return Ok($"File is uploaded Successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        
        [Route("uploadfile")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public IActionResult UploadFile([FromForm(Name = "files")] List<IFormFile> files)
        {
            try
            {
                //_fileService.SaveFile(files,  Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles"));
                _fileService.SaveFile(files,  Path.Combine("/files/", "UploadedFiles"));

                return Ok(new { files.Count, Size = FileService.SizeConverter(files.Sum(f => f.Length)) });
            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
            }
        }
        
    }
    
    
}