using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvancedApp.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdvancedApp.Controllers
{
    public class QueryController : Controller
    {
        private AdvancedContext context;
        public QueryController(AdvancedContext ctx) => context = ctx;
        public IActionResult ServerEval()
        {
            return View("Query", context.Employees.Where(e => e.Salary > 150_000));
        }
        public IActionResult ClientEval()
        {
            return View("Query", context.Employees.Where(e => IsHighEarner(e)));
        }
        private bool IsHighEarner(Employee e)
        {
            return e.Salary > 150_000;
        }
    }
}
