using NRZ.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRZ.Data.Extensions
{
    public static class RoleExtensions
    {
        public static RoleViewModel ToViewModel(this AspNetRoles role)
        {
            if (role == null) return null;

            return new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                //Description = role.Description,
                //Deactivated = role.Deactivated
            };
        }

        public static IQueryable<RoleViewModel> ToViewModels(this IQueryable<AspNetRoles> query)
        {
            return query.Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name,
                NormalizedName = x.NormalizedName,
                //Description = x.Description,
                //Deactivated = x.Deactivated
            });
        }
    }
}
