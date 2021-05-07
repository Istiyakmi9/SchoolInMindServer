using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModal.Models
{
    public class AuthenticatedUserOBJ
    {
        public string Mobileno { set; get; }
        public string EmailId { set; get; }
        public string UserId { set; get; }
        public string FullName { set; get; }
        public string Address { set; get; }
        public string City { set; get; }
        public string State { set; get; }
    }

    public class AuthUser
    {
        public string Role { set; get; }
        public string MobileNo { set; get; }
        public string SchoolTenentId { set; get; }
        public string Email { set; get; }
        public string UserId { set; get; }
        public string Password { set; get; }
        public bool IsFaculty { set; get; }
        public string AdminId { set; get; }
        public string SessionToken { set; get; }
    }
}
