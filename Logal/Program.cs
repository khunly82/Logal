
using Logal.Converters;
using Logal.Facades;
using Logal.Hubs;
using Logal.Infrastructures;
using Logal.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using SelectPdf;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(o =>
{
    o.Filters.Add<LogFilter>();
}).AddJsonOptions(o => {
    o.JsonSerializerOptions.Converters.Add(new DynamicConverter());
    o.JsonSerializerOptions.PropertyNamingPolicy = null;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(b => new SmtpClient()
{
    Host = builder.Configuration["Smtp:Host"] 
        ?? throw new Exception("Host configuration is missing"),
    Port = int.Parse(builder.Configuration["Smtp:Port"] 
        ?? throw new Exception("Port Configuration is missing")),
    Credentials = new NetworkCredential
    {
        UserName = builder.Configuration["Smtp:Username"],
        Password = builder.Configuration["Smtp:Password"],
    }

});
builder.Services.AddScoped<IMailer, Mailer>();

builder.Services.AddScoped(b => new MongoClient(builder.Configuration["MongoDb:url"]));

builder.Services.AddScoped<JwtManager>();
builder.Services.AddScoped<JwtSecurityTokenHandler>();

builder.Services.AddCors(b => b.AddDefaultPolicy(o =>
{
    o.AllowAnyMethod();
    o.AllowAnyHeader();
    o.AllowCredentials();
    o.WithOrigins("http://localhost:5174");
}));

builder.Services.AddScoped<HtmlRenderer>();
builder.Services.AddScoped<HtmlToPdf>();

// builder.Services.AddSession();


builder.Services.AddAuthentication(
    o => o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme
).AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ldjfghjkdfghdjlfkfqsdqsjfsmdjkgfmdskgjf"))
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseSession();

app.UseCors();

app.UseAuthentication();

app.Use((context, next) =>
{
    // access_token
    string? token = context.Request.Query["access_token"];

    if(token != null)
    {
        ClaimsPrincipal claims = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ldjfghjkdfghdjlfkfqsdqsjfsmdjkgfmdskgjf"))
        }, out SecurityToken st);

        context.User = claims;
    }

    return next();
});

app.UseAuthorization();

app.MapControllers();

app.MapHub<MessageHub>("ws/message", o =>
{
    o.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
});

app.Run();
