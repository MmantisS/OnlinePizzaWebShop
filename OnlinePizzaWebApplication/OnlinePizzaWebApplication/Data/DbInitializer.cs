using System;
using System.Collections.Generic;
using System.Linq;
using OnlinePizzaWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace OnlinePizzaWebApplication.Data
{
    public class DbInitializer
    {
        public static RoleManager<IdentityRole> _roleManager;
        public static void Initialize(AppDbContext context, IServiceProvider service)
        {
            context.Database.EnsureCreated();

            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            _roleManager = roleManager;
            var userManager = service.GetRequiredService<UserManager<IdentityUser>>();

            if (context.Pizzas.Any())
            {
                return;
            }
            ClearDatabase(context);
            CreateRole(context, roleManager, userManager, "Courier");
            CreateRole(context, roleManager, userManager, "Cook");
            CreateAdminRole(context, roleManager, userManager);
            CreateManagerRole(context, roleManager, userManager);
            CreateTechnologistRole(context, roleManager, userManager);
            SeedDatabase(context, roleManager, userManager);
        }
        private static void CreateRole(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, string NameOfRole)
        {
            bool roleExists = roleManager.RoleExistsAsync(NameOfRole).Result;
            if (roleExists)
            {
                return;
            }

            var role = new IdentityRole()
            {
                Name = NameOfRole
            };
            roleManager.CreateAsync(role).Wait();

            var user = new IdentityUser()
            {
                UserName = NameOfRole,
                Email = NameOfRole + "@default.com"
            };

            string adminPassword = "Password123";
            var userResult = userManager.CreateAsync(user, adminPassword).Result;

            if (userResult.Succeeded)
            {
                userManager.AddToRoleAsync(user, NameOfRole).Wait();
            }
        }

        private static void CreateTechnologistRole(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            bool roleExists = roleManager.RoleExistsAsync("Tech").Result;
            if (roleExists)
            {
                return;
            }

            var role = new IdentityRole()
            {
                Name = "Tech"
            };
            roleManager.CreateAsync(role).Wait();

            var user = new IdentityUser()
            {
                UserName = "Tech",
                Email = "Tech@default.com"
            };

            string adminPassword = "Password123";
            var userResult = userManager.CreateAsync(user, adminPassword).Result;

            if (userResult.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Tech").Wait();
            }
        }

        private static void CreateManagerRole(AppDbContext context, RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager)
        {
            bool roleExists = _roleManager.RoleExistsAsync("Manager").Result;
            if (roleExists)
            {
                return;
            }

            var role = new IdentityRole()
            {
                Name = "Manager"
            };
            _roleManager.CreateAsync(role).Wait();

            var user = new IdentityUser()
            {
                UserName = "manager",
                Email = "manager@default.com"
            };

            string adminPassword = "Password123";
            var userResult = _userManager.CreateAsync(user, adminPassword).Result;

            if (userResult.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "Manager").Wait();
            }
        }

        private static void CreateAdminRole(AppDbContext context, RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager)
        {
            bool roleExists = _roleManager.RoleExistsAsync("Admin").Result;
            if (roleExists)
            {
                return;
            }
            
            var role = new IdentityRole()
            {
                Name = "Admin"
            };
            _roleManager.CreateAsync(role).Wait();

            var user = new IdentityUser()
            {
                UserName = "admin",
                Email = "admin@default.com"
            };

            string adminPassword = "Password123";
            var userResult =  _userManager.CreateAsync(user, adminPassword).Result;

            if (userResult.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }

        private static void SeedDatabase(AppDbContext _context, RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager)
        {
            var cat1 = new Categories { Name = "Стандарт"};
            var cat2 = new Categories { Name = "Сезонные предложения"};
            var cat3 = new Categories { Name = "Новые"};

            var cats = new List<Categories>()
            {
                cat1, cat2, cat3
            };

            var piz1 = new Pizzas { Name = "Карбонара", Price = 70.00M, Category = cat1, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/2a/Pizza_capricciosa.jpg", IsPizzaOfTheWeek = false };
            var piz2 = new Pizzas { Name = "Сырная", Price = 70.00M, Category = cat3, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/3f/Vegetarian_pizza.jpg", IsPizzaOfTheWeek = false };
            var piz3 = new Pizzas { Name = "Четыре сезона", Price = 75.00M, Category = cat1, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/d/d1/Hawaiian_pizza_1.jpg", IsPizzaOfTheWeek = true };
            var piz4 = new Pizzas { Name = "Пепперони", Price = 65.00M, Category = cat1, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a3/Eq_it-na_pizza-margherita_sep2005_sml.jpg", IsPizzaOfTheWeek = false };
            var piz5 = new Pizzas { Name = "Ветчина и грибы", Price = 85.00M, Category = cat2, ImageUrl = "http://2.bp.blogspot.com/_3cSn3Qz_4IA/THkYqKwGw1I/AAAAAAAAAPg/ybKpvRbjDWE/s1600/matsl%C3%A4kten+002.JPG", IsPizzaOfTheWeek = true };

            var pizs = new List<Pizzas>()
            {
                piz1, piz2, piz3, piz4, piz5
            };

            var user1 = new IdentityUser { UserName = "user1@gmail.com", Email = "user1@gmail.com" };
            var user2 = new IdentityUser { UserName = "user2@gmail.com", Email = "user2@gmail.com" };
            var user3 = new IdentityUser { UserName = "user3@gmail.com", Email = "user3@gmail.com" };
            var user4 = new IdentityUser { UserName = "user4@gmail.com", Email = "user4@gmail.com" };
            var user5 = new IdentityUser { UserName = "user5@gmail.com", Email = "user5@gmail.com" };

            string userPassword = "Password123";

            var users = new List<IdentityUser>()
            {
                user1, user2, user3, user4, user5
            };

            foreach (var user in users)
            {
                _userManager.CreateAsync(user, userPassword).Wait();
            }

            var revs = new List<Reviews>()
            {
                new Reviews { User = user1, Title ="Best Pizza with mushrooms", Description="I love this Pizza with mushrooms on it.", Grade=4, Date=DateTime.Now, Pizza = piz1 },
                new Reviews { User = user2, Title ="Worst Pizza with mushrooms", Description="I hate this Pizza with mushrooms on it.", Grade=1, Date=DateTime.Now.AddDays(-1), Pizza = piz1 },
                new Reviews { User = user2, Title ="Only Bland Vegetables", Description="Tasteless vegetables on this soggy Pizza.", Grade=1, Date=DateTime.Now, Pizza = piz2 },
            };

            var ing1 = new Ingredients { Name = "Моцарелла", Price = 585, Quantity = 40 };
            var ing2 = new Ingredients { Name = "Мука", Price = 22, Quantity = 100, PurchasePrice = 2.5 };
            var ing3 = new Ingredients { Name = "Соус Томатный", Price = 450, Quantity = 10, PurchasePrice = 20, ExpirationDate = DateTime.Parse("15.02.2022")};
            var ing4 = new Ingredients { Name = "Бекон", Price = 10, Quantity = 40, PurchasePrice = 5, ExpirationDate = DateTime.Parse("12.12.2021")};
            var ing5 = new Ingredients { Name = "Ананас", Price = 183, Quantity = 40 };
            var ing6 = new Ingredients { Name = "Пепперони", Price = 25, Quantity = 40 };
            var ing7 = new Ingredients { Name = "Тесто", Price = 59, Quantity = 40 };
            var ing8= new Ingredients { Name = "Томаты", Price = 195, Quantity = 40 };
            var ing9 = new Ingredients { Name = "Пармезан", Price = 700, Quantity = 40 };

            var ings = new List<Ingredients>()
            {
                ing1, ing2, ing3, ing4, ing5, ing6, ing7
            };

            var pizIngs = new List<PizzaIngredients>()
            {
                new PizzaIngredients { Ingredient = ing1, Pizza = piz1 },
                new PizzaIngredients { Ingredient = ing2, Pizza = piz1 },
                new PizzaIngredients { Ingredient = ing3, Pizza = piz1 },
                new PizzaIngredients { Ingredient = ing5, Pizza = piz1 },
                new PizzaIngredients { Ingredient = ing7, Pizza = piz1},

                new PizzaIngredients { Ingredient = ing1, Pizza = piz2 },
                new PizzaIngredients { Ingredient = ing2, Pizza = piz2 },
                new PizzaIngredients { Ingredient = ing3, Pizza = piz2 },
                new PizzaIngredients { Ingredient = ing4, Pizza = piz2 },
                new PizzaIngredients { Ingredient = ing7, Pizza = piz2},

                new PizzaIngredients { Ingredient = ing1, Pizza = piz3 },
                new PizzaIngredients { Ingredient = ing2, Pizza = piz3 },
                new PizzaIngredients { Ingredient = ing3, Pizza = piz3 },
                new PizzaIngredients { Ingredient = ing7, Pizza = piz3},

                new PizzaIngredients { Ingredient = ing1, Pizza = piz4 },
                new PizzaIngredients { Ingredient = ing2, Pizza = piz4 },
                new PizzaIngredients { Ingredient = ing3, Pizza = piz4 },
                new PizzaIngredients { Ingredient = ing7, Pizza = piz4},

                new PizzaIngredients { Ingredient = ing1, Pizza = piz5 },
                new PizzaIngredients { Ingredient = ing2, Pizza = piz5 },
                new PizzaIngredients { Ingredient = ing3, Pizza = piz5 },
                new PizzaIngredients { Ingredient = ing6, Pizza = piz5 },
                new PizzaIngredients { Ingredient = ing4, Pizza = piz5 },
                new PizzaIngredients { Ingredient = ing7, Pizza = piz5},

            };
            var Employees = new List<Employee>()
            { new Employee {Name = "Колмыков Никита", Salary = 40000, Role = _roleManager.Roles.First(e => e.Name == "Manager"), WorkedHours= 20},
                new Employee {Name = "Ягодников Алексей", Salary = 50000, Role = _roleManager.Roles.First(e => e.Name == "Tech"), WorkedHours = 15},
                new Employee {Name = "Кардапольцев Дмитрий", Salary = 20000, Role = _roleManager.Roles.First(e => e.Name == "Admin")},
                new Employee {Name = "Пекаревич Иван", Salary = 30000, User = _userManager.Users.First(o => o.UserName == "Cook"), Role = _roleManager.Roles.First(e => e.Name == "Cook")},
                new Employee {Name = "Курьеров Георгий", Salary = 20000, Role = _roleManager.Roles.First(e => e.Name == "Courier"), BonusJobs = 200, AmountOfBonusJobs = 10},
                new Employee {Name = "Курьерович Сергей", Salary = 20000, Role = _roleManager.Roles.First(e => e.Name == "Courier"),  BonusJobs = 200, AmountOfBonusJobs = 5}
            };

            var ord1 = new Order
            {
                FirstName = "Иван",
                LastName = "Иванов",
                AddressLine1 = "Ленина 12",
                Email = "pelle22@gmail.com",
                OrderPlaced = DateTime.Now.AddDays(-2),
                PhoneNumber = "0705123456",
                User = user1,
                OrderTotal = 370.00M,
                EmployeeCook = Employees.ElementAt(1),
                EmployeeCourier = Employees.ElementAt(2)
            };



            var ord2 = new Order { };
            var ord3 = new Order { };

            var orderLines = new List<OrderDetail>()
            {
                new OrderDetail { Order=ord1, Pizza=piz1, Amount=2, Price=piz1.Price,},
                new OrderDetail { Order=ord1, Pizza=piz3, Amount=1, Price=piz3.Price,},
                new OrderDetail { Order=ord1, Pizza=piz5, Amount=3, Price=piz5.Price,},
            };

            var orders = new List<Order>()
            {
                ord1
            };


            _context.Employees.AddRange(Employees);
            _context.Categories.AddRange(cats);
            _context.Pizzas.AddRange(pizs);
            _context.Reviews.AddRange(revs);
            _context.Orders.AddRange(orders);
            _context.OrderDetails.AddRange(orderLines);
            _context.Ingredients.AddRange(ings);
            _context.PizzaIngredients.AddRange(pizIngs);

            _context.SaveChanges();
        }

        private static void ClearDatabase(AppDbContext _context)
        {
            var pizzaIngredients = _context.PizzaIngredients.ToList();
            _context.PizzaIngredients.RemoveRange(pizzaIngredients);

            var ingredients = _context.Ingredients.ToList();
            _context.Ingredients.RemoveRange(ingredients);

            var reviews = _context.Reviews.ToList();
            _context.Reviews.RemoveRange(reviews);

            var shoppingCartItems = _context.ShoppingCartItems.ToList();
            _context.ShoppingCartItems.RemoveRange(shoppingCartItems);

            var users = _context.Users.ToList();
            var userRoles = _context.UserRoles.ToList();

            foreach (var user in users)
            {
                if (!userRoles.Any(r => r.UserId == user.Id))
                {
                    _context.Users.Remove(user);
                }
            }

            var orderDetails = _context.OrderDetails.ToList();
            _context.OrderDetails.RemoveRange(orderDetails);

            var orders = _context.Orders.ToList();
            _context.Orders.RemoveRange(orders);

            var pizzas = _context.Pizzas.ToList();
            _context.Pizzas.RemoveRange(pizzas);

            var categories = _context.Categories.ToList();
            _context.Categories.RemoveRange(categories);

            _context.SaveChanges();
        }
    }
}
