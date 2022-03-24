using System;
using System.Collections.Generic;

#nullable disable

namespace SimpulTechTest.Models
{
    public partial class DataAppointment
    {
        public int? AppointmentId { get; set; }
        public DateTime DateTime { get; set; }
        public int? PetId { get; set; }
        public string PetName { get; set; }
        public int? OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Notes { get; set; }
        public string Telephone { get; set; }
    }
}
