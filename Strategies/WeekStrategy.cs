using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Strategies
{
    public class WeekStrategy : IStatsStrategy
    {
        public DateTime StartDate => DateTime.Today.AddDays(-6); // поточний день і 6 попередніх
        public DateTime EndDate => DateTime.Today;
    }
}
