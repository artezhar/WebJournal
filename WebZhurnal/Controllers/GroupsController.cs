using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebZhurnal.Data;
using WebZhurnal.Models;

namespace WebZhurnal.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Groups.ToListAsync());
        }

        public async Task<IActionResult> SetTeachers()
        {
            int groupId = int.Parse(Request.Form["GroupId"]);
            foreach(var teacher in _context.Users.Include(u=>u.TeacherGroups).Where(u => u.Claims.Any(c => (c.ClaimType == "Type") && (c.ClaimValue == "Teacher"))))
            {
                teacher.TeacherGroups.RemoveAll(tg => tg.GroupId == groupId);
            }
            _context.SaveChanges();

            foreach (var teacherId in Request.Form.Where(i=>i.Key!="GroupId").Select(i=>i.Key))
            {
                var teacher = await _context.Users.Include(u=>u.TeacherGroups).FirstOrDefaultAsync(u => u.Id == teacherId);
                if(teacher!=null)
                    teacher.TeacherGroups.Add(new TeacherGroup() { TeacherId = teacherId, GroupId = groupId });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetSubjects()
        {
            int groupId = int.Parse(Request.Form["GroupId"]);
            foreach (var subject in _context.Subjects.Include(u => u.SubjectGroups))
            {
                subject.SubjectGroups.RemoveAll(tg => tg.GroupId == groupId);
            }
            _context.SaveChanges();

            foreach (var subjectId in Request.Form.Where(i => i.Key != "GroupId").Select(i => i.Key))
            {
                int sId = 0;
                if (int.TryParse(subjectId, out sId))
                {
                    var subject = await _context.Subjects.Include(u => u.SubjectGroups).FirstOrDefaultAsync(u => u.Id == sId);
                    if (subject != null)
                        subject.SubjectGroups.Add(new SubjectGroup() { SubjectId = sId, GroupId = groupId });
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.Include(g=>g.TeacherGroups).Include(g=>g.SubjectGroups)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            ViewBag.Users = _context.Users.Include(u => u.Claims).Include(u => u.TeacherGroups).ToList();
            ViewBag.Subjects = _context.Subjects.Include(s=>s.SubjectGroups).ToList();


            return View(@group);
        }

        
        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Group @group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(@group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewBag.Users = _context.Users.Include(u => u.Claims).Include(u => u.TeacherGroups).ToList();
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
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
            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);
            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
