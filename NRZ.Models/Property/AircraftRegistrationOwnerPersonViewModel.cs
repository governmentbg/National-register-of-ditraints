using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Property
{
    public class AircraftRegistrationOwnerPersonViewModel
    {
        public int Id { get; set; }
        public int RegistrationId { get; set; }
        public string Identifier { get; set; }
        public string Names { get; set; }
    }
}
