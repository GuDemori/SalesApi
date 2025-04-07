using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApi.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O valor total é obrigatório.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "A data da transação é obrigatória.")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "O vendedor é obrigatório.")]
        public Guid SellerId { get; set; }

        [ForeignKey("SellerId")]
        public Seller? Seller { get; set; }
    }
}
