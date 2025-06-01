using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Strategies
{
    public interface IStatsStrategy
    {
        DateTime StartDate { get; }
        DateTime EndDate { get; }
    }
}
