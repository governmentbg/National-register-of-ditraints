using Microsoft.EntityFrameworkCore;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.Auction;
using NRZ.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Auctions
{
    public class AuctionService : BaseService, IAuctionService
    {
        public AuctionService(NRZContext context)
        : base(context, null)
        { }

        public AuctionModel Get(int id)
        {
            var result = from x in _context.Auction
                         where x.Id == id && !x.Deleted
                         select new AuctionModel()
                         {
                             Announcement = new AuctionAnnouncementModel()
                             {
                                 Title = x.Announcement.Title,
                                 Code = x.Announcement.Code,
                                 Items = from i in x.Announcement.AuctionItem
                                         let title = (i.PropertyType == Shared.Enums.PropertyType.VEHICLE.ToString() ?
                                         i.Vehicle.Model : (i.PropertyType == Shared.Enums.PropertyType.AGRIFORMACHINERY.ToString() ?
                                         i.AgriculturalMachinary.Type : (i.PropertyType == Shared.Enums.PropertyType.AIRCRAFT.ToString() ?
                                         i.Aircraft.ModelName : i.OtherProperty.Identifier)))
                                         let description = (i.PropertyType == Shared.Enums.PropertyType.VEHICLE.ToString() ?
                                         (i.Vehicle.VehicleType + " " + i.Vehicle.Model + " " + i.Vehicle.Color) : (i.PropertyType == Shared.Enums.PropertyType.AGRIFORMACHINERY.ToString() ?
                                         (i.AgriculturalMachinary.Type + " " + i.AgriculturalMachinary.RegistrationNumber) : (i.PropertyType == Shared.Enums.PropertyType.AIRCRAFT.ToString() ?
                                         (i.Aircraft.ModelName + " " + i.Aircraft.ProducerName + i.Aircraft.ProducerCountryName) : i.OtherProperty.Description)))
                                         select new AuctionItemModel()
                                         {
                                             Id = i.Id,
                                             NRZId = i.Nrzid,
                                             PropertyType = i.PropertyTypeNavigation.Name,
                                             Title = title,
                                             Description = description
                                         }
                             },
                             BidStep = x.BidStep,
                             Completed = x.Completed,
                             CurrentPrice = x.AuctionBid.Max(b => b.Bid),
                             EndDate = DateTime.SpecifyKind(x.EndDate, DateTimeKind.Utc),
                             EndPrice = x.EndPrice,
                             Id = x.Id,
                             StartDate = DateTime.SpecifyKind(x.StartDate, DateTimeKind.Utc),
                             StartPrice = x.StartPrice,
                             Winner = x.WinnerNavigation.PersonUser.FirstOrDefault().ToModel()
                         };

            return result.SingleOrDefault();
        }

        public IQueryable<AuctionModel> GetAll(string participantId = null)
        {
            var result = from x in _context.Auction
                         where !x.Deleted
                         select new AuctionModel()
                         {
                             Announcement = new AuctionAnnouncementModel()
                             {
                                 Id = x.AnnouncementId,
                                 Title = x.Announcement.Title,
                                 Code = x.Announcement.Code,
                                 Items = from i in x.Announcement.AuctionItem
                                         let title = (i.PropertyType == Shared.Enums.PropertyType.VEHICLE.ToString() ?
                                         i.Vehicle.Model : (i.PropertyType == Shared.Enums.PropertyType.AGRIFORMACHINERY.ToString() ?
                                         i.AgriculturalMachinary.Type : (i.PropertyType == Shared.Enums.PropertyType.AIRCRAFT.ToString() ?
                                         i.Aircraft.ModelName : i.OtherProperty.Identifier)))
                                         let description = (i.PropertyType == Shared.Enums.PropertyType.VEHICLE.ToString() ?
                                         (i.Vehicle.VehicleType + " " + i.Vehicle.Model + " " + i.Vehicle.Color) : (i.PropertyType == Shared.Enums.PropertyType.AGRIFORMACHINERY.ToString() ?
                                         (i.AgriculturalMachinary.Type + " " + i.AgriculturalMachinary.RegistrationNumber) : (i.PropertyType == Shared.Enums.PropertyType.AIRCRAFT.ToString() ?
                                         (i.Aircraft.ModelName + " " + i.Aircraft.ProducerName + i.Aircraft.ProducerCountryName) : i.OtherProperty.Description)))
                                         select new AuctionItemModel()
                                         {
                                             Id = i.Id,
                                             NRZId = i.Nrzid,
                                             PropertyType = i.PropertyTypeNavigation.Name,
                                             Title = title,
                                             Description = description
                                         }
                             },
                             BidStep = x.BidStep,
                             Completed = x.Completed,
                             CreatedBy = x.CreatedBy,
                             CreatedOn = x.CreatedOn,
                             CurrentPrice = x.AuctionBid.Max(b => b.Bid),
                             EndDate = x.EndDate,
                             EndPrice = x.EndPrice,
                             Id = x.Id,
                             ItemsCount = x.Announcement.AuctionItem.Count(),
                             StartDate = x.StartDate,
                             StartPrice = x.StartPrice,
                             Started = x.StartDate < DateTime.UtcNow && !x.Completed,
                             CanParticipate = string.IsNullOrEmpty(participantId) ? false : x.Announcement.AuctionRegistration.FirstOrDefault(y => y.ParticipantId == participantId 
                                                                                                                                                && !y.Deleted
                                                                                                                                                && (y.IsApproved.HasValue && y.IsApproved.Value)) != null
                         };

            return result;
        }

        public IQueryable<AuctionModel> GetOngoingAuctions(string userId = null, string createdById = null)
        {
            var currentDate = DateTime.UtcNow;
            var result = GetAll(userId).Where(x => (x.StartDate < currentDate
                                                && x.EndDate > currentDate)
                                                && !x.Completed);

            if (!string.IsNullOrWhiteSpace(createdById))
            {
                result = result.Where(x => x.CreatedBy == createdById);
            }

            return result;
        }

        public IQueryable<AuctionModel> GetFutureAuctions(string userId = null, string createdById = null)
        {
            var currentDate = DateTime.UtcNow;
            var result = GetAll(userId).Where(x => x.StartDate > currentDate);

            if (!string.IsNullOrWhiteSpace(createdById))
            {
                result = result.Where(x => x.CreatedBy == createdById);
            }

            return result;
        }

        public IQueryable<AuctionModel> GetFinishedAuctions(string userId = null, string createdById = null)
        {
            var currentDate = DateTime.UtcNow;
            var result = GetAll(userId).Where(x => x.EndDate < currentDate || x.Completed);

            if (!string.IsNullOrWhiteSpace(createdById))
            {
                result = result.Where(x => x.CreatedBy == createdById);
            }

            return result;
        }

        public AuctionsListResult GetByUserId(string userId)
        {
            var currentDate = DateTime.UtcNow;
            var result = new AuctionsListResult();
            var all = GetAll().Where(x => x.CreatedBy == userId);

            result.Ongoing = GetOngoingAuctions().Where(x => x.CreatedBy == userId);
            result.Future = GetFutureAuctions().Where(x => x.CreatedBy == userId);
            result.Finished = GetFinishedAuctions().Where(x => x.CreatedBy == userId);

            return result;
        }

        public async Task<(bool, DateTime?)> AddBid(BidModel bid, string userName)
        {
            var auc = await _context.Auction.Include(x => x.AuctionBid).SingleOrDefaultAsync(x => x.Id == bid.AuctionId);
            (bool, DateTime?) validationResult = ValidateBid(bid, auc);

            AuctionBid item = new AuctionBid()
            {
                AuctionId = bid.AuctionId,
                Bid = bid.Bid,
                BidderId = bid.BidderId,
                ClientTime = bid.ClientTime,
                PreviousBidId = bid.PreviousBidId,
                ServerTime = bid.ServerTime,
                TimeStamp = bid.TimeStamp,
                TimeStampTime = bid.TimestampTime,
                Valid = validationResult.Item1
            };

            AuctionBid prevBid = auc.AuctionBid.Where(x => x.Valid).LastOrDefault();
            if (prevBid != null)
            {
                prevBid = _context.AuctionBid.Find(bid.PreviousBidId);

                if (prevBid != null)
                {
                    prevBid.NextBid = item;
                }
            }

            await _context.AuctionBid.AddAsync(item);
            await _context.SaveChangesAsync();

            //TODO Set timestamp

            if (item.Valid)
            {
                await Log(bid.BidderId, bid.AuctionId, $"Нова цена от {bid.Bid} лв." + (prevBid != null ? $"Предишна цена {prevBid.Bid} лв." : ""));
            }
            else
            {
                await Log(bid.BidderId, bid.AuctionId, $"Невалидна цена от {bid.Bid} лв." + prevBid != null ? $"Предишна цена {prevBid.Bid} лв." : "");
            }

            return validationResult;
        }

        public async Task Log(string userId, int auctionId, string msg)
        {
            AuctionLog log = new AuctionLog()
            {
                AuctionId = auctionId,
                CreatedOn = DateTime.UtcNow,
                Text = msg,
                UserId = userId
            };

            await _context.AuctionLog.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public decimal HeigestBid(int auctionId)
        {
            return _context.AuctionBid.Where(x => x.AuctionId == auctionId).Max(x => x.Bid);
        }

        private (bool, DateTime?) ValidateBid(BidModel model, Auction auc)
        {
            if (model.ServerTime > auc.EndDate)
            {
                return (false, null);
            }

            var currentMax = auc.AuctionBid.Count > 0 ? auc.AuctionBid.Where(x => x.Valid).Max(x => x.Bid) : auc.StartPrice;
            bool valid = model.Bid >= currentMax + auc.BidStep;
            var timeDiff = auc.EndDate - model.ServerTime;

            if (valid && timeDiff.TotalMinutes < 10)
            {
                auc.EndDate = auc.EndDate.AddMinutes(10);
                return (valid, auc.EndDate);
            }
            else
            {
                return (valid, null);
            }
        }
        public async Task<(bool, DateTime?)> ValidateBid(BidModel model)
        {
            var auc = await _context.Auction.Include(x => x.AuctionBid).SingleOrDefaultAsync(x => x.Id == model.AuctionId);
            return ValidateBid(model, auc);
        }

        public async Task<bool> CanConnect(int auctionId, string userId)
        {
            var auc = await _context.Auction
                                    .Include(x => x.Announcement)
                                    .ThenInclude(x => x.AuctionRegistration)
                                    .SingleOrDefaultAsync(x => x.Id == auctionId);

            if (auc.EndDate < DateTime.UtcNow || auc.Completed)
            {
                return false;
            }

            if (auc.Announcement.CreatedBy != userId && !auc.Announcement.AuctionRegistration.Any(x => (x.IsApproved.HasValue && x.IsApproved.Value) && x.ParticipantId == userId))
            {
                return false;
            }

            return true;
        }

        public async Task<List<AuctionResult>> ProcessFinishedAuctionsAsync()
        {
            var aucs = await _context.Auction
                                     .Include(x => x.AuctionBid)
                                     .Where(x => !x.Completed && x.EndDate < DateTime.UtcNow)
                                     .ToListAsync();

            List<AuctionResult> results = new List<AuctionResult>();

            foreach (var auc in aucs)
            {
                auc.Completed = true;

                if (auc.AuctionBid.Count > 0)
                {
                    var winningBid = auc.AuctionBid.Where(x => x.Valid).OrderBy(x => x.Bid).LastOrDefault();
                    if (winningBid != null)
                    {
                        auc.Winner = winningBid.BidderId;
                        auc.EndPrice = winningBid.Bid;

                        results.Add(new AuctionResult()
                        { 
                            AuctionId = auc.Id,
                            MaxPrice = winningBid.Bid,
                            WinnerId = winningBid.BidderId
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return results;
        }
    }
}
