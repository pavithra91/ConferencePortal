using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.App_Code
{
    public class TempDate
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string PickUpTime { get; set; }

        public string DropOffTime { get; set; }
    }
}