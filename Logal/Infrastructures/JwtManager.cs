using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Logal.Infrastructures
{
    public class JwtManager
    {
        private JwtSecurityTokenHandler _tokenHandler;

        public JwtManager(JwtSecurityTokenHandler tokenHandler)
        {
            _tokenHandler = tokenHandler;
        }


        public string GenerateToken(string username, string client)
        {
            JwtSecurityToken token = new JwtSecurityToken(
                "Logal",
                "React",
                new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Role, client),
                },
                DateTime.Now,
                DateTime.Now.AddDays(1),
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ldjfghjkdfghdjlfkfqsdqsjfsmdjkgfmdskgjf")),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return _tokenHandler.WriteToken(token);
        }
    }
}
