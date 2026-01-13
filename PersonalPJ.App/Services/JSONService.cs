using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestaurantOrderSystem.App.DTOs;
using RestaurantOrderSystem.Data;
using RestaurantOrderSystem.Data.Entities;
using System.Xml;

namespace RestaurantOrderSystem.App.Services
{

    public class JSONService
    {
        private readonly RestaurantDbContext _context;

        public JSONService(RestaurantDbContext context)
        {
            _context = context;
        }

        #region Import

        /// <summary>
        /// Импорт на ресторанти от JSON
        /// </summary>
        public (int success, int failed) ImportRestaurants(string jsonPath)
        {
            try
            {
                string jsonContent = File.ReadAllText(jsonPath);
                var restaurants = JsonConvert.DeserializeObject<List<RestaurantImportDTO>>(jsonContent);

                if (restaurants == null || restaurants.Count == 0)
                {
                    return (0, 0);
                }

                int successCount = 0;
                int failedCount = 0;

                foreach (var dto in restaurants)
                {
                    // Валидация
                    if (string.IsNullOrWhiteSpace(dto.Name) ||
                        string.IsNullOrWhiteSpace(dto.Address))
                    {
                        failedCount++;
                        continue;
                    }

                    var restaurant = new Restaurant
                    {
                        Name = dto.Name,
                        Address = dto.Address,
                        Phone = dto.Phone ?? string.Empty,
                        Email = dto.Email ?? string.Empty
                    };

                    _context.Restaurants.Add(restaurant);
                    successCount++;
                }

                _context.SaveChanges();
                return (successCount, failedCount);
            }
            catch
            {
                return (0, 0);
            }
        }

        /// <summary>
        /// Импорт на меню позиции от JSON
        /// </summary>
        public (int success, int failed) ImportMenuItems(string jsonPath)
        {
            try
            {
                string jsonContent = File.ReadAllText(jsonPath);
                var menuItems = JsonConvert.DeserializeObject<List<MenuItemImportDTO>>(jsonContent);

                if (menuItems == null || menuItems.Count == 0)
                {
                    return (0, 0);
                }

                int successCount = 0;
                int failedCount = 0;

                foreach (var dto in menuItems)
                {
                    // Валидация
                    if (string.IsNullOrWhiteSpace(dto.Name) ||
                        dto.Price <= 0 ||
                        string.IsNullOrWhiteSpace(dto.RestaurantName) ||
                        string.IsNullOrWhiteSpace(dto.CategoryName))
                    {
                        failedCount++;
                        continue;
                    }

                    // Намиране на ресторант и категория
                    var restaurant = _context.Restaurants
                        .FirstOrDefault(r => r.Name == dto.RestaurantName);
                    var category = _context.Categories
                        .FirstOrDefault(c => c.Name == dto.CategoryName);

                    if (restaurant == null || category == null)
                    {
                        failedCount++;
                        continue;
                    }

                    var menuItem = new MenuItem
                    {
                        Name = dto.Name,
                        Description = dto.Description ?? string.Empty,
                        Price = dto.Price,
                        IsAvailable = dto.IsAvailable,
                        RestaurantId = restaurant.Id,
                        CategoryId = category.Id
                    };

                    _context.MenuItems.Add(menuItem);
                    successCount++;
                }

                _context.SaveChanges();
                return (successCount, failedCount);
            }
            catch
            {
                return (0, 0);
            }
        }

        #endregion

        #region Export

     
        public bool ExportAllOrders(string outputPath)
        {
            try
            {
                var orders = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Restaurant)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .Select(o => new OrderExportDTO
                    {
                        OrderId = o.Id,
                        OrderDate = o.OrderDate,
                        Status = o.Status,
                        TotalAmount = o.TotalAmount,
                        Username = o.User.Username,
                        RestaurantName = o.Restaurant.Name,
                        Items = o.OrderItems.Select(oi =>
                            $"{oi.MenuItem.Name} x{oi.Quantity} - {oi.Price:F2} lv").ToList()
                    })
                    .ToList();

                string json = JsonConvert.SerializeObject(orders, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(outputPath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

     
        public bool ExportActiveOrders(string outputPath)
        {
            try
            {
                var orders = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Restaurant)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .Where(o => o.Status == "Pending" || o.Status == "Confirmed")
                    .Select(o => new OrderExportDTO
                    {
                        OrderId = o.Id,
                        OrderDate = o.OrderDate,
                        Status = o.Status,
                        TotalAmount = o.TotalAmount,
                        Username = o.User.Username,
                        RestaurantName = o.Restaurant.Name,
                        Items = o.OrderItems.Select(oi =>
                            $"{oi.MenuItem.Name} x{oi.Quantity} - {oi.Price:F2} lv").ToList()
                    })
                    .ToList();

                string json = JsonConvert.SerializeObject(orders, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(outputPath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ExportMenuItems(string outputPath)
        {
            try
            {
                var menuItems = _context.MenuItems
                    .Include(m => m.Restaurant)
                    .Include(m => m.Category)
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

                string json = JsonConvert.SerializeObject(menuItems, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(outputPath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}