namespace RestaurantOrderSystem.App.DTOs
{

    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class CategoryImportDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}