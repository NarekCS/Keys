using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private static Func<AdvancedContext, string, IEnumerable<Employee>> query 
            = EF.CompileQuery((AdvancedContext context, string searchTerm) => context.Employees.Where(e => EF.Functions.Like(e.FirstName, searchTerm)));

        public HomeController(AdvancedContext ctx) => context = ctx;
        //public IActionResult Index()
        //{
        //    return View(context.Employees.AsNoTracking());
        //}

        public IActionResult Index(string searchTerm)
        {
            return View(string.IsNullOrEmpty(searchTerm) ? context.Employees : query(context, searchTerm));
        }


        //public async Task<IActionResult> Index(string searchTerm)
        //{
        //    IQueryable<Employee> employees = context.Employees;
        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        employees = employees.Where(e => EF.Functions.Like(e.FirstName, searchTerm));
        //    }
        //    HttpClient client = new HttpClient();
        //    ViewBag.PageSize = (await client.GetAsync("http://apress.com")).Content.Headers.ContentLength;
        //    return View(await employees.ToListAsync());
        //}

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
                                                                            .AsNoTracking()
                                                                            .First(e => e.SSN == SSN && e.FirstName == firstName && e.FamilyName == familyName));
        }

        [HttpPost]
        public IActionResult Update(Employee employee)
        {
            Employee existing = context.Employees.AsTracking().First(e => e.SSN == employee.SSN && e.FirstName == employee.FirstName && e.FamilyName == employee.FamilyName);
            //Employee existing = context.Employees.Find(employee.SSN, employee.FirstName, employee.FamilyName);
            //if (employee.Id == default(long))
            //if (context.Employees.Count(e => e.SSN == employee.SSN) == 0) 
            //if (context.Employees.Find(employee.SSN, employee.FirstName, employee.FamilyName) == null)
            if (existing == null)
            //if (context.Employees.Count(e => e.SSN == employee.SSN && e.FirstName == employee.FirstName && e.FamilyName == employee.FamilyName) == 0)
            {
                context.Add(employee);
            }
            else
            {
                //context.Entry(existing).State = EntityState.Detached;
                //context.Update(employee);
                existing.Salary = employee.Salary;
            }
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            context.Attach(employee);
            employee.SoftDeleted = true;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
