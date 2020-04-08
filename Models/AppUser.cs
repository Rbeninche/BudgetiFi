using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetiFi.Models
{
    public class AppUser : IdentityUser
    {
        public double HouseholdIncome { get; set; }
    }
}
