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
        private ApplicationContext db;
        public UsersController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Create(User user)
        {
            var err = CheckUser(user);
            if (err == null)
            {
                return BadRequest(new {Status = "error", Error = "User already exist"});
            }

            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password);
            db.Users.Add(user);
            await db.SaveChangesAsync();
            user = db.Users.FirstOrDefault(u => u.Name == user.Name);
            
            var jwt = FormJWTToken(user);

            return new JsonResult(new {id = user.ID, token = jwt} );
        }

        [HttpPost("auth")]
        public IActionResult Auth(User user)
        {   
            var err = CheckUser(user);
            if (err != null)
            {
                return BadRequest(err);
            } 
            var jwt = FormJWTToken(user);

            return Ok(new {token = jwt} );
        }

        private object CheckUser(User user)
        {
            var existUser = db.Users.FirstOrDefault(u => u.Name == user.Name);
            if (existUser == null)
            {
                return new { Status = "error", Error = "User not found"};
            }
            if (!BCrypt.Net.BCrypt.EnhancedVerify(user.Password,existUser.Password))
            {
                return new { Status = "error", Error = "Wrong password"};
            }
            return null;
        }

        private string FormJWTToken(User user)
        {            
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
            };

            ClaimsIdentity claimsIden = 
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            bool isInternal = db.Users.FirstOrDefault( x=> x.Name == user.Name).Internal;
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claimsIden.Claims,
                expires: now.AddMinutes(isInternal ? AuthOptions.LIFETIME : AuthOptions.LIFETIME_EXT),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }

}


