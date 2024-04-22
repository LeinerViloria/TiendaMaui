using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    public class Purchase
    {
        [Key]
        public int Rowid { get; set; }
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        [ForeignKey("Client")]
        [Required]
        public int RowidCliente { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int RowidProducto { get; set; }
        [Required]
        public int Cantidad { get; set; }
        public Client? Client { get; set; }
        public Product? Product { get; set; }
    }
}
