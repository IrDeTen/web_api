using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using web_api.Models;

namespace web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsersController : ControllerBase
    {
        private List<User> users = new List<User>
        {
            new User {Name="internal", Password="pass", Internal=true},
            new User {Name="external", Password="pass2", Internal=false}
        };
        private ApplicationContext db;
        public UsersController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Create(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return new JsonResult(await db.Users.ToListAsync());
        }

        [HttpPost("auth")]
        public IActionResult Auth(User user)
        {
            var identify = GetIdentity(user);
            if (identify == null)
            {
                return BadRequest(new {errorText = "Invalid username or password"});
            }
            
            bool isInternal = users.FirstOrDefault( x=> x.Name == user.Name).Internal;


            var now = DateTime.UtcNow;
            DateTime exp;
            if (isInternal)
            {
               exp = now.AddHours(AuthOptions.LIFETIME); 
            } else 
            {
                exp = now.AddHours(AuthOptions.LIFETIME_EXT); 
            }

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identify.Claims,
                expires: exp,
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var result = new 
                {
                token = encodedJwt,
                expires = exp.ToLocalTime()
                };

            return new JsonResult(result);
        }

        private ClaimsIdentity GetIdentity(User signInUser)
        {
            //TODO добавить базу данных
            User user = users.FirstOrDefault( x=> x.Name == signInUser.Name && x.Password == signInUser.Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                };
                ClaimsIdentity claimsIden = 
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIden;
            }
            return null;
        }
    }

}


