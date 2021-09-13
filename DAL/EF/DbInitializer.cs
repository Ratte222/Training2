using DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuxiliaryLib.Extensions;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace DAL.EF
{
    public static class DbInitializer
    {
        internal static readonly char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        
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

        public static void FillProductPhoto(AppDBContext dbContext)
        {
            dbContext.ChangeTracker.QueryTrackingBehavior =
                        Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
            bool autoDetectChangesEnabled = dbContext.ChangeTracker.AutoDetectChangesEnabled;
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            //long indexAnnouncementId = 0;
            int skip = 0, take = 10000, minProductPhoto = 5;
            while(true)
            {
                //to list necessary https://stackoverflow.com/questions/2113498/sqlexception-from-entity-framework-new-transaction-is-not-allowed-because-ther
                var announcements = dbContext.Announcements.Include(i => i.ProductPhotos)
                    /*.SkipWhile(i=>i.ProductPhotos.Count >= minProductPhoto)*/.Skip(skip).Take(take).ToList();
                Random random = new Random();
                foreach (var ann in announcements)
                {
                    if (ann.ProductPhotos.Count < minProductPhoto)
                    {
                        List<ProductPhoto> productPhotos = new List<ProductPhoto>();
                        for (int j = 0; j < random.Next(minProductPhoto, 10); j++)
                        {
                            productPhotos.Add(new ProductPhoto()
                            {
                                Name = GetUniqueKey(16),
                                DateTime = DateTime.Now,
                                IsPublic = true,
                                Size = random.Next(5120),
                                AnnouncementId = ann.Id,
                                MimeType = $"image/jpeg"
                            });
                        }
                        dbContext.ProductPhotos.AddRange(productPhotos);
                    }
                    if ((ann.Id % 100) == 0)
                    {
                        dbContext.SaveChanges();
                    }
                }
                if (announcements.Count < take)
                    break;
                else
                    skip += take;
            }
            
            dbContext.SaveChanges();
            dbContext.ChangeTracker.QueryTrackingBehavior =
                QueryTrackingBehavior.TrackAll;
            dbContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public static string FillMoreRandomData(AppDBContext dbContext, UserManager<Client> userManager, 
            int newUserCount)
        {
            if (newUserCount <= 0)
                return $"{nameof(newUserCount)} must be greater than zero";
            StringBuilder info = new StringBuilder();
            bool autoDetectChangesEnabled = dbContext.ChangeTracker.AutoDetectChangesEnabled;
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            dbContext.ChangeTracker.QueryTrackingBehavior =
                        Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
            for (int i = 0; i < newUserCount; i++)
            {
                info.Append(AddClient(dbContext, userManager).GetAwaiter().GetResult());
                if((i%100) == 0)
                {
                    dbContext.SaveChanges();                    
                }
            }
            dbContext.SaveChanges();
            dbContext.ChangeTracker.QueryTrackingBehavior =
                QueryTrackingBehavior.TrackAll;
            dbContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesEnabled;
            //List<Task> listTasks = new List<Task>();
            //for (int i = 0; i < newUserCount; i++)
            //{
            //    listTasks.Add(AddClient(dbContext, userManager));
            //}
            //var t = Task.WhenAll(listTasks);
            //try
            //{
            //    t.Wait();
            //}
            //catch (Exception ex)
            //{

            //}


            //if (t.Status == TaskStatus.RanToCompletion)
            //    return "Task completed successfully ";
            //else
            //    return $"Task not completed. Task.Status = {t.Status}";
            return info.ToString();
        }

        private static async Task<string> AddClient(AppDBContext dbContext, UserManager<Client> userManager)
        {
            string result = "";
            try
            {
                Client client = new Client()
                {
                    FirstName = GetUniqueKey(8),
                    LastName = GetUniqueKey(12),
                    Email = $"{GetUniqueKey(25)}@gmail.com",
                    UserName = GetUniqueKey(25),
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Now
                };
                string password = GetUniqueKey(16);
                var resulr = await userManager.CreateAsync(client, password);
                if (!resulr.Succeeded)
                    throw new Exception("Create user failed");
                IdentityResult resultAddRole = await userManager.AddToRoleAsync(client, AccountRole.User);
                if (!resultAddRole.Succeeded)
                    throw new Exception("add user role failed");
                Random random = new Random();
                List<Announcement> announcements = new List<Announcement>();
                for (int i = 0; i < random.Next(1, 355); i++)
                {
                    announcements.Add(new Announcement()
                    {
                        Name = GetUniqueKey(15),
                        Description = GetUniqueKey(random.Next(80, 500)),
                        Cost = random.Next(10000000),
                        Weight = random.Next(1000),
                        Category = (Category)random.Next(14),
                        ClientId = client.Id
                    });
                }
                await dbContext.Announcements.AddRangeAsync(announcements);
                //await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                result =
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"InnerException = {ex?.InnerException?.ToString()} \r\n" + 
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}";
            }
            return result;
        }

        public static string GetUniqueKey(int size)
        {
            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }
    }
}
