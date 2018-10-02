﻿using System;
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
        public IActionResult Edit(string SSN)
        {
            return View(string.IsNullOrWhiteSpace(SSN) ? new Employee() : context.Employees.Include(e => e.OtherIdentity).First(e => e.SSN == SSN));
        }

        [HttpPost]
        public IActionResult Update(Employee employee)
        {
            if (context.Employees.Count(e => e.SSN == employee.SSN) == 0) //if (employee.Id == default(long))
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