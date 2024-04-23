using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Product
    {
        [Key]
        public int Rowid { get; set; }
        [Required]
        [StringLength(80)]
        public string Name { get; set; } = null!;
        [Required]
        public int Stock { get; set; }
        public string? RutaImagen { get; set; }
        [Required]
        public double Precio { get; set; }
    }
}
