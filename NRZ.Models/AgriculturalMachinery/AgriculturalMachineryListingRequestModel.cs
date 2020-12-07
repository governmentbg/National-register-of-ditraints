namespace NRZ.Models.AgriculturalMachinery
{
    public class AgriculturalMachineryListingRequestModel
    {
        public int Page { get; set; }

        public int ItemsPerPage { get; set; }

        public string SearchString { get; set; }

        public string SortBy { get; set; }

        public bool SortDesc { get; set; }

    }
}
