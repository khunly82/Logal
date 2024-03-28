using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logal.Controllers
{
    [ApiController]
    [Route("ClientB")]
    [Authorize(Roles = "logalMovies2")]
    public class ClientBController: CommonController
    {

        [HttpGet("Method3")]
        public IActionResult Method3()
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            return Ok("Client B Method 3");
        }

        [HttpGet(nameof(Method1))]
        public override IActionResult Method1()
        {
            return Ok("Client B Method 1");
        }
    }
}
