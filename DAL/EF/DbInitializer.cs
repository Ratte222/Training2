using DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuxiliaryLib.Extensions;

namespace DAL.EF
{
    public static class DbInitializer
    {
        public static void Initialize(AppDBContext dbContext, UserManager<Client> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            InitializeIdentityRole(roleManager);
            if (userManager.Users.FirstOrDefault() == null)
            {
                Client artur = new Client()
                {
                    FirstName = "Artur",
                    LastName = "Zaletov",
                    Email = "Art323@gmail.com",
                    UserName = "PowerMaster323",
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now
                };
                Client nik = new Client()
                {
                    FirstName = "Nik",
                    LastName = "Bahovez",
                    Email = "Bahovez123@gmail.com",
                    UserName = "NBA",
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now
                };
                Client kakha = new Client()
                {
                    FirstName = "Kakha",
                    LastName = "Shvili",
                    Email = "Kakha_thisCentry@gmail.com",
                    UserName = "Shark",
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now
                };

                var resulr = userManager.CreateAsync(artur, "aZ12345678*").GetAwaiter().GetResult();
                if (!resulr.Succeeded)
                    throw new Exception("Create user failed");
                IdentityResult resultAddRole = userManager.AddToRoleAsync(artur, AccountRole.Admin).GetAwaiter().GetResult();
                if (!resultAddRole.Succeeded)
                    throw new Exception("add user role failed");
                
                resulr = userManager.CreateAsync(nik, "aZ12345678#").GetAwaiter().GetResult();
                if (!resulr.Succeeded)
                    throw new Exception("Create user failed");
                resultAddRole = userManager.AddToRoleAsync(nik, AccountRole.User).GetAwaiter().GetResult();
                if (!resultAddRole.Succeeded)
                    throw new Exception("add user role failed");
                
                resulr = userManager.CreateAsync(kakha, "aZ12345678&").GetAwaiter().GetResult();
                if (!resulr.Succeeded)
                    throw new Exception("Create user failed");
                resultAddRole = userManager.AddToRoleAsync(kakha, AccountRole.User).GetAwaiter().GetResult();
                if (!resultAddRole.Succeeded)
                    throw new Exception("add user role failed");
            }
        }
        private static void InitializeIdentityRole(RoleManager<IdentityRole> roleManager)
        {
            var roles = roleManager.Roles.ToListAsync().GetAwaiter().GetResult();
            foreach (string role in typeof(AccountRole).GetAllPublicConstantValues<string>())
            {
                if (roles.Find(f => f.Name == role) == null)
                {
                    roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
            }
        }
    }
}
