using RestaurantOrderSystem.Data;
using RestaurantOrderSystem.App.DTOs;
using Microsoft.EntityFrameworkCore;

namespace RestaurantOrderSystem.App.Services
{

    public class QueryService
    {
        private readonly RestaurantDbContext _context;

        public QueryService(RestaurantDbContext context)
        {
            _context = context;
        }

     
        public List<MenuItemDTO> GetTop10MostExpensiveItems()
        {
            return _context.MenuItems
                .Include(m => m.Restaurant)
                .Include(m => m.Category)
                .OrderByDescending(m => m.Price)
                .Take(10)
                .Select(m => new MenuItemDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    IsAvailable = m.IsAvailable,
                    RestaurantName = m.Restaurant.Name,
                    CategoryName = m.Category.Name
                })
                .ToList();
        }

      
        public List<OrderDTO> GetActiveOrders()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Where(o => o.Status == "Pending" || o.Status == "Confirmed")
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

     
        public List<(string RestaurantName, int OrderCount)> GetOrderCountByRestaurant()
        {
            return _context.Orders
                .Include(o => o.Restaurant)
                .GroupBy(o => o.Restaurant.Name)
                .Select(g => new
                {
                    RestaurantName = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .ToList()
                .Select(x => (x.RestaurantName, x.OrderCount))
                .ToList();
        }

    
        public List<(string RestaurantName, decimal TotalRevenue)> GetTotalRevenueByRestaurant()
        {
            return _context.Orders
                .Include(o => o.Restaurant)
                .Where(o => o.Status == "Completed")
                .GroupBy(o => o.Restaurant.Name)
                .Select(g => new
                {
                    RestaurantName = g.Key,
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToList()
                .Select(x => (x.RestaurantName, x.TotalRevenue))
                .ToList();
        }

        public List<(string MenuItemName, int OrderCount)> GetMostPopularMenuItems()
        {
            return _context.OrderItems
                .Include(oi => oi.MenuItem)
                .GroupBy(oi => oi.MenuItem.Name)
                .Select(g => new
                {
                    MenuItemName = g.Key,
                    OrderCount = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(x => x.OrderCount)
                .Take(10)
                .ToList()
                .Select(x => (x.MenuItemName, x.OrderCount))
                .ToList();
        }

   
        public decimal GetAverageOrderValue()
        {
            if (!_context.Orders.Any())
            {
                return 0;
            }

            return _context.Orders.Average(o => o.TotalAmount);
        }

     
        public List<OrderDTO> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
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

     
        public List<MenuItemDTO> GetMenuItemsByCategory(string categoryName)
        {
            return _context.MenuItems
                .Include(m => m.Restaurant)
                .Include(m => m.Category)
                .Where(m => m.Category.Name == categoryName)
                .Select(m => new MenuItemDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    IsAvailable = m.IsAvailable,
                    RestaurantName = m.Restaurant.Name,
                    CategoryName = m.Category.Name
                })
                .ToList();
        }


        public List<(string Role, int UserCount)> GetUserCountByRole()
        {
            return _context.Users
                .GroupBy(u => u.Role)
                .Select(g => new
                {
                    Role = g.Key,
                    UserCount = g.Count()
                })
                .ToList()
                .Select(x => (x.Role, x.UserCount))
                .ToList();
        }
    }
}