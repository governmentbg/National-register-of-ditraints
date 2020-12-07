using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.Person;
using NRZ.Models.Settings;
using NRZ.RegiX.Client.ResponseModels;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IAddressService _addressService;
        private readonly IIntegrationService _integrationService;

        public PersonService(NRZContext context,
            IStringLocalizer<SharedResources> localizer,
            IAddressService addressService,
            IIntegrationService integrationService)
            : base(context, localizer)
        {
            _addressService = addressService;
            _integrationService = integrationService;
        }

        public async Task CreateAsync(PersonModel model, string userId = null)
        {
            NRZ.Data.Person person = model.ToPerson();

            if (model.UserId != null)
            {
                var aspNetUser = _context.AspNetUsers.Where(x => x.Id == model.UserId).FirstOrDefault();
                aspNetUser.Email = model.Email;
                aspNetUser.NormalizedEmail = model.Email.ToUpper();
            }
            SetCreateStamp(person, userId);

            await _context.Person.AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(PersonModel model, string userId)
        {
            NRZ.Data.Person person = await _context.Person.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == model.Id && !x.Deleted);

            if (person != null)
            {
                person.ToUpdate(model);

                if (model.Address != null)
                {
                    if (model.Address.Id.HasValue)
                    {
                        await _addressService.EditAsync(model.Address);
                    }
                    else
                    {
                        person.Address = model.Address.ToAddress();
                    }
                }
                else
                {
                    person.AddressId = null;
                }

                if (model.UserId != null)
                {
                    var aspNetUser = _context.AspNetUsers.Where(x => x.Id == model.UserId).FirstOrDefault();
                    aspNetUser.Email = model.Email;
                    aspNetUser.NormalizedEmail = model.Email.ToUpper();
                }

                SetUpdateStamp(person, userId);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int personId, string userId)
        {
            NRZ.Data.Person person = await _context.Person.FirstOrDefaultAsync(x => x.Id == personId && !x.Deleted);

            if (person != null)
            {
                SetDeleteStamp(person, userId);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PersonModel> GetPersonModelAsyncByUserId(string userId)
        {
            PersonModel person = await _context.Person.Include(x => x.Address)
                                                      .Where(x => x.UserId == userId)
                                                      .Select(x => x.ToModel())
                                                      .FirstOrDefaultAsync();
            return person;
        }


        public async Task<RegixPersonModel> AddRegixPersonAsync(RegixPersonModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("RegixPersonModel is null");
            }

            RegixPersonModel existing = await _context.RegixPerson
                .Where(x =>
                x.Identifier == model.Identifier &&
                String.Equals(x.FirstName, model.FirstName) &&
                String.Equals(x.MiddleName, model.MiddleName) &&
                String.Equals(x.LastName, model.LastName) &&
                x.DateOfBirth == model.DateOfBirth &&
                x.DateOfDeath == model.DateOfDeath)
                .Select(x => x.ToModel())
                .FirstOrDefaultAsync();

            if (existing != null)
                return existing;

            var person = model.ToEntity();

            await _context.RegixPerson.AddAsync(person);
            await _context.SaveChangesAsync();

            model.Id = person.Id;
            return model;
        }

        public async Task<Data.Person> GetByPersonIdentification(string IdType, string IdNumber)
        {
            Data.Person person = await _context.Person.SingleOrDefaultAsync(x => x.IdentificationNumber == IdNumber
                                                                         && x.IdentificationNumberType == IdType
                                                                         && !x.Deleted);

            return person;
        }

        public async Task<RegixPersonModel> GetPersonFromRegixAsync(string identifier)
        {
            PersonSearchResultModel result = await _integrationService.GetPersonFromRegiXAsync(identifier);
            RegixPersonModel person = GetPersonViewModelFromResponse(result, identifier);

            return person;
        }

        private RegixPersonModel GetPersonViewModelFromResponse(PersonSearchResultModel result, string searchIdentifier)
        {
            if (result == null || result.ResponseObject == null)
                return null;

            ValidPersonResponse personResponse = result.ResponseObject as ValidPersonResponse;
            if (personResponse == null)
                throw new Exception("Could not convert response to ValidPersonResponse");

            RegixPersonModel model = personResponse.ToViewModel();
            model.RequestId = result.RequestId;
            model.Identifier = searchIdentifier;

            return model;
        }
    }
}
