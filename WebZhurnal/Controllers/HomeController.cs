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
            var usr=await userManager.GetUserAsync(HttpContext.User); 
            model.CurrentUserId= usr.Id;
            model.CurrentUserName = usr.UserName;
            model.Students = dbContext.Users.Include(u=>u.Claims).Include(u=>u.Group).Where(u => u.Claims.Any(c => c.ClaimType == "Type" && c.ClaimValue == "Student")).ToList();

            if (User.Claims.Any(c => (c.Type == "Type") && (c.Value == "Group"))) model.Students = model.Students.Where(u => u.Group!=null&& u.Group.Name == User.Identity.Name).ToList();
            //if (User.Claims.Any(c => (c.Type == "Type") && (c.Value == "Teacher"))) model.Students = model.Students.Where(u => u.Group != null && usr.TeacherGroups.Any(tg=>tg.GroupId==u.GroupId)).ToList();

            model.Subjects = dbContext.Subjects.ToList();
            model.Rates = dbContext.Rates.ToList();
            model.Groups =dbContext.Groups.Include(u=>u.TeacherGroups).ToList();
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
