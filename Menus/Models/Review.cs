using System.ComponentModel.DataAnnotations;

namespace Menus.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}

