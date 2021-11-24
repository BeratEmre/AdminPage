using Core.Entities.Concrete;
using Core.Extencions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        TokenOptions _tokenOptions=new TokenOptions() ;
        DateTime _accesTokenExpiration;
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            var token =
            _tokenOptions.Audience = Configuration.GetSection("TokenOptions:Audience").Value;
            _tokenOptions.Issuer = Configuration.GetSection("TokenOptions:Issuer").Value;
            _tokenOptions.SecurityKey = Configuration.GetSection("TokenOptions:SecurityKey").Value;
            _tokenOptions.AccessTokenExpiretion = Convert.ToInt32(Configuration.GetSection("TokenOptions:AccessTokenExpiration").Value);
            _accesTokenExpiration = DateTime.Now.AddDays(_tokenOptions.AccessTokenExpiretion);

        }
        public AccessToken CreateToken(UserForRegisterDto user, List<OperationClaim> operationClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
            var signingCredentials=new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var jwt = CreateJwtToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);
            return new AccessToken
            {
                Token = token,
                Expiration = _accesTokenExpiration,                
            };
        }
        public JwtSecurityToken CreateJwtToken(TokenOptions tokenOptions,UserForRegisterDto user,SigningCredentials signingCredentials,List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: _accesTokenExpiration,
                notBefore:DateTime.Now,
                claims: SetClaims(user,operationClaims),
                signingCredentials: signingCredentials
                );
            return jwt;
        }
        private IEnumerable<Claim> SetClaims(UserForRegisterDto user,List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim("FirstName", user.UserFirstName));
            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
            return claims;
        }
       
    }
}
