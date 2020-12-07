using NRZ.Models.Company;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<RegixCompanyModel> AddRegixCompanyAsync(RegixCompanyModel model);
        Task<RegixCompanyModel> GetCompanyFromRegixAsync(string identifier);
    }
}
