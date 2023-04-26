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
                return new LoginResult { Success = false, ErrorMessage = e.Message };
            }

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response
                var error = response.ReasonPhrase;
                return new LoginResult { Success = false, ErrorMessage = error };
            }

            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

            // Store the access and refresh tokens securely
            await SecureStorage.SetAsync(Constants.AccessToken, loginResponse.AccessToken);
            await SecureStorage.SetAsync(Constants.RefreshToken, loginResponse.RefreshToken);
            await SecureStorage.SetAsync(Constants.Email, email);
            await SecureStorage.SetAsync(Constants.Password, password);
            return new LoginResult { Success = true };
        }

        public async void Logout()
        {

            var revokeResult = await Revoke();
            if (!revokeResult.Success)
            {
                // log
            }
            SecureStorage.RemoveAll();
        }

        public async Task<AccessCheckResult> AccessCheckAsync()
        {
            // get accessToken
            var accessToken = await SecureStorage.GetAsync(Constants.AccessToken);
            // check accessToken
            if (string.IsNullOrEmpty(accessToken))
            {
                return new AccessCheckResult { Success = false, Message = AuthErrorMessages.TokensOutdated };
            }
            // validate accesstoken
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            try
            {
                var response = await _client.GetAsync("/Auth/tokenValidate");
                if (!response.IsSuccessStatusCode)
                {
                    var error = response.ReasonPhrase;
                    return new AccessCheckResult { Success = false, Message = error };
                }
                return new AccessCheckResult { Success = response.IsSuccessStatusCode };
            }
            catch (Exception)
            {
                return new AccessCheckResult { Success = false, Message = AuthErrorMessages.TokensOutdated };
            }
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
            try
            {
                var response = await _client.PostAsync("/Auth/refresh", requestJson);
                if (!response.IsSuccessStatusCode)
                {
                    // Handle the error response
                    var error = response.ReasonPhrase;
                    return new RefreshTokenResult { Success = false, ErrorMessage = error };
                }
                var refreshTokenResponse = JsonConvert.DeserializeObject<RefreshTokenResponse>(await response.Content.ReadAsStringAsync());
                // Store the new access token
                await SecureStorage.SetAsync(Constants.AccessToken, refreshTokenResponse.AccessToken);
                await SecureStorage.SetAsync(Constants.RefreshToken, refreshTokenResponse.RefreshToken);
                return new RefreshTokenResult { Success = true, AccessToken = refreshTokenResponse.AccessToken, RefreshToken = refreshTokenResponse.RefreshToken };

            }
            catch (Exception)
            {
                return new RefreshTokenResult { Success = false, ErrorMessage = AuthErrorMessages.TokensOutdated };
            }
        }
        public async Task<RevokeResult> Revoke()
        {
            var refreshToken = await SecureStorage.GetAsync("refresh_token");

            if (string.IsNullOrEmpty(refreshToken))
            {
                return new RevokeResult { Success = false, ErrorMessage = AuthErrorMessages.NoTokens };
            }

            var revokeRequest = new RevokeRequest
            {
                RefreshToken = refreshToken,
            };

            var requestJson = new StringContent(JsonConvert.SerializeObject(revokeRequest), Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PostAsync("/Auth/revoke", requestJson);
                if (!response.IsSuccessStatusCode)
                {
                    // Handle the error response
                    var error = response.ReasonPhrase;
                    return new RevokeResult { Success = false, ErrorMessage = error };
                }
                return new RevokeResult { Success = true };

            }
            catch (Exception)
            {
                return new RevokeResult { Success = false, ErrorMessage = AuthErrorMessages.BadRequest };
            }
        }

        public async Task<ApiHealthCheckResult> ApiHealthCheck()
        {
            try
            {
                var response = await _client.GetAsync("/Auth/healthCheck");
                if (!response.IsSuccessStatusCode)
                {
                    return new ApiHealthCheckResult { IsRunning=false};
                }
                return new ApiHealthCheckResult { IsRunning = true };
            }
            catch (Exception)
            {
                return new ApiHealthCheckResult { IsRunning = false };
            }
        }
    }
}
