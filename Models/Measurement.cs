using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Models
{
    public class Measurement
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Date { get; set; }
        public double Weight { get; set; }
        public double Chest { get; set; }
        public double Waist { get; set; }
        public double Hips { get; set; }
    }
}
