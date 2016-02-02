using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teleoffice.Models
{
    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String Subject { get; set; }
        public String Message { get; set; }
        public DateTime MeetingTime { get; set; }
        public String ClientId { get; set; }
        public String ProfessionalId { get; set; }
        public int IsValid { get; set; }

        public ApplicationUser Client { get; set; }
        public ApplicationUser Professional { get; set; }
    }
}
