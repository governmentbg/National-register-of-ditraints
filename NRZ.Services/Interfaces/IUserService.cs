using NRZ.Data;
using NRZ.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IUserService
    {
        IQueryable<ApplicationUser> GetAll();
        Task<ApplicationUser> GetByIdAsync(string id);
        Task AproveUser(string userId, string approvedBy);
        Task DeleteUser(string userId, string deletedBy);
        Task ActivateUser(string userId, string deletedBy);
        Task<ApplicationUserRoles> GetUserRoleByIdAsync(string userId);
        Task<ApplicationUser> GetByUserNameAsync(string userName);
    }
}
