using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teleoffice.Models
{
    public class Feedback
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String Message { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }

    }
}
