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
        public const string User = "user";
        public const string EmailNotValidMessage= "Email is not valid";
        public const string PasswordEmptyMessage= "Empty password";
        public const string QrCodeError= "Error occured during QrCode generation";
        public const int QrCodeExpirationTime = 10000;


        // connection
        public const String AzureConnectionUrl = "https://safeinapisecondaccount.azurewebsites.net/";
        public const String LocalAndroidConnectionUrl = "https://192.168.181.1:7090";
        public const String LocalIosConnectionUrl = "http://localhost:5000";
        // user requests
        public const String UserUpdateUrl = "/Auth/edit";
        public const String UserInfoUrl = "/api/Employee/information";
    }
}
