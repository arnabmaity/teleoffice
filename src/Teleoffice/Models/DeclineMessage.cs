using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teleoffice.Models
{
    public class DeclineMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProfessionalId { get; set; }
        public int NotificationId { get; set; }
        public String Message { get; set; }
        public DateTime Time { get; set; }
        
    }
}
