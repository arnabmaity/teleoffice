using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Teleoffice.Models;
using System.Dynamic;
using Teleoffice.ViewModels.Role;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Teleoffice.Controllers
{
    
    public class ManageUserController : Controller
    {
        private ApplicationDbContext context;

        public ManageUserController(ApplicationDbContext _context)
        {
            context = _context;
        }

        // GET: /<controller>/
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            //dynamic dy = new ExpandoObject();
            var  users = context.Users.OrderBy(u => u.UserName).ToArray();
            var userrole = (from a in context.Roles
                            join ft in context.UserRoles on a.Id equals ft.RoleId
                            join z in context.Users on ft.UserId equals z.Id
                            select new Roleuser { FName = z.FirstName, LName = z.LastName, Email = z.Email, Role = a.Name, Id = z.Id }).ToList();

            //dy.role = (from xt in context.UserRoles
            //            join a in context.Users on xt.UserId equals a.Id 
            //             from t in context.UserRoles.DefaultIfEmpty()
            //             select new Roleuser { FName = a.FirstName, LName = a.LastName, Email = a.Email, Role = null, Id = a.Id }).ToList();
            return View(userrole);
        }

        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult AddUserRole(String id)
        {
            dynamic vm = new ExpandoObject();
            vm.user = context.Users.Where(u => u.Id == id)
                .Single();
            vm.roles = context.Roles.OrderBy(c => c.Name).ToArray();
            return View(vm);
        }

        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteUserRole(String id, String roleid)
        {
            var userrole = context.UserRoles.Where(u => u.UserId == id & u.RoleId == roleid).Single();
            context.UserRoles.Remove(userrole);
            context.SaveChanges();
            return RedirectToAction("UserRoles", new { id = userrole.UserId });
        }

        public IActionResult AddRole(IdentityUserRole<string> userrole)
        {
            //var role = context.Roles.Where(z => z.Name == "Client").Single();
            //IdentityUserRole<string> userrole = new IdentityUserRole<string>();
            //userrole.UserId = id;
            //userrole.RoleId = role.Id;
            //context.UserRoles.Add(userrole);
            //context.SaveChanges();
            context.UserRoles.Add(userrole);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
