using BottomhalfCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class Subject
    {
        public string SubjectId { set; get; }
        [Required]
        public string SubjectName { set; get; }
        [Required]
        public int SubjectCode { set; get; }
        public int SubjectCredit { set; get; }
        public bool IsActive { set; get; }
        public string ForClass { set; get; }
    }
}
