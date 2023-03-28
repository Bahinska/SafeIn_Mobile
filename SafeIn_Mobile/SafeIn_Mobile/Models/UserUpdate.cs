using System;
using System.Collections.Generic;
using System.Text;

namespace SafeIn_Mobile.Models
{
    public class UserUpdate
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
    }
}
