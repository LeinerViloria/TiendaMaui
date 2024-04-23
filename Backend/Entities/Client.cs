using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    [Index("Email", IsUnique = true)]
    public class Client
    {
        [Key]
        public int Rowid { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public bool Status {get; set; }
    }
}
