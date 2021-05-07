using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class CommonRequestObject
    {
        public string MobileNo { set; get; }
        public string SchoolTenentId { set; get; }
        public string Email { set; get; }
        public string Password { set; get; } = null;
        public bool ParentUid { set; get; } = false;
        public string StudentUid { set; get; }
        public string SessionUid { set; get; }
    }
}
