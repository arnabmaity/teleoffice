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
    public class HomeController : Controller
    {
        private ApplicationDbContext context;
        public HomeController(ApplicationDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Feedback()
        {
            //ViewBag.user = context.Users.Where(z => z.Id == User.GetUserId()).Single();
            return View();
        }

        [HttpPost]
        public IActionResult Feedback(Feedback fb)
        {
            context.FeedBack.Add(new Feedback
            {
                Name = fb.Name,
                Email = fb.Email,
                Message = fb.Message
            });
            context.SaveChanges();
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult ViewFeedbacks()
        {
            ViewBag.fb = context.FeedBack.ToList();
           return View();
        }

    }
}
