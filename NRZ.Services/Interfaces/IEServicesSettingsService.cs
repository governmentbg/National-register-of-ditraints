using NRZ.Models.EPayment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IEServicesSettingsService
    {
        Task<EServicesSettingsModel> GetEServiceSettingsAsync();
    }
}
