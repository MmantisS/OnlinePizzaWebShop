using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlinePizzaWebApplication.Data;
using OnlinePizzaWebApplication.Models;

namespace OnlinePizzaWebApplication.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly AppDbContext _context;

        public ExpensesController(AppDbContext context)
        {
            _context = context;
        }
        // GET: ExpensesController
        public async Task<ActionResult> Index()
        {
            await _context.Employees.ForEachAsync(e => e.CountSalary());
            var list = await _context.Expenses.ToListAsync();
            foreach (var employee in _context.Employees)
            {
                if (!list.Select(e => e.Name).Contains("Выплата ЗП для " + employee.Name))
                    _context.Expenses.Add(new Expenses() {Name = "Выплата ЗП для " + employee.Name, Expense = employee.FinalSalary});
            }

            _context.SaveChanges();
            var expenses = _context.Expenses;
            return View(expenses);
        }

        // GET: ExpensesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExpensesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExpensesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExpensesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExpensesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExpensesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExpensesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
