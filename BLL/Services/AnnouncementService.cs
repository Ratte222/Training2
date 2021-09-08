using BLL.Interfaces;
using DAL.EF;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services
{
    public class AnnouncementService: BaseService<Announcement>, IAnnouncementService
    {
        public AnnouncementService(AppDBContext appDBContext):base(appDBContext)
        {

        }

        public override Announcement Get(Func<Announcement, bool> func)
        {
            Announcement announcement = _context.Announcements.Include(i => i.ProductPhotos).FirstOrDefault(func);
            announcement.Views++;
            UpdateViews(announcement);
            return announcement;
        }

        private void UpdateViews(Announcement item)
        {
            _context.Announcements.Attach(item);
            _context.Entry(item).Property(x => x.Views).IsModified = true;
            _context.SaveChanges();
        }
    }
}
