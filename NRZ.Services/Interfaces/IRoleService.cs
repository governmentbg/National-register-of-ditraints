using NRZ.Models.Role;
using NRZ.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<string>> GetAll();
        IQueryable<RoleViewModel> All();
        Task<List<RoleViewModel>> ListAll();
        Task EditRole(AspNetRoleModel model);
        Task EditUserRoleByUser(AspNetUserModel model);
        IQueryable<RoleViewModel> GetRolesByUserId(string id);
    }
}
