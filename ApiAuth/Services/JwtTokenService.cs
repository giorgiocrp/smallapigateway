using ApiAuth.Model;
using JwtTokenAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationToken = ApiAuth.Model.AuthenticationToken;

namespace ApiAuth.Services
{
    public class JwtTokenService
    {
        private readonly List<User> _users = new()
        {
            new User(){ UserName="admin", Password="aDm1n", Role="Administrator", Scopes=new[] {"shoes.read"} },
            new User(){ UserName="user01", Password="u$3r01", Role="User", Scopes=new[] {"shoes.read"} }
        };

        public AuthenticationToken? GenerateAuthToken(LoginModel loginModel)
        {
            var user = _users.FirstOrDefault(u => u.UserName == loginModel.UserName
                                               && u.Password == loginModel.Password);

            if (user is null)
            {
                return null;
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtExtensions.SecurityKey));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expirationTimeStamp = DateTime.Now.AddMinutes(5);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim("role", user.Role),
                new Claim("scope", string.Join(" ", user.Scopes))
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:5002",
                claims: claims,
                expires: expirationTimeStamp,
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new AuthenticationToken(tokenString, (int)expirationTimeStamp.Subtract(DateTime.Now).TotalSeconds);
        }
    }
}
