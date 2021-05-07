using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class SignUpModel
    {
        public string MobileNo { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string SchoolTenentId { set; get; }
        public string UserId { set; get; }
        public bool IsFaculty { set; get; }
        public string AdminId { set; get; }
        public string Address { set; get; }
        public string FullName { set; get; }
        public string SchoolName { set; get; }
        public string LicenseNo { set; get; }
        public string AffilatedBy { set; get; }
        public string AffilationNum { set; get; }
    }
}
