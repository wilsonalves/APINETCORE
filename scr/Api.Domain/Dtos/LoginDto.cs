using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = " Email é um campo obrigatório para o login")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        [StringLength(100, ErrorMessage = " Email tamanho maximo ")]
        public string email { get; set; }
    }
}