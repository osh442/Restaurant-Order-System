namespace RestaurantOrderSystem.App.DTOs
{
 
    public class MenuItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string RestaurantName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }

    public class MenuItemImportDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string RestaurantName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }


    public class MenuItemCreateDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }
        public int CategoryId { get; set; }
    }
}