namespace NRZ.Models.Auction
{
    public class AuctionItemModel
    {
        public int Id { get; set; }
        public int? NRZId { get; set; }
        public string PropertyType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsManuallyAdded { get; set; }
        public string ObjectType { get; set; }
    }
}
