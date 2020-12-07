using NRZ.Data;
using NRZ.Models.Property;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<OtherPropertyModel> AddOtherPropertyAsync(OtherPropertyModel model);
        Task<Vehicle> AddOrUpdateVehicleAsync(VehicleViewModel model);
        Task<Data.Aircraft> AddOrUpdateAircraftAsync(AircraftViewModel model);
        Task<Data.Vessel> AddOrUpdateVesselAsync(VesselViewModel model);
        //Task<List<BaseProperty>> SearchInDBAsync(Shared.Enums.PropertyType propertyType, PropertySearchRequestModel model);
        Task<List<OtherPropertyModel>> SearchOtherPropertyInDBAsync(PropertySearchRequestModel model);
        Task<List<VehicleViewModel>> SearchVehicleInDBAsync(PropertySearchRequestModel model);
        Task<List<AircraftViewModel>> SearchAircraftInDBAsync(PropertySearchRequestModel model);
        Task<List<VesselViewModel>> SearchVesselInDBAsync(PropertySearchRequestModel model);
        Task<List<AgriculturalMachineryViewModel>> SearchAgriculturalMachineryInDBAsync(PropertySearchRequestModel model);
        Task<List<object>> SearchInDataAdministratorAsync(Shared.Enums.PropertyType propertyType, PropertySearchRequestModel model);
        Task<PropertyModel> AddAsync(PropertyModel model, string userId);
        Task<PropertyModel> UpdateAsunc(PropertyModel model, string userId);
    }
}
