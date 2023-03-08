using System;
using System.Collections.Generic;
using System.Text;

namespace SafeIn_Mobile.Models.Auth
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
