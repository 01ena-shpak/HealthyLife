using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Models
{
    public class Training
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public double Duration { get; set; } // хв
        public double Calories { get; set; }
    }
}
