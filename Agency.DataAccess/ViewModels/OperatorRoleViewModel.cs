using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.DataAccess.ViewModels
{
    public class OperatorRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int TempId { get; set; }
        public string[] SelectedViewRoles { get; set; }
    }
}
