using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Logal.Controllers
{
    [ApiController]
    [Route("ClientA")]
    [Authorize(Roles = "logalMovies1,supervisor")]
    public class ClientAController : CommonController
    {
        public ClientAController(MongoClient mongoClient) : base(mongoClient)
        {
        }
    }
}
