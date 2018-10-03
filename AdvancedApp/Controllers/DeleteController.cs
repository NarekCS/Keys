using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvancedApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdvancedApp.Controllers
{
    public class DeleteController : Controller
    {
        private AdvancedContext context;
        public DeleteController(AdvancedContext ctx) => context = ctx;
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(context.Employees.Where(e => e.SoftDeleted).Include(e => e.OtherIdentity).IgnoreQueryFilters());
        }
        [HttpPost]
        public IActionResult Restore(Employee employee)
        {
            context.Employees.IgnoreQueryFilters()
                .First(e => e.SSN == employee.SSN && e.FirstName == employee.FirstName && e.FamilyName == employee.FamilyName).SoftDeleted = false;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
