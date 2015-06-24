using Agency.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.DataAccess.ViewModels
{
    public class ReceiptViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string[] Items { get; set; }
        public Contract Contract { get; set; }
        public DateTime DateTime { get; set; }
        public string Operator { get; set; }
    }
}
