using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePizzaWebApplication.Repositories;
using OnlinePizzaWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OnlinePizzaWebApplication.Data;

namespace OnlinePizzaWebApplication.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(IOrderRepository orderRepository, 
            ShoppingCart shoppingCart, AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Checkout()
        {
            ViewBag.Items = _shoppingCart.GetShoppingCartItemsAsync().Result;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            order.UserId = userId;

            var items = await _shoppingCart.GetShoppingCartItemsAsync();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Корзина пустая, добавьте товаров");
            }

            if (ModelState.IsValid)
            {
                await _orderRepository.CreateOrderAsync(order);
                await _shoppingCart.ClearCartAsync();

                return RedirectToAction("CheckoutComplete");
            }

            ViewBag.Items = _shoppingCart.ShoppingCartItems;
            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = $"Спасибо за заказ!";
            return View();
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                var allOrders = await _context.Orders.Include(o => o.OrderLines).Include(o => o.User).Include(o => o.EmployeeCourier).Include(o => o.EmployeeCook).ToListAsync();
                return View(allOrders);
            }
            else
            {
                var orders = await _context.Orders.Include(o => o.OrderLines).Include(o => o.User)
                    .Where(r => r.User == user).ToListAsync();
                return View(orders);
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var orders = await _context.Orders.Include(o => o.OrderLines).Include(o => o.User)
                .SingleOrDefaultAsync(m => m.OrderId == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isAdmin = userRoles.Any(r => r == "Admin");

            if (orders == null)
            {
                return NotFound();
            }

            if (isAdmin == false)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                if (orders.UserId != userId)
                {
                    return BadRequest("You do not have permissions to view this order.");
                }
            }

            var orderDetailsList = _context.OrderDetails.Include(o => o.Pizza).Include(o => o.Order)
                .Where(x => x.OrderId == orders.OrderId);

            ViewBag.OrderDetailsList = orderDetailsList;

            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> EditAsync(int id)
        {
            ViewBag.Cooks = _context.Employees.Where(o => o.Role.Name == "Cook");
            ViewBag.Courier = _context.Employees.Where(o => o.Role.Name == "Courier");
            var allOrders = await _context.Orders.Where(o => o.OrderId == id).Include(o => o.OrderLines).Include(o => o.User).Include(o => o.EmployeeCourier).Include(o => o.EmployeeCook).ToListAsync();
            return View(allOrders);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var updateToOrder = _context.Orders.Include(o => o.EmployeeCook).Include(o => o.EmployeeCourier).First(o => o.OrderId == id);
                //updateToOrder.status = collection["item.Status"];
                updateToOrder.EmployeeCook = _context.Employees.First(o => o.Id == Convert.ToInt32(collection[collection.Keys.ElementAt(1)]));
                updateToOrder.EmployeeCourier = _context.Employees.First(o => o.Id == Convert.ToInt32(collection[collection.Keys.ElementAt(2)]));
                _context.Update(updateToOrder);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(o => o.User)
                .SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: OrdersTest/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderId == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}