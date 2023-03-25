using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SafeIn_Mobile.Models;
using SafeIn_Mobile.Models.Auth;

namespace SafeIn_Mobile.Services
{
    public interface IUserService
    {


        Task<UserUpdateResult> UserUpdate(User user);
        Task<UserInfoResult> GetUserInfo();
        Task<bool> WriteUserInfoIntoStorage();
     
    }
}