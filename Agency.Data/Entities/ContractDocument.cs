using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public class ContractDocument
    {
        public ContractDocument()
        {

        }
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public long ContractId { get; set; }
        [ForeignKey("ContractId")]
        public Contract Contract { get; set; }
    }
}
