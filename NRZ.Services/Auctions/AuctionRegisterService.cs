using NRZ.Data;
using NRZ.Models.Auction;
using NRZ.Services.Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NRZ.Data.Extensions;
using NRZ.Models.Company;
using NRZ.Models.Person;
using NRZ.Shared.Enums;

namespace NRZ.Services.Auctions
{
    public class AuctionRegisterService : BaseService, IAuctionRegisterService
    {
        public AuctionRegisterService(NRZContext context)
        : base(context, null)
        { }

        public AuctionRegisterModel GetById(int id)
        {
            var result = _context.AuctionRegistration
                                 .Include(x => x.Announcement)
                                 .Include(x => x.Participant)
                                 .ThenInclude(x => x.PersonUser)
                                 .ThenInclude(x => x.Address)
                                 .Include(x => x.RepresentedPerson)
                                 .Include(x => x.RepresentedCompany)
                                 .Where(a => a.Id == id && !a.Deleted);

            return ParseDbModel(result).SingleOrDefault();
        }

        public IQueryable<AnnouncemetRegisterGridModel> GetAllByUser(string userId)
        {
            var result = _context.AuctionRegistration
                                 .Include(x => x.Announcement)
                                 .Include(x => x.Participant)
                                 .ThenInclude(x => x.PersonUser)
                                 .Include(x => x.RepresentedPerson)
                                 .Include(x => x.RepresentedCompany)
                                 .Where(a => a.ParticipantId == userId && !a.Deleted);

            return ParseDbModelForGrid(result);
        }

        public IQueryable<AnnouncemetRegisterGridModel> GetAllByCreator(string userId)
        {
            var result = _context.AuctionRegistration
                                 .Include(x => x.Announcement)
                                 .Include(x => x.Participant)
                                 .ThenInclude(x => x.PersonUser)
                                 .Include(x => x.RepresentedPerson)
                                 .Include(x => x.RepresentedCompany)
                                 .Where(a => a.Announcement.CreatedBy == userId && !a.Deleted);

            return ParseDbModelForGrid(result);
        }

        public IQueryable<AnnouncemetRegisterGridModel> GetByAnnouncementAndUser(int id, string userId)
        {
            var result = _context.AuctionRegistration
                                 .Include(x => x.Announcement)
                                 .Include(x => x.Participant)
                                 .ThenInclude(x => x.PersonUser)
                                 .Include(x => x.RepresentedPerson)
                                 .Include(x => x.RepresentedCompany)
                                 .Where(a => a.AnnouncementId == id && a.Announcement.CreatedBy == userId && !a.Deleted);

            return ParseDbModelForGrid(result);
        }

        public IQueryable<AnnouncemetRegisterGridModel> GetByAnnouncementId(int announcementId)
        {
            var result = _context.AuctionRegistration
                                 .Include(x => x.Announcement)
                                 .Include(x => x.Participant)
                                 .ThenInclude(x => x.PersonUser)
                                 .Include(x => x.RepresentedPerson)
                                 .Include(x => x.RepresentedCompany)
                                 .Where(a => a.AnnouncementId == announcementId && !a.Deleted);

            return ParseDbModelForGrid(result);
        }

        public async Task CreateAsync(AuctionRegisterModel model, string userId)
        {
            AuctionRegistration ar = new AuctionRegistration()
            {
                AnnouncementId = model.AnnouncementId,
                AppliedByCourtEnforcer = model.AppliedByCourtEnforcer,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
                Deleted = false,
                IsApproved = model.IsApproved,
                IsOwner = model.IsOwner,
                IsOwnerSpouse = model.IsOwnerSpouse,
                RepresentationType = model.RepresentationType,
                ResultDeliveryType = model.ResultDeliveryType
            };

            ar.UniqueNumber = GenerateUniqueNumber();
            _context.AuctionRegistration.Add(ar);

            //Add participant
            if (model.AppliedByCourtEnforcer && string.IsNullOrWhiteSpace(model.ParticipantId))
            {
                var participantPerson = model.Participant.ToPerson();
                _context.Person.Add(participantPerson);
                ar.ParticipantId = participantPerson.UserId;
            }
            else
            {
                ar.ParticipantId = userId;
            }

            //Represented Person/Company
            if (model.RepresentationType != AuctionRepresentationTypes.PERSONAL.ToString())
            {
                if (model.RepresentedCompany != null)
                {
                    await MapCompany(ar, model.RepresentedCompany);
                }
                else
                {
                    await MapPerson(ar, model.RepresentedPerson);
                }
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
                    _context.AuctionRegistrationAttachment.Add(new AuctionRegistrationAttachment() { Attachment = att, AuctionRegistration = ar });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var item = await _context.AuctionRegistration.FindAsync(id);

            if (item != null && !item.Deleted)
            {
                SetDeleteStamp(item, userId);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ProccessAsync(ApprovalModel model, string userId)
        {
            var item = await _context.AuctionRegistration.SingleOrDefaultAsync(x => x.Id == model.EntityId && !x.Deleted);

            if (item == null)
            {
                throw new NullReferenceException("Item not found");
            }

            item.IsApproved = model.Approved;
            item.ProcessedBy = userId;
            item.ProcessedOn = DateTime.UtcNow;

            if (!model.Approved)
            {
                item.RejectReason = model.RejectReason;
            }

            await _context.SaveChangesAsync();
        }

        private IQueryable<AuctionRegisterModel> ParseDbModel(IQueryable<AuctionRegistration> data)
        {
            return from a in data
                   select new AuctionRegisterModel()
                   {
                       AnnouncementCreatedBy = a.Announcement != null ? a.Announcement.CreatedBy : "",
                       AnnouncementId = a.AnnouncementId,
                       AppliedByCourtEnforcer = a.AppliedByCourtEnforcer,
                       CreatedBy = a.CreatedBy,
                       CreatedOn = a.CreatedOn,
                       Deleted = a.Deleted,
                       DeletedBy = a.DeletedBy,
                       DeletedOn = a.DeletedOn,
                       Id = a.Id,
                       IsApproved = a.IsApproved,
                       IsOwner = a.IsOwner,
                       IsOwnerSpouse = a.IsOwnerSpouse,
                       Participant = a.Participant.PersonUser.FirstOrDefault().ToModel(),
                       ParticipantId = a.ParticipantId,
                       ProcessedBy = a.ProcessedBy,
                       ProcessedOn = a.ProcessedOn,
                       RejectReason = a.RejectReason,
                       RepresentationType = a.RepresentationType,
                       RepresentationTypeName = a.RepresentationTypeNavigation.Name,
                       RepresentedCompany = a.RepresentedCompany.ToModel(),
                       RepresentedCompanyId = a.RepresentedCompanyId,
                       RepresentedPerson = a.RepresentedPerson.ToModel(),
                       RepresentedPersonId = a.RepresentedPersonId,
                       RepresentedUserId = a.RepresentedUserId,
                       ResultDeliveryType = a.ResultDeliveryType,
                       ResultDeliveryTypeName = a.ResultDeliveryTypeNavigation.Name,
                       UniqueNumber = a.UniqueNumber,
                       Attachments = a.AuctionRegistrationAttachment.Select(x => new Models.AttachmentModel()
                       {
                           FileName = x.Attachment.FileName,
                           Id = x.AttachmentId,
                           Type = x.Attachment.FileType
                       })
                   };
        }

        private IQueryable<AnnouncemetRegisterGridModel> ParseDbModelForGrid(IQueryable<AuctionRegistration> data)
        {
            return from a in data
                   select new AnnouncemetRegisterGridModel()
                   {
                       AnnouncementId = a.AnnouncementId,
                       AnnouncementTitle = a.Announcement.Title,
                       AnnouncementCode = a.Announcement.Code,
                       Id = a.Id,
                       IsApproved = a.IsApproved,
                       Participant = a.Participant.PersonUser.FirstOrDefault().ToModel(),
                       ProcessedBy = a.ProcessedBy,
                       ProcessedOn = a.ProcessedOn,
                       RejectReason = a.RejectReason,
                       UniqueNumber = a.UniqueNumber
                   };
        }

        private string GenerateUniqueNumber()
        {
            string result = "";
            // year + 5 digits number
            string lastNumber = _context.AuctionRegistration.OrderBy(x => x.Id).LastOrDefault()?.UniqueNumber;

            if (string.IsNullOrWhiteSpace(lastNumber))
            {
                result = $"{DateTime.Now.Year}{1:D5}";
            }
            else
            {
                var year = lastNumber.Substring(0, 4);
                var number = int.Parse(lastNumber.Substring(4));

                if (year == DateTime.Now.Year.ToString())
                {
                    number++;
                }
                else
                {
                    number = 1;
                    year = DateTime.Now.Year.ToString();
                }

                result = $"{year}{number:D5}";
            }

            return result;
        }

        private async Task MapCompany(AuctionRegistration ar, CompanyModel model)
        {
            var dbCompany = await _context.Company.FirstOrDefaultAsync(x => x.Eik.ToLower() == model.EIK.ToLower());
            if (dbCompany != null)
            {
                ar.RepresentedCompanyId = dbCompany.Id;
            }
            else
            {
                Company company = new Company()
                {
                    Address = model.Address.ToAddress(),
                    Eik = model.EIK,
                    Name = model.Name
                };

                ar.RepresentedCompany = company;
                _context.Company.Add(company);
            }
        }

        private async Task MapPerson(AuctionRegistration ar, PersonModel model)
        {
            var dbPerson = await _context.Person.FirstOrDefaultAsync(x => x.IdentificationNumberType == model.IdentificationType
                                                                       && x.IdentificationNumber == model.IdentificationNumber);
            if (dbPerson != null)
            {
                ar.RepresentedPersonId = dbPerson.Id;
            }
            else
            {
                Person person = model.ToPerson();
                ar.RepresentedPerson = person;
                _context.Person.Add(person);
            }
        }
    }
}
