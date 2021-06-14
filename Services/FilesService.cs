using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using School.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace School
{
    public interface IFilesService
    {
        IFormFile FormFile { get; set; }

        int Create(FileData file);
        IEnumerable<FileData> GetAll();
        FileData GetById(int id);
    }

    public class FilesService : IFilesService
    {
        private readonly ApiContext _context;
        public IFormFile FormFile { get; set; }

        public FilesService(ApiContext apiContext)
        {
            _context = apiContext;
        }
        public FileData GetById(int id)
        {
            var file = _context.Files.FirstOrDefault(r => r.ID == id);

            if (file is null) return null;
            return file;
        }

        public IEnumerable<FileData> GetAll() 
        {
            var files = _context.Files.ToListAsync();
            return (IEnumerable<FileData>)files;
        }

        public int Create(FileData file)
        {
            using (var target = new MemoryStream())
            {
                FormFile.CopyTo(target);
                file.FileUpload = target.ToArray();
            }
            file.CreatedOn = DateTime.Now;
            file.UploadedBy = "user";

            var filename = FormFile.FileName;
            //var extension = fileName.Split(".")[1];

            file.fileUploadName = filename;
            _context.Files.Add(file);
            _context.SaveChangesAsync();

            return file.ID;
        }
    }
}
