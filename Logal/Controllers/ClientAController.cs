using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logal.Controllers
{
    [ApiController]
    [Route("ClientA")]
    [Authorize(Roles = "logalMovies1,supervisor")]
    public class ClientAController: CommonController
    {


    }
}
