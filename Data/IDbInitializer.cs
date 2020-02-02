using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetiFi.Data
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}
