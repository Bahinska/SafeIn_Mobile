using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MobileTest2.Models;
using Newtonsoft.Json;
using SafeIn_Mobile.Helpers;
using SafeIn_Mobile.Models.Auth;
using Splat;
using Xamarin.Essentials;

namespace SafeIn_Mobile.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _client;
        public LoginService(HttpClient client = null)
        {
            _client = client ?? Locator.Current.GetService<HttpClient>();
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var requestJson = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            try
            {
                response = await _client.PostAsync("/Auth/login", requestJson);

            }
            catch (Exception e)
            {
                throw new Exception(AuthErrorMessages.BadRequest + ": " + e.Message);
            }

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response
                var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());
                return new LoginResult { Success = false, ErrorMessage = error.Message };
            }

            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

            // Store the access and refresh tokens securely
            await SecureStorage.SetAsync(Constants.AccessToken, loginResponse.AccessToken);
            await SecureStorage.SetAsync(Constants.RefreshToken, loginResponse.RefreshToken);
            await SecureStorage.SetAsync(Constants.Email, email);
            await SecureStorage.SetAsync(Constants.Password, password);
            return new LoginResult { Success = true };
        }

        public void Logout()
        {
            SecureStorage.RemoveAll();
        }

        public async Task<bool> AuthCheck()
        {
            // get accessToken
            var refreshToken = await SecureStorage.GetAsync("refresh_token");
            var accessToken = await SecureStorage.GetAsync("access_token");
            // check accessToken
            if (string.IsNullOrEmpty(accessToken)|| string.IsNullOrEmpty(refreshToken ))
            {
                // go to loginPage because refresh token doesn't exist
                return false;
            }
            // validate accesstoken
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync("/Auth/tokenValidate");       
            return response.IsSuccessStatusCode;
        }
        public async Task<RefreshTokenResult> RefreshTokensAsync()
        {
            var refreshToken = await SecureStorage.GetAsync("refresh_token");
            var accessToken = await SecureStorage.GetAsync("access_token");

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return new RefreshTokenResult { Success = false, ErrorMessage = AuthErrorMessages.TokensOutdated };
            }

            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = refreshToken,
                AccessToken = accessToken
            };

            var requestJson = new StringContent(JsonConvert.SerializeObject(refreshTokenRequest), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/Auth/refresh", requestJson);

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response
                var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());
                return new RefreshTokenResult { Success = false, ErrorMessage = error.Message };
            }

            var refreshTokenResponse = JsonConvert.DeserializeObject<RefreshTokenResponse>(await response.Content.ReadAsStringAsync());

            // Store the new access token
            await SecureStorage.SetAsync(Constants.AccessToken, refreshTokenResponse.AccessToken);
            await SecureStorage.SetAsync(Constants.RefreshToken, refreshTokenResponse.RefreshToken);

            return new RefreshTokenResult { Success = true, AccessToken = refreshTokenResponse.AccessToken, RefreshToken = refreshTokenResponse.RefreshToken };
        }


    }
}
