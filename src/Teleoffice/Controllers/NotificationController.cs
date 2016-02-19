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
    [Authorize]
    public class NotificationController: Controller

    {
        private ApplicationDbContext context;

        public NotificationController(ApplicationDbContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public Boolean Read(int id)
        {
            Notification notify = context.Notifications.Where(z => z.Id == id).Single();
            notify.Read = 1;
            context.Notifications.Update(notify);
            context.SaveChanges();
            return true;
        }

    }
}
