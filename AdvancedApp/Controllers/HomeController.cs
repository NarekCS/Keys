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
    public class HomeController : Controller
    {
        private AdvancedContext context;
        public HomeController(AdvancedContext ctx) => context = ctx;
        public IActionResult Index()
        {
            return View(context.Employees);
        }
        //public IActionResult Edit(long id)
        //{
        //    return View(id == default(long) ? new Employee() : context.Employees.Include(e => e.OtherIdentity).First(e => e.Id == id));
        //}
        //public IActionResult Edit(string SSN)
        //{
        //    return View(string.IsNullOrWhiteSpace(SSN) ? new Employee() : context.Employees.Include(e => e.OtherIdentity).First(e => e.SSN == SSN));
        //}
        public IActionResult Edit(string SSN, string firstName, string familyName)
        {
            return View(string.IsNullOrWhiteSpace(SSN) ? new Employee() : context.Employees.Include(e => e.OtherIdentity)
                                                                            .First(e => e.SSN == SSN && e.FirstName == firstName && e.FamilyName == familyName));
        }

        [HttpPost]
        public IActionResult Update(Employee employee)
        {
            //if (employee.Id == default(long))
            //if (context.Employees.Count(e => e.SSN == employee.SSN) == 0) 
            if (context.Employees.Count(e => e.SSN == employee.SSN && e.FirstName == employee.FirstName && e.FamilyName == employee.FamilyName) == 0)
            {
                context.Add(employee);
            }
            else
            {
                context.Update(employee);
            }
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
