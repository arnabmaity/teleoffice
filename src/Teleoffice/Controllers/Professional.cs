using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Teleoffice.Models;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using System.Dynamic;


namespace Teleoffice.Controllers
{
    [Authorize(Roles = "Professional")]
    public class ProfessionalController : Controller
    {
        private ApplicationDbContext context;
        public ProfessionalController(ApplicationDbContext _context)
        {
            context = _context;
        }
        // Get
        public IActionResult Index()
        {
            dynamic obj = new ExpandoObject();
            obj.user = context.Users.Where(u => u.Id == User.GetUserId()).Single();
            obj.notifications = context.Notifications.Where(n => n.UserId == User.GetUserId() && n.Read == 0).ToList();
            return View(obj);
        }
    }
}
