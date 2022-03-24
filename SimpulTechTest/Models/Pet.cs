using System;
using System.Collections.Generic;

#nullable disable

namespace SimpulTechTest.Models
{
    public partial class Pet
    {
        public int PetId { get; set; }
        public string PetName { get; set; }
        public string PetType { get; set; }
        public string PetAge { get; set; }
        public int OwnerId { get; set; }
    }
}
