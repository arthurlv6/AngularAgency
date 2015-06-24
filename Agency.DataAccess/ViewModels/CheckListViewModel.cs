using Agency.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.DataAccess.ViewModels
{
    public class CheckListViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Customer Customer { get; set; }
        public string[] Requirements { get; set; }
        public string Hospital { get; set; }
        public List<Hospital> Hospitals { get; set; }
    }
}
