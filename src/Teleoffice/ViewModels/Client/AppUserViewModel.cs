﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Teleoffice.Models;

namespace Teleoffice.ViewModels.Client
{
    public class AppUserViewModel
    {

        public String FName { get; set; }

        public String LName { get; set; }

        public String Subject { get; set; }

        public DateTime MeetTime { get; set; }

        public String Message { get; set; }

        public int AppId { get; set; }

        //public int ClientId { get; set; }
        //public String Message { get; set; }


    }
}
