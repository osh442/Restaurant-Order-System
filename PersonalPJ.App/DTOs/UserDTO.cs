namespace RestaurantOrderSystem.App.DTOs
{
   
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

   
    public class UserRegisterDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

 
    public class UserLoginDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}