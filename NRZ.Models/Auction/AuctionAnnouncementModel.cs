using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Auction
{
    public class AuctionAnnouncementModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string PropertyType { get; set; }
        public string PropertyTypeName { get; set; }
        public decimal StartPrice { get; set; }
        public decimal BidStep { get; set; }
        public string Order { get; set; }
        public string OrderName { get; set; }
        public string Description { get; set; }
        public int RegistrationsCount { get; set; }
        public DateTime RegisterStartDate { get; set; }
        public DateTime RegisterEndDate { get; set; }
        public DateTime AuctionStartDate { get; set; }
        public DateTime AuctionEndDate { get; set; }
        public IEnumerable<AuctionItemModel> Items { get; set; }
        public IEnumerable<AttachmentModel> Attachments { get; set; }
    }
}
