using Microsoft.IdentityModel.Tokens;
using System.Text;
 
namespace web_api.Options
{
    public class AuthOptions
    {
        public const string ISSUER = "server"; // издатель токена
        public const string AUDIENCE = "client"; // потребитель токена
        const string KEY = "secretkey123Qwert123";   // ключ для шифрации
        public const int LIFETIME = 120; // время жизни токена внутреннего пользователя - 2 час
        public const int LIFETIME_EXT = 60; // время жизни токена внешнего пользователя - 1 часа
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}