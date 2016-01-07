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
        public string filename { get; set; }
        public int time { get; set; }
        public int fk_CapturedImages { get; set; }
    }

    public class ParsedTableInner
    {
        public int pk { get; set; }
        public string fk_User { get; set; }
        public string innerfile { get; set; }
        public int time { get; set; }
        public int fk_CapturedImages { get; set; }

        public ParsedTableInner(int pk, string fk_User, string innerfile, int time, int fk_CapturedImages)
        {
            this.pk = pk;
            this.fk_User = fk_User;
            this.innerfile = innerfile;
            this.time = time;
            this.fk_CapturedImages = fk_CapturedImages;
        }
    }
}
