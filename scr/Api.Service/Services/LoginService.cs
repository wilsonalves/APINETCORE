using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Domain.Repository;

namespace Api.Service.Services
{
    public class LoginService : ILoginService
    {

        private IUserRepository _repository;
        public LoginService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> FindByLOgin(UserEntity user)
        {
            var baseUser = new UserEntity();
            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {
                return await _repository.FindByLogin(user.Email);
            }
            else
            {
                return null;
            }


        }
    }
}