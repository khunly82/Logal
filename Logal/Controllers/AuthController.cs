using Logal.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Logal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private MongoClient _client;
        private JwtManager _jwtManager;

        public AuthController(MongoClient client, JwtManager jwtManager)
        {
            _client = client;
            _jwtManager = jwtManager;
        }

        [HttpPost]
        public IActionResult Login([FromBody] dynamic login)
        {
            string username = login.Username;
            string password = login.Password;
            string dbName = login.DatabaseName;
            if (!_client.ListDatabaseNames().ToList().Any(d => d == dbName))
            {
                return BadRequest("La base de données n'existe pas");
            }

            IMongoDatabase db = _client.GetDatabase(dbName);


            BsonDocument? user = db.GetCollection<BsonDocument>("users")
                .Find(d => d["username"].ToString() == username).FirstOrDefault();

            if(user == null || user["password"] != password)
            {
                return Unauthorized();
            }
            return Ok(new { 
                Token = _jwtManager.GenerateToken(username, dbName) 
            });
        }
    }
}
