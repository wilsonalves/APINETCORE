using System.Threading.Tasks;
using Api.Domain.Dtos;


namespace Api.Domain.Interfaces
{
    public interface ILoginService
    {
        Task<object> FindByLOgin(LoginDto user);
    }
}