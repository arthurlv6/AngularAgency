using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Agency.DataAccess.ViewModels
{
    public class ReturnApiDataViewModel<T> where T : class
    {
        public ReturnApiDataViewModel()
        {
            Data = new List<T>();
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IList<T> Data { get; set; }
    }
}
