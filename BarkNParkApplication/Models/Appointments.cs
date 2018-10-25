using System;
using System.Collections.Generic;

namespace BarkNParkApplication.Models
{
    public partial class Appointments
    {
        public int ApptId { get; set; }
        public int ApptUser { get; set; }
        public DateTime ApptCheckin { get; set; }
        public DateTime ApptCheckout { get; set; }
        public int ApptStation { get; set; }
    }
}
