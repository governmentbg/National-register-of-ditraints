using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Models.Nomenclatures;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using NRZ.Ts.Client.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Services
{
    public class NomenclatureService : BaseService, INomenclatureService
    {
        public NomenclatureService(NRZContext context,
            IStringLocalizer<SharedResources> localizer = null)
            : base(context, localizer)
        {
        }

        public async Task<List<PropertyTypeModel>> GetAllPropertyTypesAsync()
        {
            var types = await _context.PropertyType
                .Select(x => new PropertyTypeModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    NameEn = x.NameEn,
                    Deactivated = x.Deactivated,
                    UseForDistraint = x.UseForDistraint
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return types;
        }

        public async Task<List<PropertyTypeModel>> GetPropertyTypesForDistraintAsync()
        {
            var types = await _context.PropertyType
                .Where(x => x.UseForDistraint == true)
                .Select(x => new PropertyTypeModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    NameEn = x.NameEn,
                    Deactivated = x.Deactivated,
                    UseForDistraint = x.UseForDistraint
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return types;
        }

        public async Task<List<RegixCompanyStatusModel>> GetRegixCompanyStatuses()
        {
            var statuses = await _context.RegixCompanyStatus
                .Select(x => new RegixCompanyStatusModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    NameEn = x.NameEn,
                    Deactivated = x.Deactivated
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return statuses;
        }

        public IQueryable<NomenclatureBaseModel> UserTypes()
        {
            var types = _context.UserType
                .Where(x => !x.Deactivated)
                .OrderBy(x => x.Sort)
                .Select(x => new NomenclatureBaseModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    NameEn = x.NameEn
                });

            return types;
        }

        public IQueryable<NomenclatureBaseModel> UserTypesAll()
        {
            var types = _context.UserType
                .Select(x => new NomenclatureBaseModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    NameEn = x.NameEn,
                    Deactivated = x.Deactivated,
                    Sort = x.Sort
                });

            return types;
        }

        public async Task<NomenclatureBaseModel> UserTypeAsync(string code)
        {
            return await UserTypes().FirstOrDefaultAsync(x => x.Code == code);
        }

        public IQueryable<NomenclatureBaseModel> UserRegisterTypes()
        {
            var result = _context.UserRegisterType
               .Where(x => !x.Disabled && x.IsPublic.Value)
               .OrderBy(x => x.Sort)
               .Select(x => new NomenclatureBaseModel
               {
                   Code = x.Code,
                   Name = x.Name,
                   NameEn = x.NameEn,
               });

            return result;
        }

        public IQueryable<NomenclatureBaseModel> AuctionOrderTypes()
        {
            var result = _context.AuctionOrder
               .Where(x => !x.Deleted)
               .OrderBy(x => x.Name)
               .Select(x => new NomenclatureBaseModel
               {
                   Code = x.Code,
                   Name = x.Name
               });

            return result;
        }

        public IQueryable<NomenclatureBaseModel> Regions(string search = null)
        {
            var result = _context.Regions
               .Where(x => string.IsNullOrEmpty(search) || x.Name.ToLower().Contains(search.ToLower()))
               .OrderBy(x => x.Name)
               .Select(x => new NomenclatureBaseModel
               {
                   Id = x.Id,
                   Name = x.Name
               });

            return result;
        }

        public IQueryable<NomenclatureBaseModel> Municipalities(int? regionId = null, string search = null)
        {
            var result = _context.Municipalities
               .Where(x => (!regionId.HasValue || x.RegionId == regionId)
                        && (string.IsNullOrEmpty(search) || x.Name.ToLower().Contains(search.ToLower())))
               .OrderBy(x => x.Name)
               .Select(x => new NomenclatureBaseModel
               {
                   Id = x.Id,
                   Code = x.Code,
                   Name = x.Name
               });

            return result;
        }

        public IQueryable<NomenclatureBaseModel> Cities(int? municipalityId = null, string search = null)
        {
            var result = _context.Cities
               .Where(x => (!municipalityId.HasValue || x.MunicipalityId == municipalityId)
                        && (string.IsNullOrEmpty(search) || x.Name.ToLower().Contains(search.ToLower())))
               .OrderBy(x => x.Name)
               .Select(x => new NomenclatureBaseModel
               {
                   Id = x.Id,
                   Name = x.Prefix + " " + x.Name
               });

            return result;
        }

        public List<NomenclatureBaseModel> GetTimestampAuthorities()
        {
            return new List<NomenclatureBaseModel>() {
                new NomenclatureBaseModel { Code = Authority.Evotrust.ToString(), Name = Authority.Evotrust.ToString() },
                new NomenclatureBaseModel { Code = Authority.Infonotary.ToString(), Name = Authority.Infonotary.ToString() }
            };
        }

        public Task<List<NomenclatureBaseModel>> GetAssetTypesAsync()
        {
            return _context.AssetType
                .Where(x => !x.Deactivated)
                .Select(x => new NomenclatureBaseModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    NameEn = x.NameEn,
                    Deactivated = x.Deactivated,
                    Sort = (short)x.Sort
                })
                .OrderBy(x => x.Sort)
                .ToListAsync();
        }

        public Task<List<NomenclatureBaseModel>> GetPropertyConstructionTypeAsync()
        {
            return _context.PropertyConstructionType
                .Where(x => !x.Deactivated)
                .Select(x => new NomenclatureBaseModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    NameEn = x.NameEn,
                    Deactivated = x.Deactivated,
                    Sort = (short)x.Sort
                })
                .OrderBy(x => x.Sort)
                .ToListAsync();
        }

        public Task<List<NomenclatureBaseModel>> GetPropertyExtrasAsync()
        {
            return _context.PropertyExtra
                .Where(x => !x.Deactivated)
                .Select(x => new NomenclatureBaseModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    NameEn = x.NameEn,
                    Deactivated = x.Deactivated,
                    Sort = (short)x.Sort
                })
                .OrderBy(x => x.Sort)
                .ToListAsync();
        }

        public Task<List<NomenclatureBaseModel>> GetRealEstateTypesAsync()
        {
            return _context.RealEstateType
                .Where(x => !x.Deactivated)
                .Select(x => new NomenclatureBaseModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    NameEn = x.NameEn,
                    Deactivated = x.Deactivated,
                    Sort = (short)x.Sort
                })
                .OrderBy(x => x.Sort)
                .ToListAsync();
        }

        public async Task<List<NomenclatureBaseModel>> RequesterTypes()
        {
            var types = await _context.RequesterType
               .Where(x => !x.Deactivated)
               .OrderBy(x => x.Sort)
               .Select(x => new NomenclatureBaseModel
               {
                   Code = x.Code,
                   Name = x.Name,
                   NameEn = x.NameEn
               }).ToListAsync();

            return types;
        }
    }
}
