using NRZ.Data;
using NRZ.Models.Distraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IDistraintService
    {
        Task<DistraintCreateModel> AddAsync(DistraintCreateModel model);
        Task<DistraintEnforceModel> EnforceAsync(DistraintEnforceModel model);
        Task<DistraintRevokeModel> RevokeAsync(DistraintRevokeModel model);
        Task<DistraintExemptModel> ExemptAsync(DistraintExemptModel model);
        IQueryable<DistraintViewModel> GetAll();
        IQueryable<DistraintViewModel> Search(string searchText);
        Task<DistraintViewModel> GetByIdAsync(int id);
    }
}
