using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebZhurnal.Data;
using WebZhurnal.Models;

namespace WebZhurnal.Controllers
{
    public class LogController : Controller
    {

        private readonly ApplicationDbContext _context;
        public LogController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Rates()
        {
            return View(_context.LogItems.Where(l=>l.Type==LogItemType.Rate).OrderByDescending(l=>l.DateTime).ToList().Cast<RateItem>());
        }
    }
}