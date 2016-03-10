using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace Teleoffice.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        //public DbSet<DeclineMessage> DeclineMsg{ get; set; }
        public DbSet<NotifyApp> NotifyApps { get; set; }
        public DbSet<Feedback> FeedBack { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
