using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Teleoffice.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public long Contact { get; set; }
        public String Gender { get; set; }

        public List<Notification> Notifications { get; set; }
        public ApplicationUser()
        {

        }
    }
}
