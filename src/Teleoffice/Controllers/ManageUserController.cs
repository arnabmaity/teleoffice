using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Teleoffice.Models;
using System.Dynamic;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Teleoffice.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManageUserController : Controller
    {
        private ApplicationDbContext context;

        public ManageUserController(ApplicationDbContext _context)
        {
            context = _context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var users = context.Users.OrderBy(u => u.UserName).ToArray();
            return View(users);
        }

        public IActionResult UserRoles(String id)
        {
            dynamic vm = new ExpandoObject();
            //context.Users.Include(r => r.Roles)
            vm.user = context.Users.Where(u => u.Id == id)
                .Single();
            //var rolemanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var urs = context.UserRoles.Where(ur => ur.UserId == id).ToArray();
            List<IdentityRole> roles = new List<IdentityRole>();
            IdentityRole role;
            foreach (var ur in urs)
            {
                role = context.Roles.Where(r => r.Id == ur.RoleId).Single();
                roles.Add(role);
            }
            vm.userroles = roles;
            return View(vm);
        }

        [HttpGet]
        public IActionResult AddUserRole(String id)
        {
            dynamic vm = new ExpandoObject();
            vm.user = context.Users.Where(u => u.Id == id)
                .Single();
            vm.roles = context.Roles.OrderBy(c => c.Name).ToArray();
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddUserRole(IdentityUserRole<String> userrole)
        {
            if (ModelState.IsValid)
            {
                context.UserRoles.Add(userrole);
                context.SaveChanges();
                return RedirectToAction("UserRoles", new { id = userrole.UserId });
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteUserRole(String id, String roleid)
        {
            var userrole = context.UserRoles.Where(u => u.UserId == id & u.RoleId == roleid).Single();
            context.UserRoles.Remove(userrole);
            context.SaveChanges();
            return RedirectToAction("UserRoles", new { id = userrole.UserId });
        }
    }
}
