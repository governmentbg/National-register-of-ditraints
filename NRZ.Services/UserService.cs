using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Models.Identity;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(NRZContext context, IStringLocalizer<SharedResources> localizer)
            : base(context, localizer)
        { }

        public IQueryable<ApplicationUser> GetAll()
        {
            var result = from u in _context.AspNetUsers
                         select new ApplicationUser()
                         {
                             Id = u.Id,
                             UserName = u.UserName,
                             Email = u.Email,
                             EmailConfirmed = u.EmailConfirmed,
                             ConfirmedByAdmin = u.ConfirmedByAdmin,
                             Deleted = u.Deleted
                         };

            return result.OrderBy(x => x.UserName);
        }

        public async Task<ApplicationUserRoles> GetUserRoleByIdAsync(string userId)
        {
            var user = await _context.AspNetUsers
                        .Include(x => x.AspNetUserRoles)
                        .ThenInclude(x => x.Role)
                        .Where(x=>x.Id == userId.Trim().ToLower())
                        .Select(x => new ApplicationUserRoles
                        {
                            UserId = x.Id,
                            UserName = x.UserName,
                            Email = x.Email,
                            Roles = x.AspNetUserRoles
                                    .Where(r => r.UserId == userId)
                                    .Select(r => new ApplicationRole
                                    {
                                        Id = r.Role.Id,
                                        Name = r.Role.Name,
                                        NormalizedName = r.Role.NormalizedName,
                                        ConcurrencyStamp = r.Role.ConcurrencyStamp
                                    })
                        }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            var result = await GetAll().SingleOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            var result = await GetAll().SingleOrDefaultAsync(x => x.UserName == userName);
            return result;
        }

        public async Task AproveUser(string userId, string approvedBy)
        {
            var user = await _context.AspNetUsers.FindAsync(userId);

            if (user != null)
            {
                user.ConfirmedByAdmin = true;
                user.ApprovedBy = approvedBy;
                user.ApprovedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(string userId, string deletedBy)
        {
            var user = await _context.AspNetUsers.FindAsync(userId);

            if (user != null)
            {
                user.Deleted = true;
                SetDeleteStamp(user, deletedBy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ActivateUser(string userId, string deletedBy)
        {
            var user = await _context.AspNetUsers.FindAsync(userId);

            if (user != null)
            {
                user.Deleted = false;
                user.DeletedBy = null;
                user.DeletedOn = null;
                SetUpdateStamp(user, deletedBy);

                await _context.SaveChangesAsync();
            }
        }
    }
}
