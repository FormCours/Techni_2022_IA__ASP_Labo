using EventManager.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManager.WebAPI.Helpers
{
    public class TokenService
    {
        IConfiguration _Configuration;

        public TokenService(IConfiguration configuration)
        {
            _Configuration = configuration;
        }


        // La génération d'un JWT necessite le NuGet package : System.IdentityModel.Tokens.Jwt 
        public string GenerateJwt(Member member)
        {
            // Création de la signature du Jwt sur base d'une clef secret
            byte[] key = Encoding.UTF8.GetBytes(_Configuration["Token:Secret"]);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            // Création d'un objet de sécurité avec les données necessaire au token
            // Attention: Ne pas stocker d'information sensible !
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                new Claim(ClaimTypes.Name, member.Pseudo)
            };

            // Création du token
            // Alternative: Utiliser le "SecurityTokenDescriptor"
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _Configuration["Token:Issuer"],
                audience: _Configuration["Token:Audience"],
                expires: DateTime.Now.AddHours(12),
                claims: claims,
                signingCredentials: credentials
            );

            // Génération sous forme de chaine de caractere 
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
