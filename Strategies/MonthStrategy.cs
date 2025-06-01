using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Strategies
{
    public class MonthStrategy : IStatsStrategy
    {
        public DateTime StartDate => DateTime.Today.AddMonths(-1).AddDays(1);
        public DateTime EndDate => DateTime.Today;
    }
}
