using Business.Abstract;
using Business.Concrete;
using Entity.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "yonetici")]
    public class UserController : ControllerBase
    {
        IUserService _userManager;
        public UserController(IUserService userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("add")]
        public IActionResult UserAdd(UserForRegisterDto user)
        {
            try
            {
                var result=_userManager.UserAdd(user);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpGet("getall")]
        public IActionResult UserGetAll()
        {
            try
            {
                var result=_userManager.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
            
        }

        [HttpPost("update")]
        public IActionResult UserUpdate(UserForRegisterDto user)
        {
            try
            {
               var result= _userManager.UserUpdate(user);
                return Ok(result);

            }
            catch (Exception exp)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpPost("getclaims")]
        public IActionResult GetClaims(UserForRegisterDto user)
        {
            try
            {
                var result=_userManager.GetClaims(user);
                return Ok(result);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

    }
}
