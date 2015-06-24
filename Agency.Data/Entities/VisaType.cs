using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public class VisaType
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public VisaType Parent { get; set; }
    }
}
