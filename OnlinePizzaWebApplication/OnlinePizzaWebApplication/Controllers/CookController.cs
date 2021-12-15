using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OnlinePizzaWebApplication.Repositories;
using OnlinePizzaWebApplication.Models;
using OnlinePizzaWebApplication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OnlinePizzaWebApplication.Controllers
{
    [Authorize(Roles = "Cook")]
    public class CookController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        //private readonly IAdminRepository _adminRepo;

        public CookController(IOrderRepository orderRepository,
            ShoppingCart shoppingCart, AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> EditOrder(int id)
        {
            ViewBag.Cooks = _context.Employees.Where(o => o.Role.Name == "Cook");
            ViewBag.Courier = _context.Employees.Where(o => o.Role.Name == "Courier");
            var allOrders = await _context.Orders.Where(o => o.OrderId == id).Include(o => o.OrderLines).Include(o => o.User).Include(o => o.EmployeeCourier).Include(o => o.EmployeeCook).ToListAsync();
            return View(allOrders);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOrder(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var updateToOrder = _context.Orders.Include(o => o.EmployeeCook).Include(o => o.EmployeeCourier)
                    .First(o => o.OrderId == id);
                updateToOrder.Status = Enum.Parse<Status>(collection[collection.Keys.ElementAt(0)]);
                updateToOrder.EmployeeCook = _context.Employees.First(o =>
                    o.Id == Convert.ToInt32(collection[collection.Keys.ElementAt(1)]));
                _context.Update(updateToOrder);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            bool isManager = await _userManager.IsInRoleAsync(user, "Cook");
            if (isAdmin || isManager)
            {
                var allOrders = await _context.Orders.Include(o => o.OrderLines)
                    .Include(o => o.EmployeeCourier)
                    .Include(o => o.EmployeeCook).Where(o => o.EmployeeCook.User == user)
                    .Include(o => o.User)
                    .ToListAsync();
                return View(allOrders);
            }
            else
            {
                var orders = await _context.Orders.Include(o => o.OrderLines)
                    .Include(o => o.User)
                    .Where(r => r.User == user).ToListAsync();
                return View(orders);
            }
        }
    }
}