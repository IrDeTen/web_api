using Microsoft.IdentityModel.Tokens;
using System.Text;
 
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const double LIFETIME = 0.1; // время жизни токена внутреннего пользователя - 2 час
        public const int LIFETIME_EXT = 1; // время жизни токена внешнего пользователя - 1 часа
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
