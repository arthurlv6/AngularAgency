using Agency.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Agency.Api.ViewModels
{
    public class ViewModelSchool
    {
        public long Id { get; set; }
        public long SchoolTypeId { get; set; }
        [ForeignKey("SchoolTypeId")]
        public ViewModelSchoolType SchoolType { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Website { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength(256)]
        public string Area { get; set; }
        [MaxLength(256)]
        public string Address { get; set; }
        [MaxLength(256)]
        public string Profession { get; set; }
        public decimal Fee { get; set; }
        public decimal Commission { get; set; }
        [MaxLength(50)]
        public string ContactName { get; set; }
        [MaxLength(50)]
        public string ContactNumber { get; set; }
        [MaxLength(50)]
        public string ContactOthers { get; set; }
        public string Detail { get; set; }
        public DateTime CreateDate { get; set; }
        [MaxLength(256)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        
        public bool Show { get; set; }
    }
}