using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Teleoffice.Models;
using Microsoft.AspNet.Authorization;
using Teleoffice.ViewModels;
using System.Security.Claims;
using Microsoft.Data.Entity.Internal;

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
            //dynamic dy = new ExpandoObject();
            ViewBag.user = context.Users.Where(z => z.Id == User.GetUserId()).Single();
            ViewBag.notify = context.Notifications.Where(z => z.UserId == User.GetUserId() && z.Read == 0).ToList();
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
            ViewBag.pros = prousers;
                        
            var apt = (from a in context.Appointments
                       join ft in context.Users on a.ProfessionalId equals ft.Id
                       where a.IsValid == 1 && a.ClientId == User.GetUserId()
                       select new AppUserViewModel { FName = ft.FirstName, LName = ft.LastName, Subject = a.Subject, MeetTime = a.MeetingTime, AppId = a.Id }).ToList();
             
            
            return View("Index", apt);
            
        }

        //Get//create appointments
        [HttpGet]
        public IActionResult CreateAppointment(String ProfId)
        {
            //dynamic dy = new ExpandoObject();
            ViewBag.user = context.Users.Where(z => z.Id == User.GetUserId()).Single();
            ViewBag.client = context.Users.Where(z => z.Id == User.GetUserId()).Single();
            ViewBag.prof = context.Users.Where(z => z.Id == ProfId).Single();
            return View();

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
                IsValid = 0    //1 after it is confirmed, 2 when declined
                
               
            });
            context.SaveChanges();
            var client = context.Users.Where(u => u.Id == app.ClientId).Single();
            var prof = context.Users.Where(u => u.Id == app.ProfessionalId).Single();
            var id = context.Appointments.Last();
            //notification for client
            Notification n = new Notification();            
            n.Message = "You requested an appointment with " + prof.FirstName + " " + prof.LastName;
            n.ReceivedTime = DateTime.Now;
            n.UserId = client.Id;
            n.Read = 0;
            n.IsApproved = 0;
            n.AppointmentId = id.Id;

            //notification for prof
            Notification m = new Notification();
            m.Message = client.FirstName + " " + client.LastName + "  requested an appointment with you!";
            m.ReceivedTime = DateTime.Now;
            m.UserId = prof.Id;
            m.Read = 0;
            m.IsApproved = 0;
            m.AppointmentId = id.Id;

            context.Notifications.Add(n);
            context.Notifications.Add(m);
            context.SaveChanges();
            return RedirectToAction("Index");

        }

        public IActionResult Agree(int id)
        {
            var notify = context.Notifications.Where(z => z.Id == id).Single();
            notify.IsApproved = 1;
            context.SaveChanges();
            var client = context.Users.Where(z => z.Id == notify.UserId).Single();
            var profid = context.Appointments.Where(z => z.Id == notify.AppointmentId).Single();
            profid.IsValid = 1;
            context.SaveChanges();
            var prof = context.Users.Where(z => z.Id == profid.ProfessionalId).Single();

            Notification n = new Notification();
            n.Message = "You confirmed an appointment with " + prof.FirstName + " " + prof.LastName;
            n.ReceivedTime = DateTime.Now;
            n.UserId = client.Id;
            n.Read = 0;
            n.IsApproved = 0;
            //n.AppointmentId = 0;

            //notification for prof
            Notification m = new Notification();
            m.Message = client.FirstName + " " + client.LastName + "  confirmed an appointment with you!";
            m.ReceivedTime = DateTime.Now;
            m.UserId = prof.Id;
            m.Read = 0;
            m.IsApproved = 0;
            m.AppointmentId = notify.AppointmentId;

            context.Notifications.Add(n);
            context.Notifications.Add(m);
            context.SaveChanges();


            return RedirectToAction("Index");
        }

        public IActionResult RequestAgain(int id)
        {
            var user = context.Notifications.Where(z => z.Id == id).Single();
            var profid = context.Appointments.Where(z => z.Id == user.AppointmentId).Single();
            var prof = context.Users.Where(z => z.Id == profid.ProfessionalId).Single();
            return RedirectToAction("CreateAppointment", new { ProfId = prof.Id });
        }

    }
}
