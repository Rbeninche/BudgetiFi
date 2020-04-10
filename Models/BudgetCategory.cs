using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetiFi.Models
{
    public class BudgetCategory
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Budget Category")]
        [Required(ErrorMessage = "Budget Category is required")]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}
