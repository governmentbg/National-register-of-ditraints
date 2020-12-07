using NRZ.Models.Nomenclatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface INomenclatureService
    {
        Task<List<PropertyTypeModel>> GetAllPropertyTypesAsync();
        IQueryable<NomenclatureBaseModel> UserTypes();
        Task<NomenclatureBaseModel> UserTypeAsync(string code);
        IQueryable<NomenclatureBaseModel> UserRegisterTypes();
        IQueryable<NomenclatureBaseModel> UserTypesAll();
        IQueryable<NomenclatureBaseModel> AuctionOrderTypes();
        IQueryable<NomenclatureBaseModel> Regions(string search = null);
        IQueryable<NomenclatureBaseModel> Municipalities(int? regionId = null, string search = null);
        IQueryable<NomenclatureBaseModel> Cities(int? municipalityId = null, string search = null);
        Task<List<RegixCompanyStatusModel>> GetRegixCompanyStatuses();
        Task<List<PropertyTypeModel>> GetPropertyTypesForDistraintAsync();
        List<NomenclatureBaseModel> GetTimestampAuthorities();
        Task<List<NomenclatureBaseModel>> GetAssetTypesAsync();
        Task<List<NomenclatureBaseModel>> GetPropertyConstructionTypeAsync();
        Task<List<NomenclatureBaseModel>> GetPropertyExtrasAsync();
        Task<List<NomenclatureBaseModel>> GetRealEstateTypesAsync();
        Task<List<NomenclatureBaseModel>> RequesterTypes();
    }
}
