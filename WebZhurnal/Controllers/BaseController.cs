using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using WebZhurnal.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using WebZhurnal.Models;

namespace WebZhurnal.Controllers
{
    public class BaseController : Controller
    {

        IHostingEnvironment _appEnvironment;
        private ApplicationDbContext dbContext;
        public BaseController(ApplicationDbContext context, IHostingEnvironment appEnvironment)
        {
            dbContext = context;
            _appEnvironment = appEnvironment;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Upload(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/Uploads/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                dbContext.Files.Add(file);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}