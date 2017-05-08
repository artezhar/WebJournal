using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebZhurnal.Data;
using WebZhurnal.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebZhurnal.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext dbContext;
        private UserManager<ApplicationUser> userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = context;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Students");
            else
                return RedirectToAction("Login", "Account");
        }

        public IActionResult About()
        {
            return View(dbContext.Subjects.ToList());
        }


        [Authorize]

        public async Task<IActionResult> SetRate(string studentId, int subjectId, int value)
        {
            var rate = dbContext.Rates.FirstOrDefault(r => r.StudentId == studentId && r.SubjectId == subjectId);
            var id = (await userManager.GetUserAsync(HttpContext.User)).Id;
            if (rate == null)
            {
                rate = new StudentRate() { StudentId = studentId, SubjectId = subjectId, Value = value };
                rate.TeacherId = id;
                dbContext.Add(rate);
            }
            else { rate.Value = value; rate.TeacherId = id; }
            dbContext.SaveChanges();
            return Ok();           
        }

        [Authorize]

        public async Task<IActionResult> Students()
        {
            var model = new StudentRateModel();
            model.CurrentUserId= (await userManager.GetUserAsync(HttpContext.User)).Id;
            model.Students = dbContext.Users.Include(u=>u.Claims).Where(u => u.Claims.Any(c => c.ClaimType == "Type" && c.ClaimValue == "Student")).ToList();
            
            model.Subjects = dbContext.Subjects.ToList();
            model.Rates = dbContext.Rates.ToList();
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
