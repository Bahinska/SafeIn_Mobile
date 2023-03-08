using System;
using System.Collections.Generic;
using System.Text;

namespace SafeIn_Mobile.Models.Auth
{
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
