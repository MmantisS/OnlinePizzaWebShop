using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlinePizzaWebApplication.Repositories;
using OnlinePizzaWebApplication.Models;
using OnlinePizzaWebApplication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OnlinePizzaWebApplication.Controllers
{
    [Authorize(Roles = "Tech")]
    public class TechController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        //private readonly IAdminRepository _adminRepo;

        public TechController(IOrderRepository orderRepository,
            ShoppingCart shoppingCart, AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Ingridients()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            bool isManager = await _userManager.IsInRoleAsync(user, "Tech");
            if (isAdmin || isManager)
            {
                var allOrders = await _context.Ingredients.ToListAsync();
                return View(allOrders);
            }
            return View();
        }

        public async Task<IActionResult> BuyMore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredients = await _context.Ingredients.SingleOrDefaultAsync(m => m.Id == id);
            if (ingredients == null)
            {
                return NotFound();
            }
            return View(ingredients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyMore(int id, int AddedQuantity, [Bind("Id,Quantity")] Ingredients ingredients)
        {
            if (id != ingredients.Id)
            {
                return NotFound();
            }
            var entity = _context.Ingredients.First(e => e.Id == ingredients.Id);
            entity.Quantity += AddedQuantity;
            _context.Expenses.Add(new Expenses()
                {Name = string.Format("Покупка ингридиента {0}", entity.Name), Expense = entity.Quantity * entity.Price});
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return View(ingredients);
        }
    }
}