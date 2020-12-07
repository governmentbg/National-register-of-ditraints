using NRZ.Data;
using NRZ.Models.AgriculturalMachinery;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IAgriculturalMachineryService
    {
        Task CreateAsync(AgriculturalMachineryModel creationModel, string userId);

        AgriculturalMachineryResponseModel GetAll(AgriculturalMachineryListingRequestModel model);

        Task<AgriculturalMachineryWithOwnerModel> GetMachineByIdAsync(int id);

        Task EditMachineAsync(AgriculturalMachineryWithOwnerNameModel model, string userId);


        Task DeleteAsync(int id, string userId);
    }
}
