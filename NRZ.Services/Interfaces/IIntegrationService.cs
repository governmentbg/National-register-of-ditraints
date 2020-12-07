using NRZ.Models.Company;
using NRZ.Models.CSI;
using NRZ.Models.EPayment;
using NRZ.Models.Person;
using NRZ.Models.Property;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IIntegrationService
    {
        Task<PropertySearchResultModel> SearchPropertyAsync(Shared.Enums.PropertyType propertyType, PropertySearchRequestModel model);
        Task<PropertySearchResultModel> TestConnectionToRegiXVehicle();
        Task<PropertySearchResultModel> TestConnectionToRegiXVessel();
        Task<PropertySearchResultModel> TestConnectionToRegiXAircraft();
        Task<CSIModel> GetCSIByNumberAndDate(string csiNumber, DateTime date);
        Task<CSIModel> GetCSIByNumber(string csiNumber, bool helpers = false);
        Task<PersonSearchResultModel> GetPersonFromRegiXAsync(string identifier);
        Task<CompanySearchResultModel> GetCompanyFromRegiXAsync(string identifier);
        
    }
}
