using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperLayer.File.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HelperLayer.File.Service
{
    public class FileService : IFile
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment webHost)
        {
            _environment = webHost;
        }

        public string ProcessFileUser(IFormFile formFile, string folderName)
        {
            string imgFilePath;

            // إذا لم يتم رفع أي ملف، استخدم الصورة الافتراضية
            if (formFile == null)
            {
                imgFilePath = $"\\File\\Default\\Userdefault.png";
                return imgFilePath;
            }

            Guid id = Guid.NewGuid();
            string ext = Path.GetExtension(formFile.FileName);
            string fullname = id + ext;

            var imgfile = $"\\File\\User\\{folderName}\\" + fullname;

            string fullpath = _environment.WebRootPath + imgfile;


            FileStream fileStream = new FileStream(fullpath, FileMode.Create);
            formFile.CopyTo(fileStream);
            fileStream.Dispose();
            return fullpath;
        }

        public string? ProcessFileAntherFile(IFormFile formFile, string folderName)
        {
            if (formFile == null || formFile.Length == 0)
                return null;  // تأكيد أن الملف غير فارغ

            Guid id = Guid.NewGuid();
            string ext = Path.GetExtension(formFile.FileName);
            string fullname = id + ext;

            var imgfile = $"\\File\\{folderName}\\" + fullname;

            string fullpath = _environment.WebRootPath + imgfile;


            FileStream fileStream = new FileStream(fullpath, FileMode.Create);
            formFile.CopyTo(fileStream);
            fileStream.Dispose();
            return fullpath;
        }

        /*public string ProcessFileAntherFile(IFormFile formFile, string folderName, Course course)
        {
            if (formFile == null)
            {
                return course.CourseFile;
            }
            Guid id = Guid.NewGuid();
            string ext = Path.GetExtension(formFile.FileName);
            string fullname = id + ext;

            var imgfile = $"\\files\\{folderName}\\" + fullname;

            string fullpath = _environment.WebRootPath + imgfile;


            FileStream fileStream = new FileStream(fullpath, FileMode.Create);
            formFile.CopyTo(fileStream);
            fileStream.Dispose();
            return fullpath;
        }*/

        public async Task<string> GetImgAsync(string file, string fulder)
        {
            if (string.IsNullOrWhiteSpace(file))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(fulder))
                throw new ArgumentException("Folder name is required.", nameof(fulder));

            var imagePath = Path.Combine($"wwwroot/file/{fulder}", file);

            if (System.IO.File.Exists(imagePath))
            {
                var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                var base64Image = Convert.ToBase64String(imageBytes);
                return $"data:image/png;base64,{base64Image}";
            }

            return string.Empty;
        }
    }
}
