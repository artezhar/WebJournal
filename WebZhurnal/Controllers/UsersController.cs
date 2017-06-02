using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebZhurnal.Data;
using WebZhurnal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebZhurnal.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Include(u=>u.Claims).Include(u=>u.Group). ToListAsync());
        }

       

        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
            ViewData["Subjects"] = new SelectList(_context.Subjects, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,PhoneNumber")] ApplicationUser applicationUser, string type, string name, string Subject, string password="`1qw23E", int GroupId=0)
        {
            if (ModelState.IsValid)
            {
               
                try
                {
                    var identityClaim = new IdentityUserClaim<string> { ClaimType = "Type", ClaimValue = type };
                    applicationUser.Claims.Add(identityClaim);
                    var nameClaim = new IdentityUserClaim<string> { ClaimType = "Name", ClaimValue = name };
                    applicationUser.Claims.Add(nameClaim);

                    if (!String.IsNullOrWhiteSpace(Subject)&&type=="Teacher")
                    {
                        if (!_context.Subjects.Any(s => s.Name == Subject))
                        {
                            var newSubject = _context.Subjects.Add(new Subject() { Name = Subject }).Entity;
                            _context.SaveChanges();
                            var subjectClaim = new IdentityUserClaim<string>
                            {
                                ClaimType = "Subject",
                                ClaimValue = (newSubject.Id.ToString())
                            };
                            applicationUser.Claims.Add(subjectClaim);
                        }

                    }

                    await _userManager.CreateAsync(applicationUser, "`1qw23E");
                }
                catch
                {
                    return View("Error");
                }

                return RedirectToAction("Index");
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name",applicationUser.GroupId);
            return View(applicationUser);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users.Include(u=>u.Claims).Include(u => u.Group).SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", applicationUser.GroupId);
            return View(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,PhoneNumber")]ApplicationUser applicationUser, string type, string name, string Subject, int GroupId = 0)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var typeClaim = new IdentityUserClaim<string> { ClaimType = "Type", ClaimValue = type.ToString() };
                    if (applicationUser.Claims.Where(cl => cl.ClaimType == "Type").Count() > 0) applicationUser.Claims.Remove(applicationUser.Claims.First(cl => cl.ClaimType == "Type"));
                    applicationUser.Claims.Add(typeClaim);
                    var nameClaim = new IdentityUserClaim<string> { ClaimType = "Name", ClaimValue = name };
                    if (applicationUser.Claims.Where(cl => cl.ClaimType == "Name").Count() > 0) applicationUser.Claims.Remove(applicationUser.Claims.First(cl => cl.ClaimType == "Name"));
                    applicationUser.Claims.Add(nameClaim);
                    if (!String.IsNullOrWhiteSpace(Subject) && type == "Teacher")
                    {
                        if (!_context.Subjects.Any(s => s.Name == Subject))
                        {
                            var newSubject = _context.Subjects.Add(new Subject() { Name = Subject }).Entity;
                            _context.SaveChanges();
                            var subjectClaim = new IdentityUserClaim<string>() { ClaimType = "Subject", ClaimValue = newSubject.Id.ToString() };
                            if (applicationUser.Claims.Where(cl => cl.ClaimType == "Subject").Count() > 0) applicationUser.Claims.Remove(applicationUser.Claims.First(cl => cl.ClaimType == "Subject"));
                            applicationUser.Claims.Add(subjectClaim);
                        }
                    }
                    await _userManager.UpdateAsync(applicationUser);
                    if (GroupId != 0)
                    {
                       var susr= _context.Users.Include(u=>u.Group).Single(u => u.Id == applicationUser.Id);
                        susr.GroupId = GroupId;
                        _context.SaveChanges();
                    }


                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", applicationUser.GroupId);
            return View(applicationUser);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            _context.Users.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
