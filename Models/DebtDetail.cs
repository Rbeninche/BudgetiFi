using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetiFi.Models
{
    public class DebtDetail
    {
        [Key]
        public int Id { get; set; }
      
        [Display(Name = "Name of Debt")]
        [Required(ErrorMessage = "Name of Debt is required")]
        public string Name { get; set; }

        [Display(Name = "Due Date"), DataType(DataType.Date)]
        [Required(ErrorMessage = "Due Date is required")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Credit Limit")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "Credit Limit is required")]
        public double CreditLimit { get; set; }

        [Display(Name = "Current Balance")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "Debt Balance is required")]
        public double Balance { get; set; }

        private double _availableCredit;

        [Display(Name = "Available Credit")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double AvailableCredit
        {
            get
            {
                _availableCredit = CreditLimit - Balance;
                return _availableCredit;

            }
            set
            {
                _availableCredit = value;

            }
        }

        



        [Display(Name = "Minimum Payment")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "Minimum Payment is required")]
        public double MinimumPayment { get; set; }

      

        [Display(Name = "Type of Debt")]
        [Required(ErrorMessage = "Account Type is required")]
        public int DebtCategoryId { get; set; }

        public virtual DebtCategory DebtCategory { get; set; }

        [Display(Name = "Institution Name")]
        [Required(ErrorMessage = "Name of Intitution is required")]
        public int CreditorCategoryId { get; set; }

        public virtual CreditorCategory CreditorCategory { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }

       
        [Display(Name = "Is this debt revolving? (Check if applicable)")]
        public bool Ignore { get; set; }
    }
}
