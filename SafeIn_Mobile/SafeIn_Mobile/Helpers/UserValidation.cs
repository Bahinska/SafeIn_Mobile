using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SafeIn_Mobile.Helpers
{
    public static class UserValidation
    {
        public static async Task<bool>  EmailValidAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use regular expression to validate email format
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            }
            catch (RegexMatchTimeoutException)
            {
                // Regex took too long to execute, handle the error here
                return false;
            }
        }
        public static async Task<bool> PasswordValidAsync(string password)
        {

            return !string.IsNullOrEmpty(password);
        }
        public static async Task<bool> UserNameValidAsync(string userName)
        {

            if (string.IsNullOrWhiteSpace(userName))
                return false;

            try
            {
                // Use regular expression to validate email format
                return Regex.IsMatch(userName, @"^[A-Za-z0-9]{5,10}$");
            }
            catch (RegexMatchTimeoutException)
            {
                // Regex took too long to execute, handle the error here
                return false;
            }
        }
    }
}
