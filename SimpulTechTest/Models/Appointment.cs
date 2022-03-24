using System;
using System.Collections.Generic;

#nullable disable

namespace SimpulTechTest.Models
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime? DateTime { get; set; }
        public int? PetId { get; set; }
        public string Notes { get; set; }
    }
}
