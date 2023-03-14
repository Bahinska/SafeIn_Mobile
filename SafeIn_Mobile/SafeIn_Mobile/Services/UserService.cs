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

namespace SafeIn_Mobile.Services
{
    class UserService : IUserService
    {
        // test only


        readonly HttpClientHandler insecureHandler;
        readonly HttpClient _client;
        readonly ILoginService _loginService;


        //static string BaseUrl = "https://motzcoffee.azurewebsites.net";
        public UserService(HttpClient client = null, ILoginService loginService=null)
        {
            _client = client ?? Locator.Current.GetService<HttpClient>();
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();

        }
        //public async Task<IEnumerable<User>> GetUser()
        //{
        //    // get accessToken
        //    var accessToken = await SecureStorage.GetAsync(Constants.AccessToken);
        //    // check accessToken
        //    bool validateAccessToken =  await _loginService.AccessCheckAsync();
        //    if (!validateAccessToken)
        //    {
        //        // go to loginPage because refresh token expired
        //        throw new Exception(AuthErrorMessages.TokensOutdated);
        //    }
        //    // put accessToken into header
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //    var response = await _client.GetAsync("/api/User");

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception(AuthErrorMessages.Unauthorized +": "+ response.ReasonPhrase);
        //    }

        //    var data = await response.Content.ReadAsStringAsync();
        //    var users = JsonConvert.DeserializeObject<IEnumerable<User>>(data);
        //    return users;

        //}

        //public async Task<User> GetUser(int userId)
        //{
        //    if (_client == null) { Console.WriteLine("helb"); }

        //    string url = $"/api/User/id/{userId}";

        //    var json = await _client.GetStringAsync(url);

        //    Console.WriteLine(json);
        //    var user = JsonConvert.DeserializeObject<User>(json);
        //    return user;
        //}






    }
}
