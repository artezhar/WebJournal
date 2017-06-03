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
            var teacher = await userManager.GetUserAsync(HttpContext.User);
            var student = dbContext.Users.Include(u => u.Claims).Single(u => u.Id == studentId);
            var subject = dbContext.Subjects.Single(s => s.Id == subjectId);
            if (rate == null)
            {
                rate = new StudentRate() { StudentId = studentId, SubjectId = subjectId, Value = value, Date=DateTime.Now };
                rate.TeacherId = teacher.Id;
                dbContext.Add(rate);
            }
            else { rate.Value = value; rate.TeacherId = teacher.Id; }
            var rateItem = new RateItem() { Type = LogItemType.Rate, DateTime = DateTime.Now, Teacher = teacher.Name, Student = student.Name, Rate = value, Subject=subject.Name };
            dbContext.LogItems.Add(rateItem);
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

        public async Task<IActionResult> MyStudents()
        {
            var teacher = await userManager.GetUserAsync(HttpContext.User);
            var students = dbContext.Users.Include(u => u.Claims).ToList()
                .Where(u => u.Type == "Student" && teacher.TeacherGroups.Select(tg=>tg.GroupId).Contains(u.GroupId.GetValueOrDefault()))
                .ToList();
            ViewBag.Students = students;
            
            ViewBag.Rates = dbContext.Rates.Where(r => r.TeacherId==teacher.Id&& students.Select(s => s.Id).Contains(r.StudentId)).ToList();
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
