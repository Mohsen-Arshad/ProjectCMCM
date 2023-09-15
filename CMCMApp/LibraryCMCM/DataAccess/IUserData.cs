using LibraryCMCM.Models;

namespace LibraryCMCM.DataAccess
{
    public interface IUserData
    {
        Task DeleteUser(int id);
        Task<UserModel> GetUser(string emailAddress, string password);
        Task<UserModel> GetUserInfo(int id);
        Task<UserModel> PostUser(string firstName, string lastName, string identificationNumber, string emailAddress, string password);
        Task UpdateUser(int id, string firstName, string lastName, string identificationNumber, string emailAddress);
    }
}