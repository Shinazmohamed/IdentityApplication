﻿using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ICollection<ApplicationUser> GetUsers()
        {
            return _context.Users.ToList();
        }
        public ApplicationUser GetUser(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            _context.Update(user);
            _context.SaveChanges();

            return user;
        }
        public ICollection<ApplicationUser> GetUsersWithLocations()
        {
            return _context.Users
                .Include(e => e.Location)
                .ToList();
        }

        public ICollection<ListUsersModel> GetUsersWithRoles()
        {
            return _context.Users
            .GroupJoin(
                _context.UserRoles,
                user => user.Id,
                userRole => userRole.UserId,
                (user, userRoles) => new
                {
                    User = user,
                    UserRoles = userRoles
                }
            )
            .SelectMany(
                x => x.UserRoles.DefaultIfEmpty(),
                (user, userRole) => new
                {
                    User = user.User,
                    RoleId = userRole != null ? userRole.RoleId : null
                }
            )
            .GroupJoin(
                _context.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, roles) => new
                {
                    User = userRole.User,
                    Roles = roles
                }
            )
            .SelectMany(
                x => x.Roles.DefaultIfEmpty(),
                (userRole, role) => new
                {
                    User = userRole.User,
                    RoleName = role != null ? role.Name : null
                }
            )
            .Join(
                _context.Location,
                user => user.User.LocationId,
                location => location.LocationId,
                (user, location) => new
                {
                    user.User,
                    Location = location,
                    user.RoleName
                }
            )
            .GroupBy(x => new { x.User.Id, x.User.Email, x.Location.LocationName, x.RoleName })
            .Select(group => new ListUsersModel
            {
                Id = Guid.Parse(group.Key.Id),
                Email = group.Key.Email,
                LocationName = group.Key.LocationName,
                Role = group.Key.RoleName
            })
            .ToList();


        }

    }
}
