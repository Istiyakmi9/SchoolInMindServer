using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationToken.Model
{
    public class SessionModal
    {
        public string UserId { set; get; }
        public string TenentId { set; get; }
        public string Mobile { set; get; }
        public string Email { set; get; }
        public string Role { set; get; }
    }
}
