using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.Company;
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
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly IIntegrationService _integrationService;

        public CompanyService(NRZContext context,
            IStringLocalizer<SharedResources> localizer,
            IIntegrationService integrationService)
            : base(context, localizer)
        {
            _integrationService = integrationService;
        }


        public async Task<RegixCompanyModel> AddRegixCompanyAsync(RegixCompanyModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("RegixCompanyModel is null");
            }

            RegixCompanyModel existing = await _context.RegixCompany
                .Where(x =>
                x.Uic == model.Uic &&
                String.Equals(x.Name, model.Name) &&
                String.Equals(x.LegalFormAbbr, model.LegalFormAbbr) &&
                String.Equals(x.LegalFormName, model.LegalFormName))
                .Select(x => x.ToModel())
                .FirstOrDefaultAsync();

            if (existing != null)
                return existing;

            var company = model.ToEntity();

            await _context.RegixCompany.AddAsync(company);
            await _context.SaveChangesAsync();

            model.Id = company.Id;
            return model;
        }

        public async Task<RegixCompanyModel> GetCompanyFromRegixAsync(string identifier)
        {
            CompanySearchResultModel result = await _integrationService.GetCompanyFromRegiXAsync(identifier);
            RegixCompanyModel company = GetCompanyViewModelFromResponse(result, identifier);
            company = await SetCompanyStatusInModel(company);

            return company;
        }

        private RegixCompanyModel GetCompanyViewModelFromResponse(CompanySearchResultModel result, string searchIdentifier)
        {
            if (result == null || result.ResponseObject == null)
                return null;

            ValidUICResponse companyResponse = result.ResponseObject as ValidUICResponse;
            if (companyResponse == null)
                throw new Exception("Could not convert response to ValidUICResponse");

            RegixCompanyModel model = companyResponse.ToViewModel();
            model.RequestId = result.RequestId;
            model.Uic = searchIdentifier;

            return model;
        }

        private async Task<RegixCompanyModel> SetCompanyStatusInModel(RegixCompanyModel company)
        {
            RegixCompanyStatus status = await _context.RegixCompanyStatus
                .Where(x => x.Code == company.StatusCode)
                .FirstOrDefaultAsync();

            if (status != null)
            {
                company.StatusName = status.Name;
                company.StatusNameEn = status.NameEn;
            }

            return company;
        }



    }
}
