using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetiFi.Models
{
    public class CreditorCategory
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Institution Name")]
        [Required(ErrorMessage = "Institution Name is required")]
        public string Name { get; set; }

       

        
        [Display(Name = "Type of Debt")]
        [Required(ErrorMessage = "Account Type is required")]
        public int DebtCategoryId { get; set; }

        public virtual DebtCategory DebtCategory { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}
