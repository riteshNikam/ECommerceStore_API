using System.ComponentModel.DataAnnotations;

namespace ECommerceStore.DTO.User
{
    public class RegisterUserDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
