using System.ComponentModel;

namespace OnlinePizzaWebApplication.Models

{
    public class Expenses
    {
        public int Id { get; set; }

        [DisplayName("Сумма расходов")]
        public double Expense { get; set; }

        [DisplayName("Название")]
        public string Name { get; set; }

    }
}