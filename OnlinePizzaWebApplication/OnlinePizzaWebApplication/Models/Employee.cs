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

        public double WorkedHours { get; set; }
        public double BonusJobs { get; set; }
        public int AmountOfBonusJobs { get; set; }
        public virtual IdentityRole Role { get; set; }
        public double CountSalary()
        { 
            Salary = Salary / 160 * WorkedHours;
            Salary += BonusJobs * AmountOfBonusJobs;
            return Salary;
        }

    }


}
