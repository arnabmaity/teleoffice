using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teleoffice.ViewModels
{
    public class AppUserViewModel
    {
        public String FName { get; set; }
        public String LName { get; set; }
        public String Subject { get; set; }
        public DateTime MeetTime { get; set; }
        public int AppId { get; set; }

    }
}
