using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Teleoffice.Models;
using Microsoft.AspNet.Authorization;
using System.Dynamic;
using System.Security.Claims;


namespace Teleoffice.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController: Controller
    {
        private ApplicationDbContext context;

        public ClientController(ApplicationDbContext _context)
        {
            context = _context;
        }
        
        //Get
        public IActionResult Index()
        {
            dynamic dy = new ExpandoObject();
            dy.user = context.Users.Where(z => z.Id == User.GetUserId()).Single();
            dy.notify = context.Notifications.Where(z => z.UserId == User.GetUserId() && z.Read == 0).ToList();
            var role = context.Roles.Where(z => z.Name == "Professional").Single();
            var userroles = context.UserRoles.Where(z => z.RoleId == role.Id).ToList();
            List<ApplicationUser> prousers = new List<ApplicationUser>();
            if (userroles.Count > 0)
            {
                foreach (var ur in userroles)
                {
                    var tu = context.Users.Where(u => u.Id == ur.UserId).Single();
                    prousers.Add(tu);
                }

            }
            dy.pros = prousers;
            return View(dy);
            
        }

        //Get//create appointments
        [HttpGet]
        public IActionResult CreateAppointment(String ProfId)
        {
            dynamic dy = new ExpandoObject();
            dy.user = context.Users.Where(z => z.Id == User.GetUserId()).Single();
            dy.client = context.Users.Where(z => z.Id == User.GetUserId()).Single();
            dy.prof = context.Users.Where(z => z.Id == ProfId).Single();
            return View(dy);

        }

        //post//submit appointments

        [HttpPost]
        public IActionResult CreateAppointment(Appointment app)
        {
            //dynamic dy = new ExpandoObject();

            context.Appointments.Add(new Appointment
            {
                Subject = app.Subject,
                Message = app.Message,
                MeetingTime = app.MeetingTime,
                ClientId = app.ClientId,
                ProfessionalId = app.ProfessionalId,
                IsValid = 1
            });

            var client = context.Users.Where(u => u.Id == app.ClientId).Single();
            var prof = context.Users.Where(u => u.Id == app.ProfessionalId).Single();

            //notification for client
            Notification n = new Notification();            
            n.Message = "You booked an appointment with " + prof.FirstName + " " + prof.LastName;
            n.ReceivedTime = DateTime.Now;
            n.UserId = client.Id;
            n.Read = 0;
            n.IsDeleted = 0;

            //notification for prof
            Notification m = new Notification();
            m.Message = client.FirstName + " " + client.LastName + " booked an appointment with you!";
            m.ReceivedTime = DateTime.Now;
            m.UserId = prof.Id;
            m.Read = 0;
            n.IsDeleted = 0;

            context.Notifications.Add(n);
            context.Notifications.Add(m);
            context.SaveChanges();
            return RedirectToAction("Index");

        }


    }
}
