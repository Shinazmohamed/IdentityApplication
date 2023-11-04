﻿using IdentityApplication.Areas.Identity.Data;

namespace IdentityApplication.Core.Entities
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<ApplicationUser> Users { get; set; }
    }
}
