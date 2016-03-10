using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Teleoffice.Models;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using System.Dynamic;
using Teleoffice.ViewModels.Professional;

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
            ViewBag.appnotify = context.Appointments.Where(z => z.IsValid == 1).ToList();
            ViewBag.user = context.Users.Where(u => u.Id == User.GetUserId()).Single();
            ViewBag.notify = context.Notifications.Where(n => n.UserId == User.GetUserId() && n.Read == 0).ToList().OrderByDescending(z => z.ReceivedTime);
            
            return View("Index");

        }

        public IActionResult Accept( int id)
        {
            var notifyapp = context.NotifyApps.Where(z => z.ProfNotificationId == id).Single();
            var appuser = context.Appointments.Where(z => z.Id == notifyapp.AppointmentId).Single();
            var client = context.Users.Where(z => z.Id == appuser.ClientId).Single();
            var prof = context.Users.Where(z => z.Id == appuser.ProfessionalId).Single();

            appuser.IsValid = 1; // accepted request
            context.Appointments.Update(appuser);
            
            var n = context.Notifications.Where(z => z.Id == id).Single();
            n.Message = "You confirmed the appointment with " + client.FirstName + " " + client.LastName;
            n.ReceivedTime = DateTime.Now;
            n.Status = 2;

            var m = context.Notifications.Where(z => z.Id == notifyapp.ClientNotificationId).Single();
            m.Message = prof.FirstName + " " + prof.LastName + " " + "confirmed the appointment with you";
            m.ReceivedTime = DateTime.Now;
            m.Status = 2;

            context.Notifications.Update(n);
            context.Notifications.Update(m);
            context.SaveChanges();
           
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Decline(AppViewModel ap, int id)
        {
            var notifyapp = context.NotifyApps.Where(z => z.ProfNotificationId == id).Single();
            var appuser = context.Appointments.Where(z => z.Id == notifyapp.AppointmentId).Single();
            var client = context.Users.Where(z => z.Id == appuser.ClientId).Single();
            var prof = context.Users.Where(z => z.Id == appuser.ProfessionalId).Single();

            appuser.IsValid = 2; //declined request
            appuser.Message = ap.Message;
            appuser.MeetingTime = ap.MeetTime;
            context.Appointments.Update(appuser);
            
            var n = context.Notifications.Where(z => z.Id == id).Single();
            n.Message = "You declined the appointment with " + client.FirstName + " " + client.LastName;
            n.ReceivedTime = DateTime.Now;
            n.Status = 3;

            var m = context.Notifications.Where(z => z.Id == notifyapp.ClientNotificationId).Single();
            m.Message = prof.FirstName + " " + prof.LastName + " " + "declined the appointment with you";
            m.ReceivedTime = DateTime.Now;
            m.Status = 3;
            
            context.Notifications.Update(n);
            context.Notifications.Update(m);
            context.SaveChanges();
            return RedirectToAction("Index");
            
        }
        
        public IActionResult ViewAppointments()
        {
            ViewBag.appnotify = context.Appointments.Where(z => z.IsValid == 1).ToList();
            ViewBag.user = context.Users.Where(u => u.Id == User.GetUserId()).Single();
            ViewBag.notify = context.Notifications.Where(n => n.UserId == User.GetUserId() && n.Read == 0).ToList();
           
            var apt = (from a in context.Appointments
                       join ft in context.Users on a.ClientId equals ft.Id
                       where (a.IsValid == 1 || a.IsValid == 2) && a.ProfessionalId == User.GetUserId()
                       select new AppViewModel { FName = ft.FirstName, LName = ft.LastName, Subject = a.Subject, MeetTime = a.MeetingTime, AppId = a.Id }).ToList();
            //some imp changes....remember
            return View("ViewAppointments", apt.OrderBy(z => z.MeetTime));
         }   

        public IActionResult NotificationDetail(int id)
        {
            ViewBag.user = context.Users.Where(u => u.Id == User.GetUserId()).Single();
            ViewBag.notify = context.Notifications.Where(z => z.Id == id).Single();
            var notifyapp = context.NotifyApps.Where(z => z.ProfNotificationId == id).Single();
            ViewBag.app = context.Appointments.Where(z => z.Id == notifyapp.AppointmentId).Single();
            return View();
        }

    }
}
