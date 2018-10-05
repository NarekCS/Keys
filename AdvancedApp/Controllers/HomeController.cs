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

        public IActionResult Index()
        {
            //ViewBag.Secondaries = context.Set<SecondaryIdentity>();
            //return View(context.Employees.Include(e => e.OtherIdentity).OrderByDescending(e => EF.Property<DateTime>(e, "LastUpdated"))); 
            IEnumerable<Employee> data = context.Employees.Include(e => e.OtherIdentity).OrderByDescending(e => e.LastUpdated).ToArray();
            ViewBag.Secondaries = data.Select(e => e.OtherIdentity);
            return View(data);
        }


        //public IActionResult Index()
        //{
        //    return View(context.Employees.AsNoTracking());
        //}

        //public IActionResult Index(string searchTerm)
        //{
        //    return View(string.IsNullOrEmpty(searchTerm) ? context.Employees : query(context, searchTerm));
        //}


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
                                                                            //.AsNoTracking()
                                                                            .First(e => e.SSN == SSN && e.FirstName == firstName && e.FamilyName == familyName));
        }

        //[HttpPost]
        //public IActionResult Update(Employee employee)
        //{
        //    // Employee existing = context.Employees.AsTracking().First(e => e.SSN == employee.SSN && e.FirstName == employee.FirstName && e.FamilyName == employee.FamilyName);
        //    //Employee existing = context.Employees.Find(employee.SSN, employee.FirstName, employee.FamilyName);
        //    //if (employee.Id == default(long))
        //    //if (context.Employees.Count(e => e.SSN == employee.SSN) == 0) 
        //    //if (context.Employees.Find(employee.SSN, employee.FirstName, employee.FamilyName) == null)
        //    //if (existing == null)
        //    if (context.Employees.Count(e => e.SSN == employee.SSN && e.FirstName == employee.FirstName && e.FamilyName == employee.FamilyName) == 0)
        //    {
        //        context.Add(employee);
        //    }
        //    else
        //    {
        //        //context.Entry(existing).State = EntityState.Detached;
        //        //existing.Salary = employee.Salary;
        //        context.Update(employee);
        //    }
        //    context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public IActionResult Update(Employee employee, decimal salary)
        //{
        //    Employee existing = context.Employees.Find(employee.SSN, employee.FirstName, employee.FamilyName);
        //    if (existing == null)
        //    {
        //        //context.Entry(employee).Property("LastUpdated").CurrentValue = System.DateTime.Now; 
        //        context.Add(employee);
        //    }
        //    else
        //    {
        //        existing.Salary = salary;               
        //        context.Entry(existing).Property("LastUpdated").CurrentValue = System.DateTime.Now;
        //    }
        //    context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        public IActionResult Update(Employee employee)//, decimal originalSalary)
        {
            if (context.Employees.Count(e => e.SSN == employee.SSN && e.FirstName == employee.FirstName && e.FamilyName == employee.FamilyName) == 0)
            {
                context.Add(employee);
            }
            else
            {
                //Employee e = new Employee { SSN = employee.SSN, FirstName = employee.FirstName, FamilyName = employee.FamilyName, Salary = originalSalary };
                Employee e = new Employee { SSN = employee.SSN, FirstName = employee.FirstName, FamilyName = employee.FamilyName, RowVersion = employee.RowVersion };
                context.Employees.Attach(e);
                e.Salary = employee.Salary;
                e.LastUpdated = DateTime.Now;
            }
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(Employee employee)
        {

            //if (employee.OtherIdentity != null)
            //{
            //    //context.Set<SecondaryIdentity>().Remove(employee.OtherIdentity);                
            //    //SecondaryIdentity s = context.Set<SecondaryIdentity>().Find(employee.OtherIdentity.Id);  ???
            //    context.Attach(employee);
            //    employee.OtherIdentity.PrimarySSN = null;
            //    employee.OtherIdentity.PrimaryFirstName = null;
            //    employee.OtherIdentity.PrimaryFamilyName = null;
            //    employee.OtherIdentity = null;
            //}
            ////context.Set<SecondaryIdentity>().FirstOrDefault(id => id.PrimarySSN == employee.SSN && id.PrimaryFirstName == employee.FirstName && id.PrimaryFamilyName == employee.FamilyName);
            ////context.Employees.Remove(employee);           
            //context.Remove(employee);
            context.Attach(employee);
            employee.SoftDeleted = true;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
