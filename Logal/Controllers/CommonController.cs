using Logal.DTO;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;

namespace Logal.Controllers
{
    public abstract class CommonController : ControllerBase
    {
        protected MongoClient _mongoClient;

        protected CommonController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet("Method1")]
        public virtual IActionResult Method1()
        {
            string? client = User.FindFirstValue(ClaimTypes.Role);

            if(client is null)
            {
                return BadRequest();
            }

            IMongoDatabase db = _mongoClient.GetDatabase(client);
            IMongoCollection<BsonDocument> c = db.GetCollection<BsonDocument>("movies");

            IEnumerable<MovieDTO> movies = c.Find((d) => true).Limit(100).ToList().Select(d => new MovieDTO
            {
                _id = d["_id"].ToString(),
                title = (string)d["title"],
                rating = (int)d["rating"],
                verrou = (string?)d.GetValue("lock", null),
            });

            return Ok(movies);
        }

        [HttpPut("Method2/{id}")]
        public virtual IActionResult Method2([FromRoute] string id, [FromBody]MovieDTO movie)
        {
            string? client = User.FindFirstValue(ClaimTypes.Role);

            if (client is null)
            {
                return BadRequest();
            }

            var builder = Builders<BsonDocument>.Update
                .Set("title", movie.title)
                .Set("rating", movie.rating);

            IMongoDatabase db = _mongoClient.GetDatabase(client);
            IMongoCollection<BsonDocument> c = db.GetCollection<BsonDocument>("movies");

            c.FindOneAndUpdate(d => d["_id"] == new ObjectId(id), builder);
            
            return NoContent();
        }


    }
}
