using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teleoffice.Models
{
    public class Notification
    {
        public String Id { get; set; }
        public String Message { get; set; }
        public DateTime ReceivedTime { get; set; }
        public int Read { get; set; }
        public int IsDeleted { get; set; }

        public String UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
