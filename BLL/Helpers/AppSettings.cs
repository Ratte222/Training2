using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Helpers
{
    public class AppSettings
    {
        public string Issuer { get; set; }    // issuer token
        public string Audience { get; set; }  // audience token
        public int Lifetime { get; set; }     // life time token
        public string Secret { get; set; }    // key for crypt
        public string PathTemp { get; set; }

        public string PathSaveProductPhoto
        {
            get
            {
                return System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),
                    PathTemp);
            }
        }
        public int PathSliceDepth { get; set; }

        public int ResizeImageWidht { get; set; }
        public string DefaultPathToVideo { get; set; }
        public string DefaultPathToAudio { get; set; }
        public int AnnouncementsInRedisCache { get; set; }
    }
}
