using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using UserForRegisterDto = Core.Entities.Concrete.UserForRegisterDto;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<UserForRegisterDto> UserAdd(Entity.Dtos.UserForRegisterDto userForRegisterDto);
        UserForRegisterDto GetUser(Core.Entities.Concrete.UserForRegisterDto user);
        UserForRegisterDto GetUserWithFirstName(UserForLoginDto userForLoginDto);
        UserForRegisterDto GetUserById(int userId);
        List<UserForRegisterDto> GetAll();
        IResult UserUpdate(Entity.Dtos.UserForRegisterDto user);
        List<OperationClaim> GetClaims(Entity.Dtos.UserForRegisterDto user);
        List<OperationClaim> GetOperationClaims();
    }
}
