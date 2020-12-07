using NRZ.Models.Company;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Data.Extensions
{
    public static class CompanyExtensions
    {
        public static RegixCompanyModel ToModel(this RegixCompany entity)
        {
            if (entity == null)
            {
                return null;
            }

            RegixCompanyModel model = new RegixCompanyModel
            {
                Id = entity.Id,
                Uic = entity.Uic,
                Name = entity.Name,
                LegalFormAbbr = entity.LegalFormAbbr,
                LegalFormName = entity.LegalFormName,
                StatusCode = entity.StatusCode,
                StatusName = entity.StatusCodeNavigation?.Name,
                StatusNameEn = entity.StatusCodeNavigation?.NameEn,
                RequestId = entity.RequestId,
                UpdatedAt = entity.UpdatedAt.HasValue == true ? DateTime.SpecifyKind(entity.UpdatedAt.Value, DateTimeKind.Utc) : default(DateTime?),
            };

            return model;
        }

        public static RegixCompany ToEntity(this RegixCompanyModel model)
        {
            if (model == null)
            {
                return null;
            }

            RegixCompany entity = new RegixCompany
            {
                Uic = model.Uic,
                Name = model.Name,
                LegalFormAbbr = model.LegalFormAbbr,
                LegalFormName = model.LegalFormName,
                StatusCode = model.StatusCode,
                RequestId = model.RequestId,
                UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
            };

            return entity;
        }

        public static CompanyModel ToModel(this Company entity)
        {
            if (entity == null)
            {
                return null;
            }

            CompanyModel model = new CompanyModel
            {
                Id = entity.Id,
                Name = entity.Name,
                EIK = entity.Eik,
                CompanyCaseNumber = entity.CompanyCaseNumber,
                Address = entity.Address.ToModel()
            };

            return model;
        }
    }
}
