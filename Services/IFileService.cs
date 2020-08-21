using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace documentManagerService.Services
{
    public interface IFileService
    {
        void SaveFile(List<IFormFile> files, string subDirectory);
        
    }
}