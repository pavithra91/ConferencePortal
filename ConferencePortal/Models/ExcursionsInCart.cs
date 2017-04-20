using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.Models
{
    public class ExcursionsInCart
    {
        public Excursion Excursion { get; set; }
        public int NoOfAdults { get; set; }
        public double Price { get; set; }
    }
}