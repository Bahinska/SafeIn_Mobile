using System;
using System.Collections.Generic;
using System.Text;

namespace SafeIn_Mobile.Models.Auth
{
    public class RefreshTokenResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
