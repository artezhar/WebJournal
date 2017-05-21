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

namespace WebZhurnal.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUser.Include(u=>u.Claims). ToListAsync());
        }

       

        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
            ViewData["Subjects"] = new SelectList(_context.Subjects, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,PhoneNumber,GroupId")] ApplicationUser applicationUser, string type, string name, string Subject, string password="`1qw23E")
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

                    _context.Add(applicationUser);
                    await _context.SaveChangesAsync();
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

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", applicationUser.GroupId);
            return View(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,PhoneNumber,GroupId")]ApplicationUser applicationUser, string type, string name, string Subject)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var identityClaim = new IdentityUserClaim<string> { ClaimType = "Type", ClaimValue = type };
                    if(!applicationUser.Claims.Any(c=>c.ClaimType==identityClaim.ClaimType&&c.ClaimValue==identityClaim.ClaimValue))
                        applicationUser.Claims.Add(identityClaim);
                    var nameClaim = new IdentityUserClaim<string> { ClaimType = "Name", ClaimValue = name };
                    if (!applicationUser.Claims.Any(c => c.ClaimType == nameClaim.ClaimType && c.ClaimValue == nameClaim.ClaimValue))
                        applicationUser.Claims.Add(nameClaim);
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();

                    if (!String.IsNullOrWhiteSpace(Subject) && type == "Teacher")
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

                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
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

            var applicationUser = await _context.ApplicationUser
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
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
