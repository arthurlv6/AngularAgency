using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Agency.Data.Entities
{
    public partial class Contract
    {
        public Contract()
        {
            ContractDocuments = new HashSet<ContractDocument>();
        }
        public long Id { get; set; }
        public long CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public long VisaTypeId { get; set; }
        [ForeignKey("VisaTypeId")]
        public VisaType VisaType { get; set; }
        [Required]
        [MaxLength(50)]
        public string CustomerName { get; set; }
        [MaxLength(50)]
        public string Reference { get; set; }
        [MaxLength(50)]
        public string School { get; set; }
        [MaxLength(50)]
        public string Position { get; set; }
        
        [Required]
        public DateTime? SignDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal ServiceFee { get; set; }
        public DateTime? ServiceFeeDate { get; set; }
        public decimal ApplicationFee { get; set; }
        public DateTime? ApplicationFeeDate { get; set; }
        public decimal InsuranceFee { get; set; }
        public DateTime? InsuranceFeeDate { get; set; }
        public decimal CommissionFee { get; set; }
        public DateTime? CommissionFeeDate { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Detail { get; set; }
        public DateTime? InsuranceTo { get; set; }
        public DateTime? VisaTo { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }
        public bool? Deleted { get; set; }
        public ICollection<ContractDocument> ContractDocuments { get; set; }
    }
}
