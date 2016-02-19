using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Teleoffice.Models;

namespace Teleoffice.ViewModels.Professional
{
    public class DeclineMessageViewModel
    {
        [Required]
        public int ProfessionalId { get; set; }

        [Required]
        public String Message { get; set; }

        public DateTime Time { get; set; }

        public IList<Notification> Notifications { get; set; }
        public ApplicationUser Users { get; set; }

    }
}
