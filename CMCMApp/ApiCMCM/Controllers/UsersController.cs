using LibraryCMCM.DataAccess;
using LibraryCMCM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCMCM.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUserData _userData;

    public UsersController(IConfiguration config, IUserData userData)
    {
        _config = config;
        _userData = userData;
    }

    #region GET
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(int id)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            if (userId != null)
            {
                if (int.Parse(userId) != id)
                {
                    return BadRequest("You are not allowed to delete somebodies else's account");
                }
                var result = await _userData.GetUserInfo(int.Parse(userId));
                return Ok(result);
            }
            return NotFound("User not found");
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
    #endregion


    #region POST LOGIN REGISTER
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserModel user)
    {
        try
        {
            var result = await _userData.PostUser(user.FirstName, user.LastName, user.IdentificationNumber, user.EmailAddress, user.Password);
            return StatusCode(StatusCodes.Status201Created,result);
        }
        catch (Exception)
        {
            return BadRequest("Please fill all the fields!");
        }

    }

    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserModel user)
    {
        try
        {
            var currentUser = await _userData.GetUser(user.EmailAddress, user.Password);
            if (currentUser is null)
            {
                return NotFound("User not found! Email or Password is not correct");
            }
            var token = GenerateToken(currentUser);
            return Ok(token);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
    #endregion

    #region PUT
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserModel user)
    {
        try
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userId = User.Claims.FirstOrDefault(c=> c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userEmail == user.EmailAddress)
            {
                await _userData.UpdateUser(int.Parse(userId), user.FirstName, user.LastName, user.IdentificationNumber, user.EmailAddress);
                return Ok("Updated Successfully");
            }
            return NotFound("User not found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    #endregion

    #region DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        try
        {
            if (userId != null)
            {
                if (int.Parse(userId) != id)
                {
                    return BadRequest("You are not allowed to delete somebodies else's account");
                }
                await _userData.DeleteUser(int.Parse(userId));
                return Ok("User successfully deleted!");
            }
            return NotFound("User not found");
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
    #endregion

    private string GenerateToken(UserModel user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetValue<string>("Authentication:SecretKey")!));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new();
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.Email, user.EmailAddress));

        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(50),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
