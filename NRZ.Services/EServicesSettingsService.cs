using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.EPayment;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services
{
    public class EServicesSettingsService : BaseService, IEServicesSettingsService
    {
        public EServicesSettingsService(NRZContext context,
            IStringLocalizer<SharedResources> localizer)
            : base(context, localizer)
        {
        }

        public async Task<EServicesSettingsModel> GetEServiceSettingsAsync()
        {
            EServicesSettingsModel settings = await _context.EservicesSettings
                                                      .Select(x => x.ToModel())
                                                      .FirstOrDefaultAsync();
            return settings;
        }

    }
}
