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
        public const string AzureConnectionUrl = "https://safeinapisecondaccount.azurewebsites.net/";
        public const string LocalAndroidConnectionUrl = "https://192.168.181.1:7090";
        public const string LocalIosConnectionUrl = "http://localhost:5000";
        // user requests
        public const string UserUpdateUrl = "/Auth/edit";
        public const string UserInfoUrl = "/api/Employee/information";
        public const string UserUpdatedMessage = "User updated!";
        public const string UserErrorUpdatedMessage = "User not updated!";
    }
}
