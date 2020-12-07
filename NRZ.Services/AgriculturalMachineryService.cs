using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Models.AgriculturalMachinery;
using NRZ.Models.Company;
using NRZ.Models.Person;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using NRZ.Services.Extensions;

namespace NRZ.Services
{
    public class AgriculturalMachineryService : BaseService, IAgriculturalMachineryService
    {

        public AgriculturalMachineryService(NRZContext context, IStringLocalizer<SharedResources> localizer) : base(context, localizer)
        {
        }

        public async Task CreateAsync(AgriculturalMachineryModel createModel, string userId)
        {
            Person person = null;
            Company company = null;
            Address address = null;

            if (createModel.Person != null)
            {
                person = new Person
                {
                    FirstName = createModel.Person.FirstName,
                    MiddleName = createModel.Person.MiddleName,
                    LastName = createModel.Person.LastName,
                    IdentificationNumber = createModel.Person.IdentificationNumber,
                    IdentificationNumberType = createModel.Person.IdentificationType,
                    Phone = createModel.Person.Phone,
                    Email = createModel.Person.Email,
                    UserId = userId

                };

                SetCreateStamp(person, userId);

                address = new Address
                {
                    RegionId = createModel.Person.Address.RegionId,
                    MunicipalityId = createModel.Person.Address.MunicipalityId,
                    CityId = createModel.Person.Address.CityId,
                    StreetAddress = createModel.Person.Address.StreetAddress
                };
            }
            else if (createModel.Person == null)
            {
                company = new Company
                {
                    Name = createModel.Company.Name,
                    Eik = createModel.Company.EIK
                };

                address = new Address
                {
                    RegionId = createModel.Company.Address.RegionId,
                    MunicipalityId = createModel.Company.Address.MunicipalityId,
                    CityId = createModel.Company.Address.CityId,
                    StreetAddress = createModel.Company.Address.StreetAddress
                };

            }


            var machine = new AgriculturalMachinery
            {
                RegistrationNumber = createModel.RegistrationNumber,
                FrameNumber = createModel.FrameNumber,
                Type = createModel.Type
            };

            if (person != null)
            {
                machine.Owner = person;
                person.Address = address;
                address.Person.Add(person);
                person.AgriculturalMachinery.Add(machine);
                await _context.Address.AddAsync(address);
                await _context.Person.AddAsync(person);
            }
            else if (person == null)
            {
                machine.Company = company;
                company.Address = address;
                address.Company.Add(company);
                company.AgriculturalMachinery.Add(machine);
                await _context.Address.AddAsync(address);
                await _context.Company.AddAsync(company);
            }

            SetCreateStamp(machine, userId);
            await _context.AgriculturalMachinery.AddAsync(machine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var machine = _context.AgriculturalMachinery.Where(ag => ag.Id == id && !ag.Deleted).FirstOrDefault();

            if (machine != null)
            {
                SetDeleteStamp(machine, userId);
                await _context.SaveChangesAsync();
            }
        }

        public AgriculturalMachineryResponseModel GetAll(AgriculturalMachineryListingRequestModel model)
        {
            AgriculturalMachineryResponseModel result = new AgriculturalMachineryResponseModel();

            var agMachineries = _context.AgriculturalMachinery
                .Include(x => x.Company)
                .Include(x => x.Owner)
                .Where(x => x.Deleted == false)
                .Select(am => new AgriculturalMachineryWithOwnerNameModel
                {
                    Id = am.Id,
                    RegistrationNumber = am.RegistrationNumber,
                    FrameNumber = am.FrameNumber,
                    Type = am.Type,
                    Owner = am.CompanyId != null
                ?
                am.Company.Name
                :
                am.Owner.FirstName + " " + am.Owner.LastName

                })
                .OrderByDescending(x => x.Id)
                .AsQueryable();

            var searchString = model.SearchString == null || model.SearchString == "" ? model.SearchString : model.SearchString.ToLower();

            if (!String.IsNullOrEmpty(searchString))
            {
                agMachineries = agMachineries
                .Where(m =>
                     m.RegistrationNumber.ToLower().Contains(searchString)
                  || m.FrameNumber.ToLower().Contains(searchString)
                  || m.Type.ToLower().Contains(searchString)
                  || m.Owner.ToLower().Contains(searchString));
            }

            result.Total = agMachineries.Count();

            if (!String.IsNullOrEmpty(model.SortBy))
            {
                agMachineries = OrderByStringWithReflection.OrderBy(agMachineries, model.SortBy, model.SortDesc);
            }

            agMachineries = agMachineries.Skip((model.Page - 1) * model.ItemsPerPage)
                 .Take(model.ItemsPerPage);

            result.Items = agMachineries;

            return result;
        }

        public async Task<AgriculturalMachineryWithOwnerModel> GetMachineByIdAsync(int id)
        {
            var a = await _context.AgriculturalMachinery
                .Where(am => am.Id == id && !am.Deleted)
                .Include(x => x.Owner)
                .Include(x => x.Company)
                .Where(x => x.Deleted == false)
                .Select(am => new AgriculturalMachineryWithOwnerModel
                {
                    Id = am.Id,
                    RegistrationNumber = am.RegistrationNumber,
                    FrameNumber = am.FrameNumber,
                    Type = am.Type,
                    Owner = am.OwnerId != null ? new PersonModel
                    {
                        FirstName = am.Owner.FirstName,
                        MiddleName = am.Owner.MiddleName,
                        LastName = am.Owner.LastName,
                        Phone = am.Owner.Phone,
                        Email = am.Owner.Email,
                        IdentificationType = am.Owner.IdentificationNumberType,
                        IdentificationNumber = am.Owner.IdentificationNumber
                    } : null,
                    Company = am.CompanyId != null ? new CompanyModel
                    {
                        Name = am.Company.Name,
                        EIK = am.Company.Eik,
                        CompanyCaseNumber = am.Company.CompanyCaseNumber
                    } : null
                })
                .FirstOrDefaultAsync();


            return a;
        }

        public async Task EditMachineAsync(AgriculturalMachineryWithOwnerNameModel model, string userId)
        {
            var machine = _context.AgriculturalMachinery.Where(am => am.Id == model.Id && !am.Deleted).FirstOrDefault();

            if (machine != null)
            {
                machine.RegistrationNumber = model.RegistrationNumber;
                machine.FrameNumber = model.FrameNumber;
                machine.Type = model.Type;

                SetUpdateStamp(machine, userId);
                await _context.SaveChangesAsync();
            }
        }

    }
}
