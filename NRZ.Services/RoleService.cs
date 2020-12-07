using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.Role;
using NRZ.Models.User;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(NRZContext context,
                           IStringLocalizer<SharedResources> localizer)
            : base(context, localizer)
        {

        }

        public async Task<IEnumerable<string>> GetAll()
        {
            return await _context.AspNetRoles
                .Select(role => role.Name).ToListAsync();
        }

        public IQueryable<RoleViewModel> All()
        {
            return _context.AspNetRoles.ToViewModels();
        }

        public Task<List<RoleViewModel>> ListAll()
        {
            return All()
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task EditRole(AspNetRoleModel model)
        {
            var aspNetRole = _context.AspNetRoles.Where(x => x.Id.ToLower().Trim() == model.Id.Trim().ToLower()).SingleOrDefault();

            if (aspNetRole == null)
            {
                throw new NullReferenceException(String.Concat(_localizer["EditDataError"], _localizer["RoleNotFound"]));
            }

            aspNetRole.Name = model.Name.Trim();
            aspNetRole.NormalizedName = model.Name.ToUpper().Trim();

            var users = await _context.AspNetUserRoles.Where(x => x.RoleId == model.Id).ToListAsync();

            var userToRemove = users.Where(x => !model.Users.Select(u => u.Id).Contains(x.UserId));
            var userToAdd = model.Users.Where(x => !users.Select(u => u.UserId).Contains(x.Id));

            foreach (var user in userToRemove)
            {
                _context.Entry<AspNetUserRoles>(user).State = EntityState.Deleted;
            }

            foreach (var user in userToAdd)
            {
                var aspNetUserRole = new AspNetUserRoles
                {
                    RoleId = aspNetRole.Id,
                    UserId = user.Id
                };
                await _context.AspNetUserRoles.AddAsync(aspNetUserRole);
            }

            await _context.SaveChangesAsync();
        }

        public async Task EditUserRoleByUser(AspNetUserModel model)
        {
            var aspNetUserRoles = await _context.AspNetUserRoles.Where(x => x.UserId.ToLower().Trim() == model.Id.Trim().ToLower()).ToListAsync();

            if (aspNetUserRoles == null)
            {
                throw new NullReferenceException(String.Concat(_localizer["EditDataError"], _localizer["RoleNotFound"]));
            }

            var rolesToRemove = aspNetUserRoles.Where(x => !model.Roles.Select(u => u.Id).Contains(x.RoleId));
            var rolesToAdd = model.Roles.Where(x => !aspNetUserRoles.Select(u => u.RoleId).Contains(x.Id));

            foreach (var role in rolesToRemove)
            {
                _context.Entry<AspNetUserRoles>(role).State = EntityState.Deleted;
            }

            foreach (var role in rolesToAdd)
            {
                var aspNetUserRole = new AspNetUserRoles
                {
                    RoleId = role.Id,
                    UserId = model.Id
                };
                await _context.AspNetUserRoles.AddAsync(aspNetUserRole);
            }

            await _context.SaveChangesAsync();
        }

        public IQueryable<RoleViewModel> GetRolesByUserId(string id)
        {
            var userRoles = _context.AspNetUserRoles
                                    .Include(x => x.Role)
                                    .Where(x => x.UserId.ToLower().Trim() == id.Trim().ToLower())
                                    .Select(x => x.Role)
                                    .ToViewModels();
            return userRoles;
        }
    }
}
