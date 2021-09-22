using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Model
{
    public class Log:BaseEntity<long>
    {
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        [StringLength(128)]
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
    }
}
