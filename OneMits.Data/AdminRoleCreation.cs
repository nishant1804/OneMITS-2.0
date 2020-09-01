﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EtherealMade.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtherealMade.Data
{
    public class AdminRoleCreation
    {
        private ApplicationDbContext _context;
        public AdminRoleCreation(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SeedAdminUser()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var userStore = new UserStore<ApplicationUser>(_context);
            var user = new ApplicationUser
            {
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@EtherealMade.com",
                NormalizedEmail = "admin@EtherealMade.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var hasher = new PasswordHasher<ApplicationUser>();
            var hashedPassword = hasher.HashPassword(user, "admin123");

            user.PasswordHash = hashedPassword;

            var hasAdminRole = _context.Roles.Any(roles => roles.Name == "Admin");
            if (!hasAdminRole)
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "admin" });
            }
            var hasStudentRole = _context.Roles.Any(roles => roles.Name == "Student");
            if (!hasStudentRole)
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Student", NormalizedName = "student" });
            }
            var hasTeacherRole = _context.Roles.Any(roles => roles.Name == "Teacher");
            if (!hasTeacherRole)
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Teacher", NormalizedName = "Teacher" });
            }

            var hasSuperUser = _context.Users.Any(u => u.NormalizedUserName == user.UserName);
            if (!hasSuperUser)
            {
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "Admin");
            }

            await _context.SaveChangesAsync();
        }
    }
}
