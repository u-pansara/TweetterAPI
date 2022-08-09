using TweetsAPI.Model;

namespace TweetsAPI.Services
{
    public interface ILoginRepository
    {                                   
        Task<ServiceResponse<int>> Register(User user, string password);

        Task<ServiceResponse<string>> Register(string userName, string password);

        Task<bool> UserExists(string userName);

        Task<ServiceResponse<string>> Login(string userName, string password);

        Task<ServiceResponse<List<UserCredential>>> GetAllUsers();

        Task<ServiceResponse<UserCredential>> GetByUserName(string userName);

        Task<ServiceResponse<List<UserCredential>>> SearchByUserName(string userName);
    }
}
