using NRZ.Models.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IAuctionService
    {
        AuctionModel Get(int id);
        IQueryable<AuctionModel> GetAll(string participantId = null);
        Task<(bool, DateTime?)> AddBid(BidModel bid, string userName);
        Task Log(string userId, int auctionId, string msg);
        decimal HeigestBid(int auctionId);
        Task<(bool, DateTime?)> ValidateBid(BidModel model);
        Task<bool> CanConnect(int auctionId, string userId);
        IQueryable<AuctionModel> GetOngoingAuctions(string userId = null, string createdById = null);
        IQueryable<AuctionModel> GetFutureAuctions(string userId = null, string createdById = null);
        IQueryable<AuctionModel> GetFinishedAuctions(string userId = null, string createdById = null);
        AuctionsListResult GetByUserId(string userId);
        Task<List<AuctionResult>> ProcessFinishedAuctionsAsync();
    }
}
