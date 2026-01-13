using RestaurantOrderSystem.App.DTOs;
using RestaurantOrderSystem.App.Utilities;

namespace RestaurantOrderSystem.App.Utilities
{

    public static class UIHelper
    {
        public static void PrintHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ {title.PadRight(57)} ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {message}");
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✗ {message}");
            Console.ResetColor();
        }

        public static void PrintInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"ℹ {message}");
            Console.ResetColor();
        }

        public static void PrintRestaurants(List<RestaurantDTO> restaurants)
        {
            if (restaurants.Count == 0)
            {
                PrintInfo("Няма намерени ресторанти.");
                return;
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      РЕСТОРАНТИ                            ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");

            foreach (var restaurant in restaurants)
            {
                Console.WriteLine($"║ ID: {restaurant.Id,-3} │ {restaurant.Name,-45} ║");
                Console.WriteLine($"║ Адрес: {restaurant.Address,-48} ║");
                Console.WriteLine($"║ Телефон: {restaurant.Phone,-15} │ Email: {restaurant.Email,-23} ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            }
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }

        public static void PrintCategories(List<CategoryDTO> categories)
        {
            if (categories.Count == 0)
            {
                PrintInfo("Няма намерени категории.");
                return;
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      КАТЕГОРИИ                             ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");

            foreach (var category in categories)
            {
                Console.WriteLine($"║ ID: {category.Id,-3} │ {category.Name,-48} ║");
                Console.WriteLine($"║ {category.Description,-56} ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            }
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }

        public static void PrintMenuItems(List<MenuItemDTO> menuItems)
        {
            if (menuItems.Count == 0)
            {
                PrintInfo("Няма намерени меню позиции.");
                return;
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     МЕНЮ ПОЗИЦИИ                           ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");

            foreach (var item in menuItems)
            {
                string available = item.IsAvailable ? "Налично" : "Не е налично";
                Console.WriteLine($"║ ID: {item.Id,-3} │ {item.Name,-45} ║");
                Console.WriteLine($"║ Цена: {item.Price,-6:F2} lv │ {available,-10} │ {item.CategoryName,-25} ║");
                Console.WriteLine($"║ Ресторант: {item.RestaurantName,-45} ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            }
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }

        public static void PrintOrders(List<OrderDTO> orders)
        {
            if (orders.Count == 0)
            {
                PrintInfo("Няма намерени поръчки.");
                return;
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                       ПОРЪЧКИ                              ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");

            foreach (var order in orders)
            {
                Console.WriteLine($"║ Поръчка #{order.Id,-3} │ Дата: {order.OrderDate:dd.MM.yyyy HH:mm}         ║");
                Console.WriteLine($"║ Потребител: {order.Username,-20} │ Статус: {order.Status,-15} ║");
                Console.WriteLine($"║ Ресторант: {order.RestaurantName,-44} ║");
                Console.WriteLine($"║ Обща сума: {order.TotalAmount,-6:F2} lv                                  ║");

                if (order.Items.Count > 0)
                {
                    Console.WriteLine("║ Позиции:                                                   ║");
                    foreach (var item in order.Items)
                    {
                        Console.WriteLine($"║   - {item.MenuItemName,-30} x{item.Quantity} | {item.Price:F2} lv    ║");
                    }
                }

                Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            }
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }

        public static void PrintUsers(List<UserDTO> users)
        {
            if (users.Count == 0)
            {
                PrintInfo("Няма намерени потребители.");
                return;
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     ПОТРЕБИТЕЛИ                            ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");

            foreach (var user in users)
            {
                Console.WriteLine($"║ ID: {user.Id,-3} │ {user.Username,-45} ║");
                Console.WriteLine($"║ Роля: {user.Role,-20} │ Статус: {user.Status,-20} ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            }
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }

        public static void WaitForKey(string message = "Натиснете Enter за продължение...")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
