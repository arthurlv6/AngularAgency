using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Entities
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public DateTime? EventUtc { get; set; }
        public string DeviceId { get; set; }
        public string Message { get; set; }
        public string MessageDump { get; set; }
        public string MethodName { get; set; }
    }
}
