using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using web_api.Repositories;
using web_api.Options;
using Models.Users;



namespace web_api.Usecases.AuthUC
{
  public class AuthUC : IAuthUC
  {
    private readonly IUserRepository _userRepo;

    public AuthUC(IUserRepository userRepo)
    {
      _userRepo = userRepo;
    }

    public (int, string) SignUp(User user)
    {      
      var existUser = GetUser(user.Login);
      if (existUser != null)
      {
        throw new UserAlreadyExist();
      }
      user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password);
      var id = _userRepo.CreateUser(user);
      var claims = UserIdentity(user);
      var jwtToken = CreateToken(user, claims);
      return (id, jwtToken);
    }

    public string SignIn(User user)
    {
      var existUser = GetUser(user.Login, user.Password);
      var claims = UserIdentity(user);
      var jwtToken = CreateToken(user, claims);
      return jwtToken;
    }

    private User GetUser(string login)
    {
      return _userRepo.GetUser(login);
    }

    public User GetUser(string login, string password)
    {
      var existUser = _userRepo.GetUser(login);
      if (existUser == null)
      {
        throw new UserNotFound();
      }
      if (!BCrypt.Net.BCrypt.EnhancedVerify(password,existUser.Password))
      {
        throw new WrongPassword();
      }
      return existUser;
    }

    private ClaimsIdentity UserIdentity(User user)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
      };

      return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }

    private string CreateToken(User user, ClaimsIdentity claims)
    {
      var now = DateTime.UtcNow;
      var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        notBefore: now,
        claims: claims.Claims,
        expires: now.AddMinutes(user.Internal ? AuthOptions.LIFETIME : AuthOptions.LIFETIME_EXT),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
      );
      return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
  }
}