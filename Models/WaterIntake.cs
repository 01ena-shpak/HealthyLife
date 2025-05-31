using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Models
{
    public class WaterIntake
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Date { get; set; }
        public double Amount { get; set; } // мл
    }
}
