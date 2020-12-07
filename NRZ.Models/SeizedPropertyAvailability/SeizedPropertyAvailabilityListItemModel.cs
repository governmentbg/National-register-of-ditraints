using System;

namespace NRZ.Models.SeizedPropertyAvailability
{
    public class SeizedPropertyAvailabilityListItemModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string RequestorPersonFullName { get; set; }

        public string InTheQualityOf { get; set; }

        public string RequestorCompanyName { get; set; }

        public string RequestorCompanyEik { get; set; }

        public string CheckedPersonFullName { get; set; }
    }
}
