using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teleoffice.Models
{
    public class NotifyApp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int ClientNotificationId { get; set; }
        public int ProfNotificationId { get; set; }        
        //public ApplicationUser User { get; set; }

    }
}
