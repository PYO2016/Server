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
        public int fk_User { get; set; }
        public string filename { get; set; }
        public int time { get; set; }
    }
}
