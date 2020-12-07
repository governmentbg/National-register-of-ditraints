using NRZ.Models.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IAnnouncementsService
    {
        Task<AuctionAnnouncementModel> Get(int id);
        IQueryable<AuctionAnnouncementModel> GetAll();
        Task Create(AuctionAnnouncementModel model, string userId);
        Task Edit(AuctionAnnouncementModel model, string userId);
        Task Delete(int id, string userId);
        IQueryable<AuctionAnnouncementModel> GetByUserId(string userId);
    }
}
