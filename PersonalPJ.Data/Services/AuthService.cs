using RestaurantOrderSystem.Data.Entities;
using RestaurantOrderSystem.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace RestaurantOrderSystem.Data.Services
{

    public class AuthService
    {
        private readonly RestaurantDbContext _context;

        public AuthService(RestaurantDbContext context)
        {
            _context = context;
        }

    
        public UserDTO? Register(UserRegisterDTO dto)
        {
            
            if (_context.Users.Any(u => u.Username == dto.Username))
            {
                return null;
            }

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password, 
                Role = "RegisteredUser",
                Status = "Active"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Status = user.Status
            };
        }

       
        public UserDTO? Login(UserLoginDTO dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == dto.Username && u.Password == dto.Password);

            if (user == null || user.Status == "Blocked")
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Status = user.Status
            };
        }

       
        public List<UserDTO> GetAllUsers()
        {
            return _context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = u.Role,
                    Status = u.Status
                })
                .ToList();
        }



        public bool BlockUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null || user.Role == "Administrator")
            {
                return false;
            }

            user.Status = "Blocked";
            _context.SaveChanges();
            return true;
        }


        public bool DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null || user.Role == "Administrator")
            {
                return false;
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}