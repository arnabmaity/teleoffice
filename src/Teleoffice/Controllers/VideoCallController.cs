using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Teleoffice.Models;
using System.Security.Claims;
using System.Dynamic;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Teleoffice.Controllers
{
    //[Authorize(Roles = "Client")]
    //[Authorize(Roles = "Professional")]
    public class VideoCallController : Controller
    {
        private ApplicationDbContext context;

        public VideoCallController(ApplicationDbContext _context)
        {
            context = _context;
        }
        // GET: /<controller>/
        public IActionResult Index(int Id)
        {
            dynamic dy = new ExpandoObject();
            var apt = context.Appointments.Where(z => (z.IsValid == 1 || z.IsValid == 2 || z.IsValid == 3) && z.Id == Id).Single();
            var usrrole1 = context.UserRoles.Where(z => z.UserId == User.GetUserId()).Single();
            dy.usrrole = usrrole1;

            var role1 = context.Roles.Where(z => z.Name == "Client").Single();
            dy.role = role1;
            var role2 = context.Roles.Where(z => z.Name == "Professional").Single();
            
            dy.id = Id;
            dy.client = context.Users.Where(z => z.Id == apt.ClientId).Single();
            
            dy.prof = context.Users.Where(z => z.Id == apt.ProfessionalId).Single();
            dy.user = context.Users.Where(z => z.Id == User.GetUserId()).Single();
            dy.app = apt;
            if (usrrole1.RoleId == role1.Id)
            {
                apt.IsValid = 2;
                context.Appointments.Update(apt);
                context.SaveChanges();
            }
            if (usrrole1.RoleId == role2.Id)
            {
                apt.IsValid = 3;
                context.Appointments.Update(apt);
                context.SaveChanges();
            }

            return View(dy);
        }

        public IActionResult Call(int id)
        {
            var apt = context.Appointments.Where(z => z.IsValid == 1 && z.Id == id).Single();
            ViewBag.app = apt;
            return View();
        }
    }
}
