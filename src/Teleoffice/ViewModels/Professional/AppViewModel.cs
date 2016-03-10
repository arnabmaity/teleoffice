using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Teleoffice.Models;

namespace Teleoffice.ViewModels.Professional
{
    public class AppViewModel
    {
        
        public String FName { get; set; }

        public String LName { get; set; }

        public String Subject { get; set; }

        public DateTime MeetTime { get; set; }

        public int AppId { get; set; }


        public int ProfessionalId { get; set; }
        public String Message { get; set; }
        

    }
}
