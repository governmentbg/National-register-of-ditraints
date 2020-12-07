using NRZ.Models.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IAuctionRegisterService
    {
        AuctionRegisterModel GetById(int id);
        IQueryable<AnnouncemetRegisterGridModel> GetAllByUser(string userId);
        IQueryable<AnnouncemetRegisterGridModel> GetByAnnouncementAndUser(int id, string userId);
        IQueryable<AnnouncemetRegisterGridModel> GetByAnnouncementId(int announcementId);
        IQueryable<AnnouncemetRegisterGridModel> GetAllByCreator(string userId);
        Task CreateAsync(AuctionRegisterModel model, string userId);
        Task DeleteAsync(int id, string userId);
        Task ProccessAsync(ApprovalModel model, string userId);
    }
}
