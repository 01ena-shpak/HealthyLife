using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Date { get; set; }
        public string MealType { get; set; }
        public string Description { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbs { get; set; }
        public double Grams { get; set; }
    }
}
