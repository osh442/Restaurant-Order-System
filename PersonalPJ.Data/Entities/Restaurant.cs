using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderSystem.Data.Entities
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(200)]
        public string Address { get; set; } = null!;

        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        [MaxLength(100)]
        public string Email { get; set; } = null!;

        // Navigation properties
        public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}