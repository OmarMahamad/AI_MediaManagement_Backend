using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HelperLayer.File.Interface
{
    public interface IFile
    {
        string ProcessFileUser(IFormFile formFile, string folderName);
        string? ProcessFileAntherFile(IFormFile formFile, string folderName);
        /*string ProcessFileAntherFile(IFormFile formFile, string folderName, Course course);*/
        Task<string> GetImgAsync(string file, string fulder);
    }
}
