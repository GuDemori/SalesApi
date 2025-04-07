using System.ComponentModel.DataAnnotations;

namespace SalesApi.Models
{
    public class Seller
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato inválido para email.")]
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
