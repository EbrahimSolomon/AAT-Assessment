using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAT_Assessment3.Shared
{
    public class Registration
    {
        public int RegistrationId { get; set; }
        public int EventId { get; set; } 
        public string UserEmail { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ReferenceNumber { get; set; }
    }
}
