using System;
using System.Collections.Generic;
using System.Text;

namespace SafeIn_Mobile.Helpers
{
    internal class Constants
    {
        public const string AccessToken = "access_token";
        public const string RefreshToken = "refresh_token";
        public const string Email = "email";
        public const string Password = "password";
        public const string EmailNotValidMessage= "Email is not valid";
        public const string PasswordEmptyMessage= "Empty password";
        public const string QrCodeError= "Error occured during QrCode generation";
        public const int QrCodeExpirationTime = 300;
    }
}
