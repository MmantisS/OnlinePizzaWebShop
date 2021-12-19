using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzaWebApplication.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int PizzaId { get; set; }
        [DisplayName("Количество")]
        public int Amount { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }
        [DisplayName("Пицца")]
        public virtual Pizzas Pizza { get; set; }
        [DisplayName("Заказ")]
        public virtual Order Order { get; set; }
    }
}
