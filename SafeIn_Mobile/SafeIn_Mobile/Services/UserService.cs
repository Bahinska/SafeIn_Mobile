using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SafeIn_Mobile.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Splat;
using SafeIn_Mobile.Helpers;
using SafeIn_Mobile.Models.Auth;

namespace SafeIn_Mobile.Services
{
    class UserService : IUserService
    {
        // test only


        readonly HttpClient _client;
        readonly ILoginService _loginService;


        //static string BaseUrl = "https://motzcoffee.azurewebsites.net";
        public UserService(HttpClient client = null, ILoginService loginService = null)
        {
            _client = client ?? Locator.Current.GetService<HttpClient>();
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();

        }

        public async Task<UserInfoResult> GetUserInfo()
        {
            // get accessToken
            var accessToken = await SecureStorage.GetAsync(Constants.AccessToken);
            // check accessToken
            var assessCheckResult = await _loginService.AccessCheckAsync();
            if (!assessCheckResult.Success)
            {
                // go to loginPage because refresh token expired
                return new UserInfoResult { Success = false, ErrorMessage = assessCheckResult.Message };
            }
            // put accessToken into header
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // create update instance
            try
            {
                var response = await _client.GetAsync(Constants.UserInfoUrl);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle the error response
                    var error = response.ReasonPhrase;
                    return new UserInfoResult { Success = false, ErrorMessage = error };
                }
                var userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(await response.Content.ReadAsStringAsync());

                return new UserInfoResult
                {
                    Success = true,
                    UserName = userInfoResponse.UserName,
                    Email = userInfoResponse.Email,
                    Company = userInfoResponse.Company,
                    Role = userInfoResponse.Role
                };
            }
            catch (Exception ex)
            {
                return new UserInfoResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<UserUpdateResult> UserUpdate(UserUpdate user)
        {
            // get accessToken
            var accessToken = await SecureStorage.GetAsync(Constants.AccessToken);
            // check accessToken
            var assessCheckResult = await _loginService.AccessCheckAsync();
            if (!assessCheckResult.Success)
            {
                // go to loginPage because refresh token expired
                return new UserUpdateResult { Success = false, ErrorMessage = assessCheckResult.Message };
            }
            // put accessToken into header
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // create update instance
            var userUpdateRequest = new UserUpdateRequest { Email = user.Email, UserName = user.UserName, CurrentPassword = user.CurrentPassword, Password = user.Password };
            var requestJson = new StringContent(JsonConvert.SerializeObject(userUpdateRequest), Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PutAsync(Constants.UserUpdateUrl, requestJson);
                if (!response.IsSuccessStatusCode)
                {
                    // handle the error response
                    var error = response.ReasonPhrase;
                    return new UserUpdateResult { Success = false, ErrorMessage = error };
                }
                var data = await response.Content.ReadAsStringAsync();
                return new UserUpdateResult { Success = true };
            }
            catch (Exception ex)
            {
                return new UserUpdateResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public bool WriteUserIntoSecureStorage(User user)
        {
            try
            {
                string userJson = JsonConvert.SerializeObject(user);

                SecureStorage.SetAsync(Constants.User, userJson);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<User> GetUserFromSecureStorageAsync()
        {
            string userJson = await SecureStorage.GetAsync(Constants.User);
            User user = JsonConvert.DeserializeObject<User>(userJson);
            return user;
        }


    }
}
