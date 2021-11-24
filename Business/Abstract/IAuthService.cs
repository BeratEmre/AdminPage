using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entity;
using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using UserForRegisterDto = Core.Entities.Concrete.UserForRegisterDto;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<UserForRegisterDto> Register(Entity.Dtos.UserForRegisterDto userForRegisterDto);
        IDataResult<UserForRegisterDto> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(UserForLoginDto userForLoginDto);
        IResult DecodeToken(TokenDecode token);
        IDataResult<AccessToken> CreateAccessToken(Core.Entities.Concrete.UserForRegisterDto user);
        IDataResult<List<Authority>> GetUserAuthorities();
    }
}
