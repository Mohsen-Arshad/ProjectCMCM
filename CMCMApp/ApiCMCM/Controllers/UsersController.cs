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
    //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]   ------ this line is for caching
    public IActionResult GetUser()
    {
        return Ok();
    }
    #endregion

    #region POST LOGIN REGISTER
    [HttpPost("[action]")]
    [AllowAnonymous]
    public IActionResult Register([FromBody] UserModel user)
    {

        return Ok();
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


    #region POST
    #endregion


    #region PUT
    #endregion



    #region DELETE
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
            DateTime.UtcNow.AddMinutes(2),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
