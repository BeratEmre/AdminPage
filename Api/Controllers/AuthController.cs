using Business.Abstract;
using Core.Entities.Concrete;
using Entity;
using Entity.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto ForLoginDto)
        {
            var userToLogin = _authService.Login(ForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("register")]
        public ActionResult Register(Entity.Dtos.UserForRegisterDto userForRegisterDto)
        {
            UserForLoginDto login = new UserForLoginDto()
            {
                Password= userForRegisterDto.Password,
                UserFirstName=userForRegisterDto.FirstName,
            };
            var userexists = _authService.UserExists(login);
            if (!userexists.Success)
            {
                return BadRequest(userexists.Message);
            }

            var registerResult = _authService.Register(userForRegisterDto);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("decodetoken")]
        public ActionResult DecodeTokenForName(TokenDecode token)
        {
            var result=_authService.DecodeToken(token);
            if (result.Success)         
            return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetUserAuthorities")]
        public ActionResult GetUserAuthorities( )
        {
            var result = _authService.GetUserAuthorities();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}