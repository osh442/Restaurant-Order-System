namespace RestaurantOrderSystem.App.DTOs
{

    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string RestaurantName { get; set; } = null!;
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
    }


    public class OrderCreateDTO
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<OrderItemCreateDTO> Items { get; set; } = new List<OrderItemCreateDTO>();
    }


    public class OrderExportDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Username { get; set; } = null!;
        public string RestaurantName { get; set; } = null!;
        public List<string> Items { get; set; } = new List<string>();
    }
}