using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security;
using Core.Utilities.Security.Jwt;
using Entity;
using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using UserForRegisterDto = Entity.Dtos.UserForRegisterDto;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<Core.Entities.Concrete.UserForRegisterDto> Register(UserForRegisterDto userForRegisterDto)
        {
            return _userService.UserAdd(userForRegisterDto);
        }

        public IDataResult<Core.Entities.Concrete.UserForRegisterDto> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetUserWithFirstName(userForLoginDto);
            if (userToCheck == null)
            {
                return new ErrorDataResult<Core.Entities.Concrete.UserForRegisterDto>("Kullanıcı buluınamadı");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<Core.Entities.Concrete.UserForRegisterDto>("Şifreniz eşleşmedi");
            }

            return new SuccessDataResult<Core.Entities.Concrete.UserForRegisterDto>(userToCheck, "Giriş başarılı");
        }

        public IResult UserExists(UserForLoginDto userForLoginDto)
        {
            if (_userService.GetUserWithFirstName(userForLoginDto) != null)
            {
                return new ErrorResult("Kullanıcı zaten kayıtlı");
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(Core.Entities.Concrete.UserForRegisterDto user)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);
            var userClaim = new UserForRegisterDto()
            {
                FirstName = user.UserFirstName,
                LastName = user.UserLastName,
                Password = user.Password,
                Id = user.Id
            };
            var claims = _userService.GetClaims(userClaim);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, "Token üretildi");
        }

        public IResult DecodeToken(TokenDecode token)
        {
            var tokenDecode = new JwtSecurityToken(jwtEncodedString: token.Token);
            var resultString = tokenDecode.Claims.First(c => c.Type == "FirstName").Value;
            if (resultString != null)
                return new SuccessResult(resultString);
            return new ErrorResult("Token geçerli değil");
        }

        public IDataResult<List<Authority>> GetUserAuthorities()
        {
            try
            {
                List<Authority> authorities = new List<Authority>();
                List<Core.Entities.Concrete.UserForRegisterDto> users = _userService.GetAll();
                foreach (var user in users)
                {
                    UserForRegisterDto userFor = new UserForRegisterDto()
                    {
                        Id = user.Id,
                        FirstName = user.UserFirstName,
                        LastName = user.UserLastName,
                        Password = user.Password
                    };
                    List<OperationClaim> userClaims = _userService.GetClaims(userFor);
                    foreach (var claim in userClaims)
                    {
                        if (authorities.Find(x => x.AuthorityName == claim.Name) == null)
                        {
                            Authority authority = new Authority() { AuthorityCount=1,AuthorityName=claim.Name};
                            authorities.Add(authority);
                        }
                        else
                        {
                            foreach (var authority in authorities)
                            {
                                if (authority.AuthorityName == claim.Name)
                                {
                                    authority.AuthorityCount++;
                                }                                
                            }
                        }

                    }
                }
                return new SuccessDataResult<List<Authority>>(authorities);
            }
            catch (Exception exp)
            {
                return new ErrorDataResult<List<Authority>>(exp.Message);
            }
        }
    }
}
