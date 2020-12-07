using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Person
{
    public class RegixPersonModel
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public long? RequestId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string FullName => $"{FirstName ?? ""} {MiddleName ?? ""} {LastName ?? ""}";
    }
}
