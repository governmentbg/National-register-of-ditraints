using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class Address
    {
        public Address()
        {
            Company = new HashSet<Company>();
            Person = new HashSet<Person>();
            Property = new HashSet<Property>();
        }

        public int Id { get; set; }
        public int RegionId { get; set; }
        public int MunicipalityId { get; set; }
        public int CityId { get; set; }
        public string StreetAddress { get; set; }

        public virtual Cities City { get; set; }
        public virtual Municipalities Municipality { get; set; }
        public virtual Regions Region { get; set; }
        public virtual ICollection<Company> Company { get; set; }
        public virtual ICollection<Person> Person { get; set; }
        public virtual ICollection<Property> Property { get; set; }
    }
}
