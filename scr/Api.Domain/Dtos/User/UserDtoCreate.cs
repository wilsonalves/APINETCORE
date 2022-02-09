using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos.User
{
    public class UserDtoCreate
    {
        [Required(ErrorMessage = "Nome campo é obrigatorio")]
        [StringLength(60, ErrorMessage = "Nome deve ter no maximo {1} caracteres")]
        public string Name { get; set; }


        [Required(ErrorMessage = " Email é um campo obrigatório para o login")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        [StringLength(100, ErrorMessage = " Email tamanho maximo ")]
        public string Email { get; set; }
    }
}