
using LibraryCMCM.Models;

namespace LibraryCMCM.DataAccess;

public class UserData : IUserData
{
    private readonly ISqlDataAccess _sql;

    public UserData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    //dbo.spGetUser -- Get User Info
    public async Task<UserModel> GetUserInfo(int id)
    {
        var result = await _sql.LoadData<UserModel, dynamic>(
            "dbo.spGetUserInfo",
            new { Id = id },
            "Default");

        return result.FirstOrDefault();
    }

    //dbo.spGetUser -- Login
    public async Task<UserModel> GetUser(string emailAddress, string password)
    {
        var result = await _sql.LoadData<UserModel, dynamic>(
            "dbo.spGetUser",
            new { EmailAddress = emailAddress, Password = password },
            "Default");

        return result.FirstOrDefault();
    }


    //dbo.spPostUser -- Register
    public async Task<UserModel> PostUser(
        string firstName,
        string lastName,
        string identificationNumber,
        string emailAddress,
        string password)
    {
        var result = await _sql.LoadData<UserModel, dynamic>(
            "dbo.spPostUser",
            new
            {
                FirstName = firstName,
                LastName = lastName,
                IdentificationNumber = identificationNumber,
                EmailAddress = emailAddress,
                Password = password
            },
            "Default");

        return result.FirstOrDefault();
    }

    //dbo.spUpdateUser
    public Task UpdateUser(
        int id,
        string firstName,
        string lastName,
        string identificationNumber,
        string emailAddress)
    {
        return _sql.SaveData(
            "dbo.spUpdateUser",
            new
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                IdentificationNumber = identificationNumber,
                EmailAddress = emailAddress
            },
            "Default");
    }

    //dbo.spDeleteUser
    public Task DeleteUser(int id)
    {
        return _sql.SaveData("dbo.spDeleteUser", new { Id = id }, "Default");
    }
}
