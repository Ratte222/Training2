using DAL.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAL.EF
{
    public class AppDBContext : IdentityDbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {                
        //        optionsBuilder.UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=SignalR_Training;Trusted_Connection=True;MultipleActiveResultSets=true");
        //    }            
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Client>(
                m =>
                {
                    m.Property(i => i.RemoveData)
                    .HasDefaultValueSql(null);
                }
            );
            modelBuilder.Entity<Announcement>().HasKey(i => i.Id);
            modelBuilder.Entity<ProductPhoto>(
                m =>
                {
                    m.HasKey(i => i.Id);
                    m.HasOne(i => i.Announcement)
                    .WithMany(j => j.ProductPhotos)
                    .HasForeignKey(i => i.AnnouncementId);
                });
        }
    }
}
