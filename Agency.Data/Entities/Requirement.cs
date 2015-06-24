using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public class Requirement
    {
        public int Id { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        public int SortOrder { get; set; }
    }
}
