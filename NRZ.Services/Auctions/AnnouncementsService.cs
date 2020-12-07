using Microsoft.EntityFrameworkCore;
using NRZ.Data;
using NRZ.Models.Auction;
using NRZ.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Auctions
{
    public class AnnouncementsService : BaseService, IAnnouncementsService
    {
        public AnnouncementsService(NRZContext context)
        : base(context, null)
        { }

        public async Task Create(AuctionAnnouncementModel model, string userId)
        {
            AuctionAnnouncement entry = new AuctionAnnouncement()
            {
                AuctionEndDate = model.AuctionEndDate.ToUniversalTime(),
                AuctionStartDate = model.AuctionStartDate.ToUniversalTime(),
                BidStep = model.BidStep,
                Code = model.Code,
                Description = model.Description,
                Order = model.Order,
                PropertyType = model.PropertyType,
                RegisterEndDate = model.RegisterEndDate.ToUniversalTime(),
                RegisterStartDate = model.RegisterStartDate.ToUniversalTime(),
                Title = model.Title,
                StartPrice = model.StartPrice,
                UserId = userId
            };

            SetCreateStamp(entry, userId);
            _context.AuctionAnnouncement.Add(entry);

            foreach (var item in model.Items)
            {

                AuctionItem ai = new AuctionItem()
                {
                    Auction = entry,
                    Description = item.Description,
                    PropertyType = item.PropertyType
                };

                if (item.IsManuallyAdded)
                {
                    // Ръчно добавен
                    switch (Enum.Parse(typeof(Shared.Enums.PropertyType), item.ObjectType))
                    {
                        case Shared.Enums.NonNrzPropertyType.Asset:
                            ai.OtherPropertyId = item.NRZId;
                            break;
                        case Shared.Enums.NonNrzPropertyType.Property:
                            ai.RealEstateId = item.NRZId;
                            break;
                        default:
                            break;
                    }
                } else
                {
                    // Намерен от регистър на НРЗ
                    switch (Enum.Parse(typeof(Shared.Enums.PropertyType), item.PropertyType))
                    {
                        case Shared.Enums.PropertyType.AIRCRAFT: 
                            ai.AircraftId = item.NRZId;
                            break;
                        case Shared.Enums.PropertyType.AGRIFORMACHINERY:
                            ai.AgriculturalMachinaryId = item.NRZId;
                            break;
                        case Shared.Enums.PropertyType.OTHER:
                            ai.OtherPropertyId = item.NRZId;
                            break;
                        case Shared.Enums.PropertyType.VEHICLE:
                            ai.VehicleId = item.NRZId;
                            break;
                        case Shared.Enums.PropertyType.REALESTATE:
                            ai.RealEstateId = item.NRZId;
                            break;
                        case Shared.Enums.PropertyType.VESSEL:
                            //TODO
                            break;
                        default:
                            break;
                    }
                }

                SetCreateStamp(ai, userId);
                _context.AuctionItem.Add(ai);
            }

            if (model.Attachments != null && model.Attachments.Any())
            {
                foreach (var item in model.Attachments)
                {
                    Attachment att = new Attachment()
                    {
                        FileName = item.FileName,
                        ContentType = item.ContentType,
                        FileType = item.FileName.Split('.').Last(),
                        Content = item.Content
                    };
                    _context.Attachment.Add(att);
                    entry.AnnouncementAttachments.Add(new AnnouncementAttachments() { Attachment = att, Announcement = entry });
                }
            }

            Auction auc = new Auction()
            { 
                Announcement = entry,
                Completed = false,
                BidStep = entry.BidStep,
                Deleted = false,
                EndDate = entry.AuctionEndDate,
                StartDate = entry.AuctionStartDate,
                StartPrice = entry.StartPrice,
            };

            SetCreateStamp(auc, userId);

            await _context.Auction.AddAsync(auc);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(AuctionAnnouncementModel model, string userId)
        {
            var dbEntry = await _context.AuctionAnnouncement.FindAsync(model.Id);

            if (dbEntry != null)
            {
                dbEntry.AuctionEndDate = model.AuctionEndDate;
                dbEntry.AuctionStartDate = model.AuctionStartDate;
                dbEntry.BidStep = model.BidStep;
                dbEntry.Code = model.Code;
                dbEntry.PropertyType = model.PropertyType;
                dbEntry.RegisterEndDate = model.RegisterEndDate;
                dbEntry.RegisterStartDate = model.RegisterStartDate;
                dbEntry.Title = model.Title;
                dbEntry.StartPrice = model.StartPrice;

                foreach (var item in model.Items)
                {

                    AuctionItem ai = new AuctionItem()
                    {
                        Auction = dbEntry,
                        Description = item.Description,
                        Nrzid = item.NRZId,
                        PropertyType = item.PropertyType
                    };

                    SetCreateStamp(ai, userId);
                    _context.AuctionItem.Add(ai);
                }

                if (model.Attachments != null && model.Attachments.Count() > 0)
                {
                    //Remove attachments
                    var attToRemove = dbEntry.AnnouncementAttachments
                                                  .Where(x => !model.Attachments.Any(y => y.Id == x.AttachmentId))
                                                  .Select(x => x.Attachment);

                    _context.Attachment.RemoveRange(attToRemove);


                    //Add attachments
                    foreach (var attachment in model.Attachments)
                    {
                        if (attachment.Id == default)
                        {
                            //Add new attachment
                            Attachment att = new Attachment()
                            {
                                FileName = attachment.FileName,
                                ContentType = attachment.ContentType,
                                FileType = attachment.FileName.Split('.').Last(),
                                Content = attachment.Content
                            };

                            await _context.Attachment.AddAsync(att);
                            await _context.AnnouncementAttachments.AddAsync(new AnnouncementAttachments()
                            {
                                Attachment = att,
                                Announcement = dbEntry
                            });
                        }
                    }
                }
                else
                {
                    //remove all attachments
                    var attachments = dbEntry.AnnouncementAttachments.Select(x => x.Attachment);
                    _context.Attachment.RemoveRange(attachments);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id, string userId)
        {
            var dbEntry = _context.AuctionAnnouncement.Find(id);

            if (dbEntry != null)
            {
                SetDeleteStamp(dbEntry, userId);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AuctionAnnouncementModel> Get(int id)
        {
            var result = from a in _context.AuctionAnnouncement
                         join p in _context.PropertyType on a.PropertyType equals p.Code
                         where a.Id == id && !a.Deleted
                         select new AuctionAnnouncementModel()
                         {
                             Id = a.Id,
                             AuctionEndDate = DateTime.SpecifyKind(a.AuctionEndDate, DateTimeKind.Utc),
                             AuctionStartDate = DateTime.SpecifyKind(a.AuctionStartDate, DateTimeKind.Utc),
                             BidStep = a.BidStep,
                             Code = a.Code,
                             Description = a.Description,
                             Order = a.OrderNavigation.Code,
                             OrderName = a.OrderNavigation.Name,
                             PropertyType = p.Code,
                             PropertyTypeName = p.Name,
                             RegisterEndDate = DateTime.SpecifyKind(a.RegisterEndDate, DateTimeKind.Utc),
                             RegisterStartDate = DateTime.SpecifyKind(a.RegisterStartDate, DateTimeKind.Utc),
                             StartPrice = a.StartPrice,
                             Title = a.Title,
                             Items = from i in a.AuctionItem
                                     let title = (i.PropertyType == Shared.Enums.PropertyType.VEHICLE.ToString() 
                                        ? i.Vehicle.Model 
                                        : (i.PropertyType == Shared.Enums.PropertyType.AGRIFORMACHINERY.ToString()
                                            ? i.AgriculturalMachinary.Type 
                                            : (i.PropertyType == Shared.Enums.PropertyType.AIRCRAFT.ToString() 
                                                ?  i.Aircraft.ModelName 
                                                : (i.PropertyType == Shared.Enums.PropertyType.REALESTATE.ToString()
                                                    ? i.RealEstate.Type + " / " + i.RealEstate.Area + (i.RealEstate.PropertyConstructionTypeId != null ? " / " + i.RealEstate.PropertyConstructionTypeId : "") + (i.RealEstate.Identifier != null ? " / " + i.RealEstate.Identifier : "")
                                                    : i.OtherProperty.Identifier))))
                                     let description = (i.PropertyType == Shared.Enums.PropertyType.VEHICLE.ToString() 
                                        ? (i.Vehicle.VehicleType  + " " + i.Vehicle.Model + " " + i.Vehicle.Color )
                                        : (i.PropertyType == Shared.Enums.PropertyType.AGRIFORMACHINERY.ToString() 
                                            ? (i.AgriculturalMachinary.Type + " " + i.AgriculturalMachinary.RegistrationNumber) 
                                            : (i.PropertyType == Shared.Enums.PropertyType.AIRCRAFT.ToString() 
                                                ? (i.Aircraft.ModelName + " " + i.Aircraft.ProducerName + i.Aircraft.ProducerCountryName)
                                                : (i.PropertyType == Shared.Enums.PropertyType.REALESTATE.ToString()
                                                ? i.RealEstate.Description
                                                : i.OtherProperty.Description))))
                                     select new AuctionItemModel()
                                     {
                                         Id = i.Id,
                                         NRZId = i.Nrzid,
                                         PropertyType = i.PropertyTypeNavigation.Name,
                                         Title = title,
                                         Description = description
                                     },
                             Attachments = a.AnnouncementAttachments.Select(x => new Models.AttachmentModel()
                             {
                                 Id = x.AttachmentId,
                                 FileName = x.Attachment.FileName
                             })
                         };

            var announcement = await result.SingleOrDefaultAsync();
            return announcement;
        }

        public IQueryable<AuctionAnnouncementModel> GetAll()
        {
            var result = from a in _context.AuctionAnnouncement
                         join p in _context.PropertyType on a.PropertyType equals p.Code
                         where !a.Deleted
                         select new AuctionAnnouncementModel()
                         {
                             Id = a.Id,
                             AuctionEndDate = DateTime.SpecifyKind(a.AuctionEndDate, DateTimeKind.Utc),
                             AuctionStartDate = DateTime.SpecifyKind(a.AuctionStartDate, DateTimeKind.Utc),
                             BidStep = a.BidStep,
                             Code = a.Code,
                             Description = a.Description,
                             PropertyType = p.Name,
                             Order = a.OrderNavigation.Name,
                             RegistrationsCount = a.AuctionRegistration.Count(),
                             RegisterEndDate = DateTime.SpecifyKind(a.RegisterEndDate, DateTimeKind.Utc),
                             RegisterStartDate = DateTime.SpecifyKind(a.RegisterStartDate, DateTimeKind.Utc),
                             StartPrice = a.StartPrice,
                             Title = a.Title
                         };

            return result;
        }

        public IQueryable<AuctionAnnouncementModel> GetByUserId(string userId)
        {
            var result = from a in _context.AuctionAnnouncement
                         join p in _context.PropertyType on a.PropertyType equals p.Code
                         where a.CreatedBy == userId && !a.Deleted
                         select new AuctionAnnouncementModel()
                         {
                             Id = a.Id,
                             AuctionEndDate = DateTime.SpecifyKind(a.AuctionEndDate, DateTimeKind.Utc),
                             AuctionStartDate = DateTime.SpecifyKind(a.AuctionStartDate, DateTimeKind.Utc),
                             BidStep = a.BidStep,
                             Code = a.Code,
                             Description = a.Description,
                             PropertyType = p.Name,
                             Order = a.OrderNavigation.Name,
                             RegistrationsCount = a.AuctionRegistration.Count(),
                             RegisterEndDate = DateTime.SpecifyKind(a.RegisterEndDate, DateTimeKind.Utc),
                             RegisterStartDate = DateTime.SpecifyKind(a.RegisterStartDate, DateTimeKind.Utc),
                             StartPrice = a.StartPrice,
                             Title = a.Title
                         };

            return result;
        }
    }
}
