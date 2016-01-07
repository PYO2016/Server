using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyo_Server.Models
{
    public class ParsedTable
    {
        [Key]
        public int pk { get; set; }
        public string fk_User { get; set; }
        public bool isProccessed { get; set; }
        public string filename { get; set; }
        public string result { get; set; }
        public DateTime time { get; set; }
    }

    public class ParsedTableInner
    {
        public int pk { get; set; }
        public string fk_User { get; set; }
        public bool isProccessed { get; set; }
        public string filename { get; set; }
        public string result { get; set; }
        public DateTime time { get; set; }

        public ParsedTableInner(int pk, string fk_User, bool isProccessed, string filename, string result, DateTime time)
        {
            this.pk = pk;
            this.fk_User = fk_User;
            this.isProccessed = isProccessed;
            this.filename = filename;
            this.result = result;
            this.time = time;
        }
    }
}
