using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderSystem.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = null!; 

        [MaxLength(20)]
        public string Status { get; set; } = "Active"; 
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}