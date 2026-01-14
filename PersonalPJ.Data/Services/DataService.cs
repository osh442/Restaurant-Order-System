using RestaurantOrderSystem.Data.Entities;
using RestaurantOrderSystem.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace RestaurantOrderSystem.Data.Services
{

    public class DataService
    {
        private readonly RestaurantDbContext _context;

        public DataService(RestaurantDbContext context)
        {
            _context = context;
        }

        #region Restaurants

        /// Показване на всички ресторанти
        public List<RestaurantDTO> GetAllRestaurants()
        {
            return _context.Restaurants
                .Select(r => new RestaurantDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    Phone = r.Phone,
                    Email = r.Email
                })
                .ToList();
        }

        /// Добавяне на ресторант
        public RestaurantDTO AddRestaurant(RestaurantImportDTO dto)
        {
            var restaurant = new Restaurant
            {
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email
            };

            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();

            return new RestaurantDTO
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                Phone = restaurant.Phone,
                Email = restaurant.Email
            };
        }

        #endregion

        #region Categories

        /// Показване на всички категории
        public List<CategoryDTO> GetAllCategories()
        {
            return _context.Categories
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToList();
        }

        /// Добавяне на категория
        public CategoryDTO AddCategory(CategoryImportDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        #endregion

        #region MenuItems

        /// Показване на всички меню позиции
        public List<MenuItemDTO> GetAllMenuItems()
        {
            return _context.MenuItems
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
        }

        /// Показване на налични меню позиции
        public List<MenuItemDTO> GetAvailableMenuItems()
        {
            return _context.MenuItems
                .Include(m => m.Restaurant)
                .Include(m => m.Category)
                .Where(m => m.IsAvailable)
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

        /// Търсене на меню позиции по име
        public List<MenuItemDTO> SearchMenuItems(string searchTerm)
        {
            return _context.MenuItems
                .Include(m => m.Restaurant)
                .Include(m => m.Category)
                .Where(m => m.Name.Contains(searchTerm) || m.Description.Contains(searchTerm))
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

        /// Добавяне на меню позиция
        public MenuItemDTO? AddMenuItem(MenuItemCreateDTO dto)
        {
            // Проверка дали ресторантът и категорията съществуват
            if (!_context.Restaurants.Any(r => r.Id == dto.RestaurantId) ||
                !_context.Categories.Any(c => c.Id == dto.CategoryId))
            {
                return null;
            }

            var menuItem = new MenuItem
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                RestaurantId = dto.RestaurantId,
                CategoryId = dto.CategoryId,
                IsAvailable = true
            };

            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();

            var restaurant = _context.Restaurants.Find(dto.RestaurantId);
            var category = _context.Categories.Find(dto.CategoryId);

            return new MenuItemDTO
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                RestaurantName = restaurant!.Name,
                CategoryName = category!.Name
            };
        }

        #endregion
    }
}