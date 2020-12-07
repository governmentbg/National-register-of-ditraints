using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class AircraftRegistrationOperatorPerson
    {
        public int Id { get; set; }
        public int RegistrationId { get; set; }
        public string Identifier { get; set; }
        public string Names { get; set; }

        public virtual AircraftRegistration Registration { get; set; }
    }
}
