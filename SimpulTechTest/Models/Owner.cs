using System;
using System.Collections.Generic;

#nullable disable

namespace SimpulTechTest.Models
{
    public partial class Owner
    {
        public int OwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}
