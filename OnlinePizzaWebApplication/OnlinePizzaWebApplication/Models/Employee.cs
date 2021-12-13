using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzaWebApplication.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }

        public double FinalSalary { get; set; }
        public double WorkedHours { get; set; }
        public double BonusJobs { get; set; }
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
