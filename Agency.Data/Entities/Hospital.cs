using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public class Hospital
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Area { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        public int SortOrder { get; set; }
    }
}
