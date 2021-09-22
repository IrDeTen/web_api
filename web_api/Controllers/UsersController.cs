using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using Models.Response;
using web_api.Usecases;

namespace web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsersController : ControllerBase
    {
        private IAuthUC _authUC;
        public UsersController(IAuthUC authUC)
        {
            _authUC = authUC;
        }

        [HttpPost("sign_up")]
        public IActionResult SignUp(User user)
        {
            int id;
            string token;
            try {          
                (id, token) = _authUC.SignUp(user);
            } catch (UserAlreadyExist) {
                return BadRequest(new ErrorResponse("User already exist"));
            } 

            return Ok(new RegisterResponse(token, id));
        }

        [HttpPost("sign_in")]
        public IActionResult SignIn(User user)
        {   
            string token;
            try {
                token = _authUC.SignIn(user);
            } catch (UserNotFound) {
                return BadRequest(new ErrorResponse("User not found"));
            } catch (WrongPassword) {
                return BadRequest(new ErrorResponse("Wrong user password"));
            }
            return Ok(new AuthResponse(token) );
        }
    }
}


