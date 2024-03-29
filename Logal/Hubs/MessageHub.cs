using Logal.Forms;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;

namespace Logal.Hubs
{
    public class MessageHub : Hub
    {
        private MongoClient _mongoClient;

        public MessageHub(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public void Send(MessageForm form)
        {
            Clients.Others.SendAsync("newMessage", form.Message);
        }

        public void Update(UpdateItemForm form)
        {
            string? client = Context.User?.FindFirstValue(ClaimTypes.Role);
            string? username = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (client is null)
            {
                return;
            }

            IMongoDatabase db = _mongoClient.GetDatabase(client);
            IMongoCollection<BsonDocument> c = db.GetCollection<BsonDocument>(form.Collection);

            UpdateDefinition<BsonDocument> builder;

            if(form.IsUpdating)
            {
                builder = Builders<BsonDocument>.Update.Set("lock", username);

            } else
            {
                builder = Builders<BsonDocument>.Update.Unset("lock");
            }


            c.FindOneAndUpdate(d => d["_id"] == new ObjectId(form.UpdatedItem), builder);

            Clients.Group(client).SendAsync("lock_" + form.Collection, form);
        }

        public async override Task OnConnectedAsync()
        {
            string? client = Context.User?.FindFirstValue(ClaimTypes.Role);

            if (client is null)
            {
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, client);
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            string? client = Context.User?.FindFirstValue(ClaimTypes.Role);
            string? username = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (client is null)
            {
                return;
            }
            IMongoDatabase db = _mongoClient.GetDatabase(client);
            foreach(string colName in db.ListCollectionNames().ToList())
            {
                var c = db.GetCollection<BsonDocument>(colName);
                List<BsonDocument> items = c.Find(d => d["lock"] == username)
                    .ToList();
                foreach(BsonDocument item in items)
                {
                    UpdateDefinition<BsonDocument> builders = Builders<BsonDocument>.Update.Unset("lock");
                    c.UpdateOne(item, builders);
                    await Clients.Group(client).SendAsync("lock_" + colName, new { 
                        UpdatedItem = item["_id"].ToString(), 
                        IsUpdating = false 
                    });
                }
            }
        }
    }
}
