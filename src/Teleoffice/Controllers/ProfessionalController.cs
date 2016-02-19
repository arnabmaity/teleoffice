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
            var user = context.Users.Where(u => u.Id == User.GetUserId()).Single();
            var notifications = context.Notifications.Where(n => n.UserId == User.GetUserId() && n.Read == 0).ToList();
            var viewModel = new DeclineMessageViewModel
            {
                Notifications = notifications,
                Users = user
            };
            return View(viewModel);

            //dynamic obj = new ExpandoObject();
            //obj.user = context.Users.Where(u => u.Id == User.GetUserId()).Single();
            //obj.notifications = context.Notifications.Where(n => n.UserId == User.GetUserId() && n.Read == 0).ToList();
            //obj.viewm = new DeclineMessageViewModel();
            //return View(obj);
        }

        public IActionResult Accept( int id)
        {
            var notifid = context.Notifications.Where(z => z.Id == id).Single(); //some mistake here it cant be single
            notifid.IsApproved = 3;
            context.Notifications.Update(notifid);
            context.SaveChanges();
            var appuser = context.Appointments.Where(z => z.Id == notifid.AppointmentId).Single();
            var client = context.Users.Where(z => z.Id == appuser.ClientId).Single();
            var prof = context.Users.Where(z => z.Id == appuser.ProfessionalId).Single();

            //Notification for client
            Notification n = new Notification();
            n.Message =  prof.FirstName + " " + prof.LastName + " " +  "approved your appointment request.";
            n.ReceivedTime = DateTime.Now;
            n.UserId = client.Id;
            n.Read = 0;
            n.IsApproved = 1;

            //Notification for professional
            Notification m = new Notification();
            m.Message = "You approved the appointment with" + " " +client.FirstName + " " + client.LastName ;
            m.ReceivedTime = DateTime.Now;
            m.UserId = prof.Id;
            m.Read = 0;
            m.IsApproved = 1;
            //deleted the IsDelete column


            context.Notifications.Add(n);
            context.Notifications.Add(m);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //public IActionResult DeclineGet(int id)
        //{
        //    ViewData["id"] = id;
        //    return View();
        //}


        [HttpPost]
        public IActionResult Decline(DeclineMessageViewModel view, int id)
        {
            DeclineMessage model = new DeclineMessage();
            model.ProfessionalId = view.ProfessionalId;
            model.Message = view.Message;
            model.NotificationId = id;
            model.Time = view.Time;

            context.DeclineMsg.Add(model);
            context.SaveChanges();
           //context.DeclineMsg.Add( new DeclineMessage
           //{
           //   ProfessionalId = view.ProfessionalId
           //    NotificationId = id,
           //    Time = view.Time
           //});
                        
            var msg = context.DeclineMsg.Where(z => z.NotificationId == id).Single();
            var notifid = context.Notifications.Where(z => z.Id == id).Single();
            notifid.IsApproved = 3;
            //context.Notifications.Update(notifid);
            //context.SaveChanges();
            var appuser = context.Appointments.Where(z => z.Id == notifid.AppointmentId).Single();//some mistake...rectify it
            notifid.AppointmentId = appuser.Id;
            context.Notifications.Update(notifid);
            context.SaveChanges();
            var client = context.Users.Where(z => z.Id == appuser.ClientId).Single();
            var prof = context.Users.Where(z => z.Id == appuser.ProfessionalId).Single();

            //Notification for client
            Notification n = new Notification();
            n.Message = prof.FirstName + " " + prof.LastName + " " + "declined your request with the following message:" + "  " + msg.Message +"  " + "with preferred time: "+ msg.Time ;
            n.ReceivedTime = DateTime.Now;
            n.UserId = client.Id;
            n.Read = 0;
            n.IsApproved = 2;
            n.AppointmentId = notifid.AppointmentId;

            //Notification for professional
            Notification m = new Notification();
            m.Message = "You declined the appointment with" + " " + client.FirstName + " " + client.LastName;
            m.ReceivedTime = DateTime.Now;
            m.UserId = prof.Id;
            m.Read = 0;
            m.IsApproved = 2;
            //deleted the IsDelete column


            context.Notifications.Add(n);
            context.Notifications.Add(m);
            context.SaveChanges();
            return RedirectToAction("Index");
            
        }

       

    }
}
