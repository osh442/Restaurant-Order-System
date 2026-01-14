using RestaurantOrderSystem.Data.Entities;
using RestaurantOrderSystem.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace RestaurantOrderSystem.Data.Services
{
  
    public class OrderService
    {
        private readonly RestaurantDbContext _context;

        public OrderService(RestaurantDbContext context)
        {
            _context = context;
        }

        public OrderDTO? CreateOrder(OrderCreateDTO dto)
        {
            
            if (!_context.Users.Any(u => u.Id == dto.UserId) ||
                !_context.Restaurants.Any(r => r.Id == dto.RestaurantId))
            {
                return null;
            }

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            
            foreach (var item in dto.Items)
            {
                var menuItem = _context.MenuItems.Find(item.MenuItemId);
                if (menuItem == null || !menuItem.IsAvailable)
                {
                    continue;
                }

                var orderItem = new OrderItem
                {
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    Price = menuItem.Price
                };

                totalAmount += menuItem.Price * item.Quantity;
                orderItems.Add(orderItem);
            }

            if (orderItems.Count == 0)
            {
                return null;
            }

            
            var order = new Order
            {
                UserId = dto.UserId,
                RestaurantId = dto.RestaurantId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = totalAmount,
                Notes = dto.Notes,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return GetOrderById(order.Id);
        }

        public OrderDTO? GetOrderById(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return null;
            }

            return new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
                Username = order.User.Username,
                RestaurantName = order.Restaurant.Name,
                Items = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    MenuItemName = oi.MenuItem.Name
                }).ToList()
            };
        }

        public List<OrderDTO> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Select(o => new OrderDTO
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    Notes = o.Notes,
                    Username = o.User.Username,
                    RestaurantName = o.Restaurant.Name,
                    Items = o.OrderItems.Select(oi => new OrderItemDTO
                    {
                        Id = oi.Id,
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                        MenuItemName = oi.MenuItem.Name
                    }).ToList()
                })
                .ToList();
        }

    
        public List<OrderDTO> GetOrdersByUserId(int userId)
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Where(o => o.UserId == userId)
                .Select(o => new OrderDTO
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    Notes = o.Notes,
                    Username = o.User.Username,
                    RestaurantName = o.Restaurant.Name,
                    Items = o.OrderItems.Select(oi => new OrderItemDTO
                    {
                        Id = oi.Id,
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                        MenuItemName = oi.MenuItem.Name
                    }).ToList()
                })
                .ToList();
        }


        public bool UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
            {
                return false;
            }

            order.Status = newStatus;
            _context.SaveChanges();
            return true;
        }

   
        public bool DeleteOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return true;
        }
    }
}