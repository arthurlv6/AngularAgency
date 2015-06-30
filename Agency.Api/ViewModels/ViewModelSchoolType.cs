using Agency.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Agency.Api.ViewModels
{
    public class ViewModelSchoolType
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public long? ParentId { get; set; }
        //public ICollection<School> Schools { get; set; }
        //public SchoolType Parent { get; set; }
    }
}