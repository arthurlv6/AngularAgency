using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public class SchoolType
    {
        public SchoolType()
        {
            Schools = new HashSet<School>();
        }
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public ICollection<School> Schools { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public SchoolType Parent { get; set; }
    }
}
