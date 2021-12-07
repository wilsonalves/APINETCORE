using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Domain.Repository;
using Api.Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Service.Services
{
    public class LoginService : ILoginService
    {

        private SigningConfigurations _signingConfigurations;
        private TokenConfiguration _tokenConfiguration;
        private IUserRepository _repository;
        public LoginService(IConfiguration _configuration)
        {
            this._configuration = _configuration;

        }

        private IConfiguration _configuration { get; }
        public LoginService(IUserRepository repository, SigningConfigurations signingConfigurations,
         TokenConfiguration tokenConfiguration, IConfiguration iconfiguration)
        {
            _repository = repository;
            _signingConfigurations = signingConfigurations;
            _tokenConfiguration = tokenConfiguration;
            _configuration = iconfiguration;
        }

        public async Task<object> FindByLOgin(LoginDto user)
        {
            var baseUser = new UserEntity();
            if (user != null && !string.IsNullOrWhiteSpace(user.email))
            {
                baseUser = await _repository.FindByLogin(user.email);
                if (baseUser == null)
                {
                    return new
                    {
                        authenticated = false,
                        message = "Falha ao autenticar"
                    };
                }
                else
                {
                    var identity = new ClaimsIdentity(
                         new System.Security.Principal.GenericIdentity(user.email),
                         new[]{
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.email)

                         }
                     );

                    DateTime createDate = DateTime.Now;
                    DateTime expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfiguration.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    string token = CreateToken(identity, createDate, expirationDate, handler);
                    return SucessObject(createDate, expirationDate, token, user);

                }



            }
            else
            {
                return null;

            }


        }
        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });
            var token = handler.WriteToken(securityToken);
            return token;

        }

        private object SucessObject(DateTime createDate, DateTime expirationDate, string token, LoginDto user)
        {
            return new
            {
                authenticated = true,
                create = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                acessToken = token,
                UserName = user.email,
                message = " usuario logado com sucesso."
            };

        }

    }
}




