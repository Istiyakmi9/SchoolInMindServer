using BottomhalfCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    [Transient]
    public class ServiceResult
    {
        public bool IsValidModal { set; get; }
        public string ErrorMessage { set; get; }
        public string SuccessMessage { set; get; }
        public int StatusCode { set; get; }
        public IList<string> ErrorResultedList { set; get; }
    }
}
