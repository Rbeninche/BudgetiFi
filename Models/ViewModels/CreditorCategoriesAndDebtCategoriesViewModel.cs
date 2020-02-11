using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetiFi.Models.ViewModels
{
    public class CreditorCategoriesAndDebtCategoriesViewModel
    {
        public IEnumerable<DebtCategory> DebtCategoryList { get; set; }
        public CreditorCategory CreditorCategory { get; set; }
        public List<string> CreditorCategoryList { get; set; }

        public string StatusMessage { get; set; }
    }
}
