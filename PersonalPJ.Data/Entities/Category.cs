using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderSystem.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(200)]
        public string Description { get; set; } = null!;

        public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}