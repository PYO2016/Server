using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyo_Server.Models
{
    public class CapturedImage
    {
        [Key]
        public int fk_ParsedTable { get; set; }
        public string filename { get; set; }
    }
}
