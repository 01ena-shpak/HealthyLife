using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Strategies
{
    public class AllTimeStrategy : IStatsStrategy
    {
        public DateTime StartDate => DateTime.MinValue; // найдавніша дата
        public DateTime EndDate => DateTime.Today;
    }
}
