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
    //[Authorize(Roles = "Administrator")]
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
            var apt = context.Appointments.Where(z => z.IsValid == 1 && z.Id == Id).Single();
            dy.app = apt;
            return View(dy);
        }
    }
}
