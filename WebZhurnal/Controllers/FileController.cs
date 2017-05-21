using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using WebZhurnal.Data;
using Microsoft.AspNetCore.Hosting;
using WebZhurnal.Models;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebZhurnal.Controllers
{
    public class FileController : Controller
    {

        protected ApplicationDbContext _context;
        protected IHostingEnvironment _appEnvironment;

        public FileController(ApplicationDbContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        
        public async Task<FileModel> Upload(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (!Directory.Exists(Path.Combine(_appEnvironment.WebRootPath, "Uploads")))
                    Directory.CreateDirectory(Path.Combine(_appEnvironment.WebRootPath, "Uploads"));
                string path = Path.Combine("Uploads", uploadedFile.FileName);
                using (var fileStream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path,  DateTimeUploaded=DateTime.Now };
                _context.Files.Add(file);
                _context.SaveChanges();
                return file;
            }
            return null;
            
        }


    }

    public class ExcelController : FileController
    {
        private UserManager<ApplicationUser> _userManager;

        public ExcelController(ApplicationDbContext context, IHostingEnvironment appEnvironment, UserManager<ApplicationUser> userManager) : base(context, appEnvironment)
        {
            _userManager = userManager;
        }

        public ActionResult Index(int mode=0)
        {
            ViewBag.Mode = mode;
            return View(_context.Files.Where(f => f.Name.EndsWith("xlsx")).ToList());
        }

        public async Task<ActionResult> ImportUsers(IFormFile excelFile)
        {
            FileModel file = await Upload(excelFile);
            if (file==null) return View("Error");
            ExcelManager manager = new ExcelManager(Path.Combine(_appEnvironment.WebRootPath, file.Path));
            ViewBag.FileId = file.Id;
            ViewBag.ExcelRows = manager.GetRows(10);
            return View(manager.GetRow(1));
        }

        public async Task<ActionResult> DoImportUsers(int fileId, string[] columns, int startRow=1, string defaultType="")
        {
            ExcelManager manager = new ExcelManager(
                Path.Combine(_appEnvironment.WebRootPath, 
                _context.Files.Single(f=>f.Id==fileId).Path));
            foreach(var row in manager.GetRows().Skip(startRow-1))
            {
                try
                {
                    ApplicationUser user = new ApplicationUser();
                    int j = columns.ToList().IndexOf("LoginColumn");
                    if (j < 0) throw new ArgumentException("Не найдено поле с логином");
                    user.UserName = row[j];
                    j = columns.ToList().IndexOf("PhoneNumberColumn");
                    if (j > 0) user.PhoneNumber = row[j];
                    await _userManager.CreateAsync(user, "`1qw23E");
                    for (int i = 0; i < row.Count; i++)
                    {
                        if (String.IsNullOrEmpty(columns[i]))
                        {
                            continue;
                        }
                        string propName = columns[i].Replace("Column", "");
                        if (propName == "Group")
                        {
                            var group = _context.Groups.FirstOrDefault(g => g.Name == row[i]);
                            if (group == null)
                            {
                                group = _context.Groups.Add(new Group() { Name = row[i] }).Entity;
                                _context.SaveChanges();
                            }
                            user.GroupId = group.Id;
                        }
                        else if (propName == "Type")
                        {
                            await _userManager.AddClaimAsync(user, new IdentityUserClaim<string> { ClaimType = propName, ClaimValue = String.IsNullOrEmpty(defaultType) ? row[i] : defaultType }.ToClaim());
                        }
                        else
                        {
                            await _userManager.AddClaimAsync(user, new IdentityUserClaim<string> { ClaimType = propName, ClaimValue = row[i] }.ToClaim());
                        }
                    }
                    await _userManager.UpdateAsync(user);
                }
                catch
                {
                    continue;
                }
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Import(int fileId, int mode)
        {
            return RedirectToAction("Index", "Admin");
        }

    }
}