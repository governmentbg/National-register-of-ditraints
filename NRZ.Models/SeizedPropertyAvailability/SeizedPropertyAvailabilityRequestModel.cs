using NRZ.Models.Company;
using NRZ.Models.Person;

namespace NRZ.Models.SeizedPropertyAvailability
{
    public class SeizedPropertyAvailabilityRequestModel
    {
        public PersonModel Requester { get; set; }

        public string RequesterCompanyRepresentative { get; set; }

        public string RequesterCompanyEik { get; set; }

        public string RequesterCompanyCaseNumber { get; set; }

        public bool IsCheckedPerson { get; set; }

        public PersonModel CheckedPerson { get; set; }

        public CompanyModel CheckedCompany { get; set; }

        public string InTheQualityOfPersonTypeCode { get; set; }
    }
}
