using System;
using System.Collections.Generic;
using System.Text;

namespace SafeIn_Mobile.Models
{
    public class UserInfoResult
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Role { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
