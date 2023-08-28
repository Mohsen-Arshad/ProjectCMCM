using LibraryCMCM.DataAccess;
using LibraryCMCM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiCMCM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestData _requestData;

        public RequestsController(IRequestData requestData)
        {
            _requestData = requestData;
        }

        #region GET
        [HttpGet]
        public async Task<IActionResult> GetAllRequests()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _requestData.GetAllRequests(int.Parse(userId));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _requestData.GetRequest(id);

            if (int.Parse(userId) == result.UserId)
            {
                return Ok(result);
            }

            return NotFound("Record not found or you don't have access to this record!");
        }
        #endregion

    }
}
