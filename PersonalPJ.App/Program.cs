using RestaurantOrderSystem.Data;
using RestaurantOrderSystem.Data.Services;    
using RestaurantOrderSystem.Data.DTOs;        
using RestaurantOrderSystem.App.Utilities;

namespace RestaurantOrderSystem.App
{
    
    class Program
    {
        private static RestaurantDbContext _context = null!;
        private static AuthService _authService = null!;
        private static DataService _dataService = null!;
        private static OrderService _orderService = null!;
        private static QueryService _queryService = null!;
        private static JSONService _jsonService = null!;

        private static UserDTO? _currentUser = null;

        static void Main(string[] args)
        {
            
            _context = new RestaurantDbContext();
            _authService = new AuthService(_context);
            _dataService = new DataService(_context);
            _orderService = new OrderService(_context);
            _queryService = new QueryService(_context);
            _jsonService = new JSONService(_context);

            
            UIHelper.PrintHeader("RESTAURANT ORDER SYSTEM");
            Console.WriteLine("Проверка на базата данни...");

            if (_context.Database.EnsureCreated())
            {
                UIHelper.PrintSuccess("База данни е създадена успешно!");
                SeedInitialData();
            }
            else
            {
                UIHelper.PrintInfo("База данни вече съществува.");
            }

            UIHelper.WaitForKey();

            
            while (true)
            {
                ShowWelcomeScreen();
            }
        }


        static void ShowWelcomeScreen()
        {
            UIHelper.PrintHeader("ДОБРЕ ДОШЛИ В RESTAURANT ORDER SYSTEM");

            if (_currentUser != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Влезли сте като: {_currentUser.Username} ({_currentUser.Role})");
                Console.ResetColor();
                Console.WriteLine();
            }

            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Продължи като Guest");
            Console.WriteLine("0. Изход");
            Console.Write("\nИзберете опция: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    LoginMenu();
                    break;
                case "2":
                    RegisterMenu();
                    break;
                case "3":
                    GuestMenu();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    UIHelper.PrintError("Невалидна опция!");
                    UIHelper.WaitForKey();
                    break;
            }
        }

        
        /// Login меню
        
        static void LoginMenu()
        {
            UIHelper.PrintHeader("LOGIN");

            Console.Write("Потребителско име: ");
            string? username = Console.ReadLine();

            Console.Write("Парола: ");
            string? password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                UIHelper.PrintError("Моля въведете потребителско име и парола!");
                UIHelper.WaitForKey();
                return;
            }

            var loginDto = new UserLoginDTO
            {
                Username = username,
                Password = password
            };

            _currentUser = _authService.Login(loginDto);

            if (_currentUser == null)
            {
                UIHelper.PrintError("Грешно потребителско име или парола!");
                UIHelper.WaitForKey();
                return;
            }

            UIHelper.PrintSuccess($"Успешно влизане! Добре дошли, {_currentUser.Username}!");
            UIHelper.WaitForKey();

        
            if (_currentUser.Role == "Administrator")
            {
                AdministratorMenu();
            }
            else
            {
                RegisteredUserMenu();
            }
        }

        
        /// Register меню
        
        static void RegisterMenu()
        {
            UIHelper.PrintHeader("РЕГИСТРАЦИЯ");

            Console.Write("Потребителско име: ");
            string? username = Console.ReadLine();

            Console.Write("Парола: ");
            string? password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                UIHelper.PrintError("Моля въведете потребителско име и парола!");
                UIHelper.WaitForKey();
                return;
            }

            var registerDto = new UserRegisterDTO
            {
                Username = username,
                Password = password
            };

            _currentUser = _authService.Register(registerDto);

            if (_currentUser == null)
            {
                UIHelper.PrintError("Потребителското име вече съществува!");
                UIHelper.WaitForKey();
                return;
            }

            UIHelper.PrintSuccess($"Успешна регистрация! Добре дошли, {_currentUser.Username}!");
            UIHelper.WaitForKey();

            RegisteredUserMenu();
        }

        
        /// Guest меню 
        
        static void GuestMenu()
        {
            while (true)
            {
                UIHelper.PrintHeader("GUEST MENU");
                Console.WriteLine("Вие сте Guest потребител (само четене)\n");

                Console.WriteLine("1. Преглед на ресторанти");
                Console.WriteLine("2. Преглед на меню позиции");
                Console.WriteLine("3. Търсене на меню позиция");
                Console.WriteLine("0. Назад");
                Console.Write("\nИзберете опция: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewRestaurants();
                        break;
                    case "2":
                        ViewMenuItems();
                        break;
                    case "3":
                        SearchMenuItems();
                        break;
                    case "0":
                        return;
                    default:
                        UIHelper.PrintError("Невалидна опция!");
                        UIHelper.WaitForKey();
                        break;
                }
            }
        }

        /// RegisteredUser меню 

        static void RegisteredUserMenu()
        {
            while (true)
            {
                UIHelper.PrintHeader("REGISTERED USER MENU");
                Console.WriteLine($"Влезли сте като: {_currentUser!.Username}\n");

                Console.WriteLine("=== ПРЕГЛЕД ===");
                Console.WriteLine("1. Преглед на ресторанти");
                Console.WriteLine("2. Преглед на категории");
                Console.WriteLine("3. Преглед на меню позиции");
                Console.WriteLine("4. Преглед на моите поръчки");

                Console.WriteLine("\n=== ДОБАВЯНЕ ===");
                Console.WriteLine("5. Създай поръчка");

                Console.WriteLine("\n=== JSON ОПЕРАЦИИ ===");
                Console.WriteLine("6. Импорт на ресторанти от JSON");
                Console.WriteLine("7. Импорт на меню позиции от JSON");
                Console.WriteLine("8. Експорт на поръчки към JSON");

                Console.WriteLine("\n=== СПРАВКИ (LINQ) ===");
                Console.WriteLine("9. Топ 10 най-скъпи меню позиции");
                Console.WriteLine("10. Активни поръчки");
                Console.WriteLine("11. Статистика по ресторанти");
                Console.WriteLine("12. Най-популярни меню позиции");
                Console.WriteLine("13. Средна стойност на поръчка");

                Console.WriteLine("\n0. Logout");
                Console.Write("\nИзберете опция: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewRestaurants();
                        break;
                    case "2":
                        ViewCategories();
                        break;
                    case "3":
                        ViewMenuItems();
                        break;
                    case "4":
                        ViewMyOrders();
                        break;
                    case "5":
                        CreateOrder();
                        break;
                    case "6":
                        ImportRestaurants();
                        break;
                    case "7":
                        ImportMenuItems();
                        break;
                    case "8":
                        ExportOrders();
                        break;
                    case "9":
                        ShowTop10ExpensiveItems();
                        break;
                    case "10":
                        ShowActiveOrders();
                        break;
                    case "11":
                        ShowRestaurantStatistics();
                        break;
                    case "12":
                        ShowPopularMenuItems();
                        break;
                    case "13":
                        ShowAverageOrderValue();
                        break;
                    case "0":
                        _currentUser = null;
                        UIHelper.PrintSuccess("Успешно излизане!");
                        UIHelper.WaitForKey();
                        return;
                    default:
                        UIHelper.PrintError("Невалидна опция!");
                        UIHelper.WaitForKey();
                        break;
                }
            }
        }


        /// Administrator меню 

        /// <summary>
        /// Administrator меню (управление на потребители + меню позиции)
        /// </summary>
        static void AdministratorMenu()
        {
            while (true)
            {
                UIHelper.PrintHeader("ADMINISTRATOR MENU");
                Console.WriteLine($"Влезли сте като: {_currentUser!.Username} (Admin)\n");

                Console.WriteLine("=== УПРАВЛЕНИЕ НА ПОТРЕБИТЕЛИ ===");
                Console.WriteLine("1. Преглед на всички потребители");
                Console.WriteLine("2. Блокирай потребител");
                Console.WriteLine("3. Изтрий потребител");

                Console.WriteLine("\n=== УПРАВЛЕНИЕ НА МЕНЮ ===");
                Console.WriteLine("4. Преглед на меню позиции");
                Console.WriteLine("5. Добави меню позиция");

                Console.WriteLine("\n0. Logout");
                Console.Write("\nИзберете опция: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllUsers();
                        break;
                    case "2":
                        BlockUser();
                        break;
                    case "3":
                        DeleteUser();
                        break;
                    case "4":
                        ViewMenuItems();
                        break;
                    case "5":
                        AddMenuItem();
                        break;
                    case "0":
                        _currentUser = null;
                        UIHelper.PrintSuccess("Успешно излизане!");
                        UIHelper.WaitForKey();
                        return;
                    default:
                        UIHelper.PrintError("Невалидна опция!");
                        UIHelper.WaitForKey();
                        break;
                }
            }
        }

        #region View 

        static void ViewRestaurants()
        {
            UIHelper.PrintHeader("РЕСТОРАНТИ");
            var restaurants = _dataService.GetAllRestaurants();
            UIHelper.PrintRestaurants(restaurants);
            UIHelper.WaitForKey();
        }

        static void ViewCategories()
        {
            UIHelper.PrintHeader("КАТЕГОРИИ");
            var categories = _dataService.GetAllCategories();
            UIHelper.PrintCategories(categories);
            UIHelper.WaitForKey();
        }

        static void ViewMenuItems()
        {
            UIHelper.PrintHeader("МЕНЮ ПОЗИЦИИ");
            var menuItems = _dataService.GetAllMenuItems();
            UIHelper.PrintMenuItems(menuItems);
            UIHelper.WaitForKey();
        }

        static void SearchMenuItems()
        {
            UIHelper.PrintHeader("ТЪРСЕНЕ НА МЕНЮ ПОЗИЦИЯ");
            Console.Write("Въведете име на ястие за търсене: ");
            string? searchTerm = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                UIHelper.PrintError("Моля въведете текст за търсене!");
                UIHelper.WaitForKey();
                return;
            }

            var results = _dataService.SearchMenuItems(searchTerm);
            UIHelper.PrintMenuItems(results);
            UIHelper.WaitForKey();
        }

        static void ViewMyOrders()
        {
            UIHelper.PrintHeader("МОИТЕ ПОРЪЧКИ");
            var orders = _orderService.GetOrdersByUserId(_currentUser!.Id);
            UIHelper.PrintOrders(orders);
            UIHelper.WaitForKey();
        }

        static void ViewAllUsers()
        {
            UIHelper.PrintHeader("ВСИЧКИ ПОТРЕБИТЕЛИ");
            var users = _authService.GetAllUsers();
            UIHelper.PrintUsers(users);
            UIHelper.WaitForKey();
        }

        #endregion

        #region CRUD Methods

        static void AddMenuItem()
        {
            UIHelper.PrintHeader("ДОБАВЯНЕ НА МЕНЮ ПОЗИЦИЯ");

            // Показване на налични ресторанти
            Console.WriteLine("=== НАЛИЧНИ РЕСТОРАНТИ ===\n");
            var restaurants = _dataService.GetAllRestaurants();

            foreach (var r in restaurants)
            {
                Console.WriteLine($"{r.Id}. {r.Name}");
            }

            Console.Write("\nВъведете ID на ресторант: ");
            if (!int.TryParse(Console.ReadLine(), out int restaurantId))
            {
                UIHelper.PrintError("Невалидно ID!");
                UIHelper.WaitForKey();
                return;
            }

            // Clear и показване на категории
            UIHelper.PrintHeader("ДОБАВЯНЕ НА МЕНЮ ПОЗИЦИЯ");
            Console.WriteLine("=== НАЛИЧНИ КАТЕГОРИИ ===\n");
            var categories = _dataService.GetAllCategories();

            foreach (var c in categories)
            {
                Console.WriteLine($"{c.Id}. {c.Name} - {c.Description}");
            }

            Console.Write("\nВъведете ID на категория: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryId))
            {
                UIHelper.PrintError("Невалидно ID!");
                UIHelper.WaitForKey();
                return;
            }

            // Clear и въвеждане на данни
            UIHelper.PrintHeader("ДОБАВЯНЕ НА МЕНЮ ПОЗИЦИЯ");

            Console.Write("Име на ястието: ");
            string? name = Console.ReadLine();

            Console.Write("Описание: ");
            string? description = Console.ReadLine();

            Console.Write("Цена: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                UIHelper.PrintError("Невалидна цена!");
                UIHelper.WaitForKey();
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                UIHelper.PrintError("Името е задължително!");
                UIHelper.WaitForKey();
                return;
            }

            var dto = new MenuItemCreateDTO
            {
                Name = name,
                Description = description ?? string.Empty,
                Price = price,
                RestaurantId = restaurantId,
                CategoryId = categoryId
            };

            var result = _dataService.AddMenuItem(dto);

            UIHelper.PrintHeader("ДОБАВЯНЕ НА МЕНЮ ПОЗИЦИЯ");

            if (result != null)
            {
                UIHelper.PrintSuccess($"Меню позиция '{result.Name}' е добавена успешно!");
                Console.WriteLine($"\nРесторант: {result.RestaurantName}");
                Console.WriteLine($"Категория: {result.CategoryName}");
                Console.WriteLine($"Цена: {result.Price:F2} lv");
            }
            else
            {
                UIHelper.PrintError("Грешка при добавяне на меню позиция!");
            }

            UIHelper.WaitForKey();
        }

        static void CreateOrder()
        {
            UIHelper.PrintHeader("СЪЗДАВАНЕ НА ПОРЪЧКА");

            var restaurants = _dataService.GetAllRestaurants();
            UIHelper.PrintRestaurants(restaurants);

            Console.Write("Въведете ID на ресторант: ");
            if (!int.TryParse(Console.ReadLine(), out int restaurantId))
            {
                UIHelper.PrintError("Невалидно ID!");
                UIHelper.WaitForKey();
                return;
            }

            var menuItems = _dataService.GetAvailableMenuItems();
            UIHelper.PrintMenuItems(menuItems);

            var orderItems = new List<OrderItemCreateDTO>();

            while (true)
            {
                Console.Write("\nВъведете ID на меню позиция (0 за край): ");
                if (!int.TryParse(Console.ReadLine(), out int menuItemId))
                {
                    UIHelper.PrintError("Невалидно ID!");
                    continue;
                }

                if (menuItemId == 0)
                {
                    break;
                }

                Console.Write("Количество: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                {
                    UIHelper.PrintError("Невалидно количество!");
                    continue;
                }

                orderItems.Add(new OrderItemCreateDTO
                {
                    MenuItemId = menuItemId,
                    Quantity = quantity
                });

                UIHelper.PrintSuccess("Позицията е добавена към поръчката!");
            }

            if (orderItems.Count == 0)
            {
                UIHelper.PrintError("Поръчката трябва да има поне една позиция!");
                UIHelper.WaitForKey();
                return;
            }

            Console.Write("Забележки (опционално): ");
            string? notes = Console.ReadLine();

            var orderDto = new OrderCreateDTO
            {
                UserId = _currentUser!.Id,
                RestaurantId = restaurantId,
                Notes = notes ?? string.Empty,
                Items = orderItems
            };

            var result = _orderService.CreateOrder(orderDto);

            if (result != null)
            {
                UIHelper.PrintSuccess($"Поръчка #{result.Id} е създадена успешно! Обща сума: {result.TotalAmount:F2} lv");
            }
            else
            {
                UIHelper.PrintError("Грешка при създаване на поръчка!");
            }

            UIHelper.WaitForKey();
        }

        static void BlockUser()
        {
            UIHelper.PrintHeader("БЛОКИРАНЕ НА ПОТРЕБИТЕЛ");
            var users = _authService.GetAllUsers();
            UIHelper.PrintUsers(users);

            Console.Write("Въведете ID на потребител за блокиране: ");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                UIHelper.PrintError("Невалидно ID!");
                UIHelper.WaitForKey();
                return;
            }

            bool success = _authService.BlockUser(userId);

            if (success)
            {
                UIHelper.PrintSuccess("Потребителят е блокиран успешно!");
            }
            else
            {
                UIHelper.PrintError("Грешка! Потребителят не съществува или е администратор.");
            }

            UIHelper.WaitForKey();
        }

        static void DeleteUser()
        {
            UIHelper.PrintHeader("ИЗТРИВАНЕ НА ПОТРЕБИТЕЛ");
            var users = _authService.GetAllUsers();
            UIHelper.PrintUsers(users);

            Console.Write("Въведете ID на потребител за изтриване: ");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                UIHelper.PrintError("Невалидно ID!");
                UIHelper.WaitForKey();
                return;
            }

            bool success = _authService.DeleteUser(userId);

            if (success)
            {
                UIHelper.PrintSuccess("Потребителят е изтрит успешно!");
            }
            else
            {
                UIHelper.PrintError("Грешка! Потребителят не съществува или е администратор.");
            }

            UIHelper.WaitForKey();
        }

        #endregion

        #region JSON Methods

        static void ImportRestaurants()
        {
            UIHelper.PrintHeader("ИМПОРТ НА РЕСТОРАНТИ ОТ JSON");
            Console.Write("Въведете пътя до JSON файла: ");
            string? path = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                UIHelper.PrintError("Файлът не съществува!");
                UIHelper.WaitForKey();
                return;
            }

            var (success, failed) = _jsonService.ImportRestaurants(path);
            UIHelper.PrintSuccess($"Успешно импортирани: {success} ресторанта");

            if (failed > 0)
            {
                UIHelper.PrintError($"Неуспешни: {failed} записа");
            }

            UIHelper.WaitForKey();
        }

        static void ImportMenuItems()
        {
            UIHelper.PrintHeader("ИМПОРТ НА МЕНЮ ПОЗИЦИИ ОТ JSON");
            Console.Write("Въведете пътя до JSON файла: ");
            string? path = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                UIHelper.PrintError("Файлът не съществува!");
                UIHelper.WaitForKey();
                return;
            }

            var (success, failed) = _jsonService.ImportMenuItems(path);
            UIHelper.PrintSuccess($"Успешно импортирани: {success} меню позиции");

            if (failed > 0)
            {
                UIHelper.PrintError($"Неуспешни: {failed} записа");
            }

            UIHelper.WaitForKey();
        }

        static void ExportOrders()
        {
            UIHelper.PrintHeader("ЕКСПОРТ НА ПОРЪЧКИ КЪМ JSON");
            Console.WriteLine("1. Експорт на всички поръчки");
            Console.WriteLine("2. Експорт на активни поръчки");
            Console.Write("Изберете опция: ");

            string? choice = Console.ReadLine();
            Console.Write("Въведете път за запазване (напр. orders.json): ");
            string? outputPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                UIHelper.PrintError("Моля въведете валиден път!");
                UIHelper.WaitForKey();
                return;
            }

            bool success = false;

            if (choice == "1")
            {
                success = _jsonService.ExportAllOrders(outputPath);
            }
            else if (choice == "2")
            {
                success = _jsonService.ExportActiveOrders(outputPath);
            }

            if (success)
            {
                UIHelper.PrintSuccess($"Поръчките са експортирани успешно в {outputPath}");
            }
            else
            {
                UIHelper.PrintError("Грешка при експортиране!");
            }

            UIHelper.WaitForKey();
        }

        #endregion

        #region LINQ Query Methods

        static void ShowTop10ExpensiveItems()
        {
            UIHelper.PrintHeader("ТОП 10 НАЙ-СКЪПИ МЕНЮ ПОЗИЦИИ");
            var items = _queryService.GetTop10MostExpensiveItems();
            UIHelper.PrintMenuItems(items);
            UIHelper.WaitForKey();
        }

        static void ShowActiveOrders()
        {
            UIHelper.PrintHeader("АКТИВНИ ПОРЪЧКИ");
            var orders = _queryService.GetActiveOrders();
            UIHelper.PrintOrders(orders);
            UIHelper.WaitForKey();
        }

        static void ShowRestaurantStatistics()
        {
            UIHelper.PrintHeader("СТАТИСТИКА ПО РЕСТОРАНТИ");

            var orderCounts = _queryService.GetOrderCountByRestaurant();
            var revenues = _queryService.GetTotalRevenueByRestaurant();

            Console.WriteLine("\n=== БРОЙ ПОРЪЧКИ ПО РЕСТОРАНТИ ===");
            foreach (var (name, count) in orderCounts)
            {
                Console.WriteLine($"{name,-40} : {count} поръчки");
            }

            Console.WriteLine("\n=== ОБЩ ПРИХОД ПО РЕСТОРАНТИ ===");
            foreach (var (name, revenue) in revenues)
            {
                Console.WriteLine($"{name,-40} : {revenue:F2} lv");
            }

            UIHelper.WaitForKey();
        }

        static void ShowPopularMenuItems()
        {
            UIHelper.PrintHeader("НАЙ-ПОПУЛЯРНИ МЕНЮ ПОЗИЦИИ");
            var items = _queryService.GetMostPopularMenuItems();

            Console.WriteLine("\n=== ТОП 10 НАЙ-ПОРЪЧВАНИ ЯСТИЯ ===");
            int rank = 1;
            foreach (var (name, count) in items)
            {
                Console.WriteLine($"{rank}. {name,-40} : {count} пъти");
                rank++;
            }

            UIHelper.WaitForKey();
        }

        static void ShowAverageOrderValue()
        {
            UIHelper.PrintHeader("СРЕДНА СТОЙНОСТ НА ПОРЪЧКА");
            var average = _queryService.GetAverageOrderValue();
            Console.WriteLine($"\nСредната стойност на поръчка е: {average:F2} lv");
            UIHelper.WaitForKey();
        }

        #endregion

        #region Initial Data Seeding


        /// Добавяне на начални данни в базата
        /// <summary>
        /// Добавяне на начални данни в базата
        /// </summary>
        static void SeedInitialData()
        {
            Console.WriteLine("\nДобавяне на начални данни...");

            // Добавяне на администратор
            var admin = new UserRegisterDTO
            {
                Username = "admin",
                Password = "admin123"
            };
            _authService.Register(admin);

            // Промяна на ролята на администратора
            var adminUser = _context.Users.First(u => u.Username == "admin");
            adminUser.Role = "Administrator";
            _context.SaveChanges();

            // Добавяне на тестов потребител
            var testUser = new UserRegisterDTO
            {
                Username = "user",
                Password = "user123"
            };
            _authService.Register(testUser);

            // Добавяне на категории
            var categories = new List<CategoryImportDTO>
    {
        new CategoryImportDTO { Name = "Предястия", Description = "Студени и топли предястия" },
        new CategoryImportDTO { Name = "Салати", Description = "Свежи салати" },
        new CategoryImportDTO { Name = "Основни ястия", Description = "Месни и рибни ястия" },
        new CategoryImportDTO { Name = "Паста", Description = "Италианска паста" },
        new CategoryImportDTO { Name = "Десерти", Description = "Сладкиши и десерти" }
    };

            foreach (var cat in categories)
            {
                _dataService.AddCategory(cat);
            }

            // Добавяне на ресторанти
            var restaurants = new List<RestaurantImportDTO>
    {
        new RestaurantImportDTO
        {
            Name = "La Bella Vita",
            Address = "ул. Витоша 15, София",
            Phone = "02/123-4567",
            Email = "info@labellavita.bg"
        },
        new RestaurantImportDTO
        {
            Name = "Bulgari Restaurant",
            Address = "бул. Княз Борис I 45, София",
            Phone = "02/234-5678",
            Email = "contact@bulgari.bg"
        }
    };

            foreach (var rest in restaurants)
            {
                _dataService.AddRestaurant(rest);
            }

            // Добавяне на меню позиции
            var menuItems = new List<MenuItemImportDTO>
    {
        // Предястия - Гуакамоле
        new MenuItemImportDTO
        {
            Name = "Гуакамоле",
            Description = "Пюре от авокадо с лимон, чесън и домати",
            Price = 8.50m,
            IsAvailable = true,
            RestaurantName = "La Bella Vita",
            CategoryName = "Предястия"
        },
        
        // Салати - Овчарска салата
        new MenuItemImportDTO
        {
            Name = "Овчарска салата",
            Description = "Салата с домати, краставици, чушки и сирене",
            Price = 7.50m,
            IsAvailable = true,
            RestaurantName = "Bulgari Restaurant",
            CategoryName = "Салати"
        },
        
        // Основни ястия - Свински джолан
        new MenuItemImportDTO
        {
            Name = "Свински джолан",
            Description = "Печен свински джолан със зеленчуци",
            Price = 18.00m,
            IsAvailable = true,
            RestaurantName = "Bulgari Restaurant",
            CategoryName = "Основни ястия"
        },
        
        // Паста - Болонезе
        new MenuItemImportDTO
        {
            Name = "Спагети Болонезе",
            Description = "Спагети с кайма и доматен сос",
            Price = 14.50m,
            IsAvailable = true,
            RestaurantName = "La Bella Vita",
            CategoryName = "Паста"
        },
        
        // Десерти - Суфле
        new MenuItemImportDTO
        {
            Name = "Шоколадово суфле",
            Description = "Топло шоколадово суфле с течна средина",
            Price = 9.50m,
            IsAvailable = true,
            RestaurantName = "La Bella Vita",
            CategoryName = "Десерти"
        }
    };

            // Добавяне на меню позициите в базата
            foreach (var menuItem in menuItems)
            {
                var restaurant = _context.Restaurants.FirstOrDefault(r => r.Name == menuItem.RestaurantName);
                var category = _context.Categories.FirstOrDefault(c => c.Name == menuItem.CategoryName);

                if (restaurant != null && category != null)
                {
                    var dto = new MenuItemCreateDTO
                    {
                        Name = menuItem.Name,
                        Description = menuItem.Description,
                        Price = menuItem.Price,
                        RestaurantId = restaurant.Id,
                        CategoryId = category.Id
                    };

                    _dataService.AddMenuItem(dto);
                }
            }

            UIHelper.PrintSuccess("Начални данни са добавени успешно!");
        }

        #endregion
    }
}