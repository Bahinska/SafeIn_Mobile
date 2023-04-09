using System;
using System.Collections.Generic;
using System.Text;

namespace SafeIn_Mobile.Models.Auth
{
    public class RevokeTokensResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
