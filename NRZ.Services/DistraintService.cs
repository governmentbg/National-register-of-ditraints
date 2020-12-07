using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.Company;
using NRZ.Models.Distraint;
using NRZ.Models.Person;
using NRZ.Models.Property;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Services
{
    public class DistraintService : BaseService, IDistraintService
    {
        private readonly IPropertyService _propertyService;
        private readonly IPersonService _personService;
        private readonly ICompanyService _companyService;

        public DistraintService(NRZContext context,
            IPropertyService propertyService,
            IPersonService personService,
            ICompanyService companyService,
            IStringLocalizer<SharedResources> localizer = null)
            : base(context, localizer)
        {
            _propertyService = propertyService;
            _personService = personService;
            _companyService = companyService;
        }


        public async Task<DistraintCreateModel> AddAsync(DistraintCreateModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("DistraintCreateModel is null");
            }

            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var distraint = model.ToEntity();

                    if (model.IsNewProperty)
                    {
                        OtherPropertyModel newProperty = await _propertyService.AddOtherPropertyAsync(model.NewOtherProperty);
                        distraint.PropertyIdOtherProperty = newProperty?.Id;
                    }
                    else if (model.PropertyIdVehicle == -1 && !String.IsNullOrEmpty(model.VehicleProperty.RegistrationNumber))
                    {
                        Vehicle vehicle = await _propertyService.AddOrUpdateVehicleAsync(model.VehicleProperty);
                        distraint.PropertyIdVehicle = vehicle.Id;
                    }
                    else if (model.PropertyIdAircraft == -1 && !String.IsNullOrEmpty(model.AircraftProperty.MsnserialNumber))
                    {
                        Aircraft aircraft = await _propertyService.AddOrUpdateAircraftAsync(model.AircraftProperty);
                        distraint.PropertyIdAircraft = aircraft.Id;
                    }
                    else if (model.PropertyIdVessel == -1 && !String.IsNullOrEmpty(model.VesselProperty.RegistrationData?.RegistrationNumber))
                    {
                        Vessel vessel = await _propertyService.AddOrUpdateVesselAsync(model.VesselProperty);
                        distraint.PropertyIdVessel = vessel.Id;
                    }

                    if (distraint.IsInFavourOfPerson)
                    {
                        if (model.InFavourOfPerson != null && !String.IsNullOrWhiteSpace(model.InFavourOfPerson.Identifier))
                        {
                            RegixPersonModel person = await _personService.AddRegixPersonAsync(model.InFavourOfPerson);
                            distraint.InFavourOfPersonId = person.Id;
                            distraint.InFavourOfCompany = null;
                            distraint.InFavourOfCompanyId = null;
                        }
                        else
                        {
                            throw new Exception("In favour of person missing data!");
                        }
                    }
                    else
                    {
                        if (model.InFavourOfCompany != null && !String.IsNullOrWhiteSpace(model.InFavourOfCompany.Uic))
                        {
                            RegixCompanyModel company = await _companyService.AddRegixCompanyAsync(model.InFavourOfCompany);
                            distraint.InFavourOfCompanyId = company.Id;
                            distraint.InFavourOfPerson = null;
                            distraint.InFavourOfPersonId = null;
                        }
                        else
                        {
                            throw new Exception("In favour of company missing data!");
                        }
                        
                    }

                    if(distraint.IsDebtorPerson)
                    {
                        if (model.DebtorPerson != null && !String.IsNullOrWhiteSpace(model.DebtorPerson.Identifier))
                        {
                            RegixPersonModel person = await _personService.AddRegixPersonAsync(model.DebtorPerson);
                            distraint.DebtorPersonId = person.Id;
                            distraint.DebtorCompany = null;
                            distraint.DebtorCompanyId = null;
                        }
                        else
                        {
                            throw new Exception("Debtor person missing data!");
                        }
                    }
                    else
                    {
                        if (model.DebtorCompany != null && !String.IsNullOrWhiteSpace(model.DebtorCompany.Uic))
                        {
                            RegixCompanyModel company = await _companyService.AddRegixCompanyAsync(model.DebtorCompany);
                            distraint.DebtorCompanyId = company.Id;
                            distraint.DebtorPerson = null;
                            distraint.DebtorPersonId = null;
                        }
                        else
                        {
                            throw new Exception("Debtor company missing data!");
                        }
                    }
                    

                    await _context.Distraint.AddAsync(distraint);
                    await _context.SaveChangesAsync();

                    tran.Commit();

                    model.Id = distraint.Id;
                    return model;
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
            }


        }

        public async Task<DistraintEnforceModel> EnforceAsync(DistraintEnforceModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("DistraintEnforceModel is null");
            }

            var distraint = await _context.Distraint
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (distraint == null)
            {
                throw new Exception("Distraint to be enforced was not found");
            }

            if(distraint.StatusCode.ToUpper() != Shared.Enums.DistraintStatus.REGISTERED.ToString())
            {
                throw new Exception("Distraint is not in valid status to be enforced");
            }

            model.EnforcedAt = DateTime.SpecifyKind(DateTime.Now.ToUniversalTime(), DateTimeKind.Utc);
            
            distraint.StatusCode = Shared.Enums.DistraintStatus.ENFORCED.ToString();
            distraint.EnforcementDate = model.EnforcementDate.HasValue == true ? DateTime.SpecifyKind(model.EnforcementDate.Value.ToUniversalTime(), DateTimeKind.Utc) : DateTime.UtcNow;
            distraint.EnforcedAt = model.EnforcedAt;
            distraint.EnforcedBy = model.EnforcedBy;

            _context.Distraint.Update(distraint);
            await _context.SaveChangesAsync();

            return model;

        }

        public async Task<DistraintRevokeModel> RevokeAsync(DistraintRevokeModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("DistraintRevokeModel is null");
            }

            var distraint = await _context.Distraint
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (distraint == null)
            {
                throw new Exception("Distraint to be revoked was not found");
            }

            if (distraint.StatusCode.ToUpper() != Shared.Enums.DistraintStatus.ENFORCED.ToString())
            {
                throw new Exception("Distraint is not in valid status to be revoked");
            }

            model.RevokedAt = DateTime.SpecifyKind(DateTime.Now.ToUniversalTime(), DateTimeKind.Utc);

            distraint.StatusCode = Shared.Enums.DistraintStatus.REVOKED.ToString();
            distraint.RevocationDate = model.RevocationDate.HasValue == true ? DateTime.SpecifyKind(model.RevocationDate.Value.ToUniversalTime(), DateTimeKind.Utc) : DateTime.UtcNow;
            distraint.RevokedAt = model.RevokedAt;
            distraint.RevokedBy = model.RevokedBy;

            _context.Distraint.Update(distraint);
            await _context.SaveChangesAsync();

            return model;

        }

        public async Task<DistraintExemptModel> ExemptAsync(DistraintExemptModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("DistraintExemptModel is null");
            }

            var distraint = await _context.Distraint
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (distraint == null)
            {
                throw new Exception("Distraint to be exempted was not found");
            }

            if (distraint.StatusCode.ToUpper() != Shared.Enums.DistraintStatus.ENFORCED.ToString())
            {
                throw new Exception("Distraint is not in valid status to be exempted");
            }

            model.ExemptedAt = DateTime.SpecifyKind(DateTime.Now.ToUniversalTime(), DateTimeKind.Utc);

            distraint.StatusCode = Shared.Enums.DistraintStatus.EXEMPTED.ToString();
            distraint.ExemptionDate = model.ExemptionDate.HasValue == true ? DateTime.SpecifyKind(model.ExemptionDate.Value.ToUniversalTime(), DateTimeKind.Utc) : DateTime.UtcNow;
            distraint.ExemptedAt = model.ExemptedAt;
            distraint.ExemptedBy = model.ExemptedBy;

            _context.Distraint.Update(distraint);
            await _context.SaveChangesAsync();

            return model;

        }

        private IQueryable<Distraint> All()
        {
            var listQuery = _context.Distraint
                .Include(x => x.StatusCodeNavigation)
                .Include(x => x.PropertyTypeCodeNavigation)
                .Include(x => x.PropertyIdVehicleNavigation)
                .Include(x => x.PropertyIdVehicleNavigation).ThenInclude(x => x.VehicleOwner)
                .Include(x => x.PropertyIdVehicleNavigation).ThenInclude(x => x.VehicleUser)
                .Include(x => x.PropertyIdAircraftNavigation)
                .Include(x => x.PropertyIdAircraftNavigation).ThenInclude(x => x.AircraftDebt)
                .Include(x => x.PropertyIdAircraftNavigation).ThenInclude(x => x.AircraftRegistration)
                .Include(x => x.PropertyIdAircraftNavigation).ThenInclude(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOperatorPerson)
                .Include(x => x.PropertyIdAircraftNavigation).ThenInclude(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOperatorEntity)
                .Include(x => x.PropertyIdAircraftNavigation).ThenInclude(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOwnerPerson)
                .Include(x => x.PropertyIdAircraftNavigation).ThenInclude(x => x.AircraftRegistration).ThenInclude(x => x.AircraftRegistrationOwnerEntity)
                .Include(x => x.PropertyIdVesselNavigation)
                .Include(x => x.PropertyIdVesselNavigation).ThenInclude(x => x.VesselEngine)
                .Include(x => x.PropertyIdVesselNavigation).ThenInclude(x => x.VesselOwner)
                .Include(x => x.PropertyIdVesselNavigation).ThenInclude(x => x.VesselRegistrationData)
                .Include(x => x.PropertyIdVesselNavigation).ThenInclude(x => x.VesselRegistrationData).ThenInclude(x => x.StatusNavigation)
                .Include(x => x.PropertyIdAgriForMachineryNavigation)
                .Include(x => x.PropertyIdAgriForMachineryNavigation).ThenInclude(x => x.Owner)
                .Include(x => x.PropertyIdAgriForMachineryNavigation).ThenInclude(x => x.Company)
                .Include(x => x.PropertyIdOtherPropertyNavigation)
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.AspNetUserRoles)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.AspNetUserRoles).ThenInclude(x => x.Role)
                .Include(x => x.InFavourOfPerson)
                .Include(x => x.DebtorPerson)
                .Include(x => x.InFavourOfCompany)
                .Include(x => x.InFavourOfCompany).ThenInclude(x => x.StatusCodeNavigation)
                .Include(x => x.DebtorCompany)
                .Include(x => x.DebtorCompany).ThenInclude(x => x.StatusCodeNavigation)
                .OrderByDescending(x => x.Id);

            return listQuery;
        }

        public IQueryable<DistraintViewModel> GetAll()
        {
            var listQuery = All()
                .AsNoTracking()
                .Select(x => x.ToViewModel());

            return listQuery;
        }

        public IQueryable<DistraintViewModel> Search(string searchText)
        {
            var listQuery = All()
                .AsNoTracking()
                .Select(x => x.ToViewModel());

            if (!String.IsNullOrWhiteSpace(searchText))
            {
                long searchNumber;
                bool isNumber = long.TryParse(searchText, out searchNumber);

                listQuery = All()
                .Where(x => (isNumber == true && x.Id == searchNumber) ||
                            (isNumber == true && x.PropertyIdVehicle == searchNumber) ||
                            (isNumber == true && x.PropertyIdAircraft == searchNumber) ||
                            (isNumber == true && x.PropertyIdVessel == searchNumber) ||
                            (isNumber == true && x.PropertyIdAgriForMachinery == searchNumber) ||
                            (isNumber == true && x.PropertyIdRealEstate == searchNumber) ||
                            (isNumber == true && x.PropertyIdOtherProperty == searchNumber) ||
                            x.StatusCode.ToLower().Contains(searchText.ToLower()) ||
                            (x.StatusCodeNavigation != null && x.StatusCodeNavigation.Name.ToLower().Contains(searchText.ToLower())) ||
                            x.PropertyTypeCode.ToLower().Contains(searchText.ToLower()) ||
                            (x.CreatedByNavigation != null && x.CreatedByNavigation.UserName.ToLower().Contains(searchText.ToLower())) ||
                            (x.CreatedByNavigation != null && x.CreatedByNavigation.AspNetUserRoles.Any() && x.CreatedByNavigation.AspNetUserRoles.Any(a => a.Role.Name.ToLower() == searchText.ToLower())) ||
                            x.InFavourOf.ToLower().Contains(searchText.ToLower()) ||
                            (x.InFavourOfPerson != null && x.InFavourOfPerson.FirstName.ToLower().Contains(searchText.ToLower())) ||
                            (x.InFavourOfPerson != null && x.InFavourOfPerson.MiddleName.ToLower().Contains(searchText.ToLower())) ||
                            (x.InFavourOfPerson != null && x.InFavourOfPerson.LastName.ToLower().Contains(searchText.ToLower())) ||
                            (x.InFavourOfCompany != null && x.InFavourOfCompany.Name.ToLower().Contains(searchText.ToLower())) ||
                            (x.DebtorPerson != null && x.DebtorPerson.FirstName.ToLower().Contains(searchText.ToLower())) ||
                            (x.DebtorPerson != null && x.DebtorPerson.MiddleName.ToLower().Contains(searchText.ToLower())) ||
                            (x.DebtorPerson != null && x.DebtorPerson.LastName.ToLower().Contains(searchText.ToLower())) ||
                            (x.DebtorCompany != null && x.DebtorCompany.Name.ToLower().Contains(searchText.ToLower())) ||
                            x.SuitNumber.ToLower().Contains(searchText.ToLower()) ||
                            x.Debtor.ToLower().Contains(searchText.ToLower()) ||
                            (x.PropertyTypeCodeNavigation != null && x.PropertyTypeCodeNavigation.Name.ToLower().Contains(searchText.ToLower())) ||
                            (x.PropertyIdVehicleNavigation != null && x.PropertyIdVehicleNavigation.RegistrationNumber.ToLower().Contains(searchText.ToLower())) ||
                            (x.PropertyIdAircraftNavigation != null && x.PropertyIdAircraftNavigation.MsnserialNumber.ToLower().Contains(searchText.ToLower())) ||
                            (x.PropertyIdOtherPropertyNavigation != null && x.PropertyIdOtherPropertyNavigation.Identifier.ToLower().Contains(searchText.ToLower())) ||
                            (x.PropertyIdAgriForMachineryNavigation!= null && x.PropertyIdAgriForMachineryNavigation.RegistrationNumber.ToLower().Contains(searchText.ToLower())) ||
                            (x.PropertyIdVesselNavigation != null && x.PropertyIdVesselNavigation.VesselRegistrationData.Any() && x.PropertyIdVesselNavigation.VesselRegistrationData.FirstOrDefault().RegistrationNumber.ToLower().Contains(searchText.ToLower())) ||
                            x.Location.ToLower().Contains(searchText.ToLower()))
                .AsNoTracking()
                .Select(x => x.ToViewModel());
            }

            return listQuery;
        }

        public Task<DistraintViewModel> GetByIdAsync(int id)
        {
            var listQuery = All()
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => x.ToViewModel())
                .FirstOrDefaultAsync();

            return listQuery;
        }


    }
}
