using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Domain.Interfaces
{
    public interface ILoginService
    {
        Task<object> FindByLOgin(UserEntity user);
    }
}