using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzaWebApplication.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [DisplayName("ФИО")]
        public string Name { get; set; }
        [DisplayName("Оклад")]
        public double Salary { get; set; }
        [DisplayName("Финальная ЗП")]
        public double FinalSalary { get; set; }
        [DisplayName("Отработанные часы")]
        public double WorkedHours { get; set; }
        [DisplayName("Стоимость доп.работ")]
        public double BonusJobs { get; set; }
        [DisplayName("Количество выполненных заказов")]
        public int AmountOfBonusJobs { get; set; }

        public virtual IList<OrderDetail> AssignedOrders { get; set; }
        public IdentityRole Role { get; set; }
        
        public IdentityUser User { get; set; } 
        public double CountSalary()
        {
            FinalSalary = Salary / 160 * WorkedHours;
            FinalSalary += BonusJobs * AmountOfBonusJobs;
            return FinalSalary;
        }

    }


}
