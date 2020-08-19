using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using documentManagerService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace documentManagerService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<DocumentController> _logger;

        public DocumentController(ILogger<DocumentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
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
        
    }
    
    
}