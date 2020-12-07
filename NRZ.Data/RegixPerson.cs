using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class RegixPerson
    {
        public RegixPerson()
        {
            DistraintDebtorPerson = new HashSet<Distraint>();
            DistraintInFavourOfPerson = new HashSet<Distraint>();
        }

        public int Id { get; set; }
        public string Identifier { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public long? RequestId { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Distraint> DistraintDebtorPerson { get; set; }
        public virtual ICollection<Distraint> DistraintInFavourOfPerson { get; set; }
    }
}
