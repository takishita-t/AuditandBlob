using AuditSample;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var obj = new UserObj {
                Name = "user",
                Age = 25
            };
            return Ok(obj);
        }

        [HttpPost("logintwofactor")]
        public async Task<ActionResult<string>> LoginTwoFactor(User request)
        {
            string token = CreateToken(request);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            var jwt = "takishita";
            return jwt;
        }
    }
}
