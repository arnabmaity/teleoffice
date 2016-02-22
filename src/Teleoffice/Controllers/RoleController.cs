using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Teleoffice.Models;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Teleoffice.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private ApplicationDbContext context;

        public RoleController(ApplicationDbContext _context)
        {
            context = _context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var roles = context.Roles.OrderBy(r => r.Name).ToArray();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IdentityRole role)
        {
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = role.Name
                });
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        [Authorize (Roles ="Administrator")]
        public IActionResult DeleteRole( String Name)
        {
            var role = context.Roles.Where(z => z.Name == Name).Single();
            context.Roles.Remove(role);
            context.SaveChanges();
            return RedirectToAction(nameof(RoleController.Index), "Role");
        }
    }
}
