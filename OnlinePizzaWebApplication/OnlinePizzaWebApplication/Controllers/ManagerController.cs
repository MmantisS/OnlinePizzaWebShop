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
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        //private readonly IAdminRepository _adminRepo;

        public ManagerController(IOrderRepository orderRepository,
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

        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            bool isManager = await _userManager.IsInRoleAsync(user, "Manager");
            if (isAdmin || isManager)
            {
                var allOrders = await _context.Orders.Include(o => o.OrderLines).Include(o => o.EmployeeCourier).Include(o => o.EmployeeCook).Include(o => o.User).ToListAsync();
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

        public async Task<IActionResult> Employees()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            bool isManager = await _userManager.IsInRoleAsync(user, "Manager");
            if (isAdmin || isManager)
            {
                var Employees = await _context.Employees.ToListAsync();
                return View(Employees);
            }
            return View();
        }

    }
}