using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security;
using DataAccess.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        UserDal _userdal = new UserDal();

        public List<UserForRegisterDto> GetAll()
        {
            var result=_userdal.GetAll();
            return result;
        }

        public List<OperationClaim> GetClaims(Entity.Dtos.UserForRegisterDto user)
        {
            return _userdal.GetClaims(user);
        }

        public List<OperationClaim> GetOperationClaims()
        {
            return _userdal.GetOperationClaims();
        }

        public UserForRegisterDto GetUser(UserForRegisterDto user)
        {
            
            return _userdal.Get(u => u == user);
        }

        public Core.Entities.Concrete.UserForRegisterDto GetUserById(int userId)
        {
            return _userdal.Get(u => u.Id == userId);
        }

        public UserForRegisterDto GetUserWithFirstName(Entity.Dtos.UserForLoginDto userForLoginDto)
        {
            return _userdal.Get(u => u.UserFirstName == userForLoginDto.UserFirstName && u.Password==userForLoginDto.Password);
        }

        public IDataResult<UserForRegisterDto> UserAdd(Entity.Dtos.UserForRegisterDto userForRegisterDto)
        {
            try
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);
                var user = new UserForRegisterDto
                {
                    UserFirstName = userForRegisterDto.FirstName,
                    UserLastName = userForRegisterDto.LastName,
                    Password = userForRegisterDto.Password,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true
                };
                _userdal.Add(user);

                return new SuccessDataResult<Core.Entities.Concrete.UserForRegisterDto>(user, "Kullanıcı eklendi");
            }
            catch (Exception exp)
            {
                return new ErrorDataResult<Core.Entities.Concrete.UserForRegisterDto>("Bir hata oluştu \n" + exp);
            }            
        }

        public IResult UserUpdate(Entity.Dtos.UserForRegisterDto user)
        {
            try
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);
                var userUpdate = new UserForRegisterDto
                {
                    Id = user.Id,
                    UserFirstName = user.FirstName,
                    UserLastName = user.LastName,
                    Password = user.Password,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true
                };
                _userdal.Update(userUpdate);
                return new SuccessResult("Kullanıcı başarıyla güncellendi");
            }
            catch (Exception exp)
            {
                return new ErrorResult("Bir hata oluştu \n" + exp);
            }           
        }
    }
}
