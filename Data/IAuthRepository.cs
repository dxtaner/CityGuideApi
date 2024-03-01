using CityGuideApi.Models;
using System;
using System.Threading.Tasks; 

namespace CityGuideApi.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string userName, string password);
        Task<bool> UserExists(string userName);
    }
}
