using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SafeIn_Mobile.Models;

namespace SafeIn_Mobile.Services
{
    public interface IUserService
    {
      
        Task<IEnumerable<User>> GetUser();
        Task<User> GetUser(int userId);
       
    }
}