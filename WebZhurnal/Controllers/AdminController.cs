using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebZhurnal.Data;
using Microsoft.AspNetCore.Identity;
using WebZhurnal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebZhurnal.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private ApplicationDbContext dbContext;
        private UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = context;
            this._userManager = userManager;
        }

        public IActionResult Students()
        {
            return View();
        }

        public async Task<IActionResult> CreateAdmin(string password = "`1qw23E")
        {
            var user = new ApplicationUser { UserName = "admin", Email = "admin@yest" };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                try
                {
                    var identityClaim = new IdentityUserClaim<string> { ClaimType = "Type", ClaimValue = "Admin" };
                    user.Claims.Add(identityClaim);
                    var nameClaim = new IdentityUserClaim<string> { ClaimType = "Name", ClaimValue = "Администратор" };
                    user.Claims.Add(nameClaim);
                    await _userManager.UpdateAsync(user);
                }
                catch(Exception e)
                {
                    return Content(e.Message);
                }
                return Ok();
            }
            return Content(result.ToString());
        }
        public async Task<IActionResult> AddStudent(string login, string name, string email, string password="`1qw23E")
        {
            var user = new ApplicationUser { UserName = login, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                try
                {
                    var identityClaim = new IdentityUserClaim<string> { ClaimType = "Type", ClaimValue = "Student" };
                    user.Claims.Add(identityClaim);
                    var nameClaim = new IdentityUserClaim<string> { ClaimType = "Name", ClaimValue = name };
                    user.Claims.Add(nameClaim);
                    await _userManager.UpdateAsync(user);
                }
                catch
                {
                    return BadRequest();
                }
                return Ok();
            }
            return BadRequest();
        }
        public async Task<IActionResult> DeleteStudent(string login)
        {
            try
            {
                await _userManager.DeleteAsync(dbContext.Users.Single(u => u.UserName == login));
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        public IActionResult Teachers()
        {
            return View();
        }
        public IActionResult Subjects()
        {
            return View();
        }
    }
}