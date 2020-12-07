using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Models.Role
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string Description { get; set; }

        public bool Deactivated { get; set; }

        public bool Active => !Deactivated;
    }
}
