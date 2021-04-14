using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Adutova_Lebedeva_Lab.Models
{
    public class AuthOptions
    {
        public static string Issuer => "Adutova_Lebedeva";// издатель токена
        public static string Audience => "APIclients";// потребитель токена
        public static int Lifetime => 1;// время жизни токена - 1 
        const string Key = "mysupersecret_secretkey!123";   // ключ для шифрации
        public static SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes("superSecretKeyMustBeLoooooong"));

        //internal static string GenerateToken(bool is_admin = false)
        //{
        //    var now = DateTime.UtcNow;
        //    var claims = new List<Claim>
        //        {
        //            new Claim(ClaimsIdentity.DefaultNameClaimType, "user"),
        //            new Claim(ClaimsIdentity.DefaultRoleClaimType, is_admin?"admin":"guest")
        //        };
        //    ClaimsIdentity identity =
        //    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
        //        ClaimsIdentity.DefaultRoleClaimType);
        //    // создаем JWT-токен
        //    var jwt = new JwtSecurityToken(
        //            issuer: Issuer,
        //            audience: Audience,
        //            notBefore: now,
        //            expires: now.AddYears(LifetimeInYears),
        //            claims: identity.Claims,
        //            signingCredentials: new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256)); ;
        //    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        //    return encodedJwt;
        //    return new { token = encodedJwt };
        //}
    }
}
