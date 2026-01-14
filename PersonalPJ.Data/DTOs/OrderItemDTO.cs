namespace RestaurantOrderSystem.Data.DTOs
{
 
    public class OrderItemDTO
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string MenuItemName { get; set; } = null!;
}


public class OrderItemCreateDTO
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
}
}