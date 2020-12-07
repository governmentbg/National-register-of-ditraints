using NRZ.Models.EPayment;
using NRZ.Models.Role;
using NRZ.Models.SeizedPropertyAvailability;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface ISeizedPropertyAvailabilityRequestService
    {
        Task<(bool hasToPay, PaymentRequestSendResultModel paymentRequestModel, IEnumerable<SeizedPropertyAvailabilityResultModel> searchModels)> Search(SeizedPropertyAvailabilityRequestModel model, string currentUserId);
        Task<IEnumerable<SeizedPropertyAvailabilityListItemModel>> GetAll();
        Task<SeizedAvailabilityRequestReportModel> GetReport(int distraintId);
        Task<AircraftDetailsModel> GetAircrafInfo(long distraintId);
        Task<VehicleDetailsModel> GetVehicleInfo(int distraintId);
        Task<VesselDetailsModel> GetVesselInfo(int distraintId);
        Task<RealEstateDetailsModel> GetRealEstateInfo(int distraintId);
        Task<AgriformachineryIDetailsModel> GetAgriformachineryInfo(int distraintId);
        Task<OtherDetailsModel> GetOtherInfo(int distraintId);
        Task<List<SeizedPropertyAvailabilityResultModel>> GetPaidReport(string paidGuid);
    }
}
