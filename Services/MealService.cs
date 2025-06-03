using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using HealthyLife.Models;

namespace HealthyLife.Services
{
    public class MealService
    {
        private static string connectionString = "Data Source=users.db";

        public static void AddMeal(Meal meal)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Meals (Username, Date, MealType, Description, Calories, Proteins, Fats, Carbs, Grams)
                                 VALUES (@Username, @Date, @MealType, @Description, @Calories, @Proteins, @Fats, @Carbs, @Grams)";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", meal.Username);
                cmd.Parameters.AddWithValue("@Date", meal.Date);
                cmd.Parameters.AddWithValue("@MealType", meal.MealType);
                cmd.Parameters.AddWithValue("@Description", meal.Description);
                cmd.Parameters.AddWithValue("@Calories", meal.Calories);
                cmd.Parameters.AddWithValue("@Proteins", meal.Proteins);
                cmd.Parameters.AddWithValue("@Fats", meal.Fats);
                cmd.Parameters.AddWithValue("@Carbs", meal.Carbs);
                cmd.Parameters.AddWithValue("@Grams", meal.Grams);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Meal> GetMealsByDate(string username, string date)
        {
            var meals = new List<Meal>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Meals WHERE Username = @Username AND Date = @Date";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Date", date);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        meals.Add(new Meal
                        {
                            Id = (int)(long)reader["Id"],
                            Username = reader["Username"].ToString(),
                            Date = reader["Date"].ToString(),
                            MealType = reader["MealType"].ToString(),
                            Description = reader["Description"].ToString(),
                            Calories = double.Parse(reader["Calories"].ToString()),
                            Proteins = double.Parse(reader["Proteins"].ToString()),
                            Fats = double.Parse(reader["Fats"].ToString()),
                            Carbs = double.Parse(reader["Carbs"].ToString()),
                            Grams = double.Parse(reader["Grams"].ToString())
                        });
                    }
                }
            }
            return meals;
        }

        public static void DeleteMeal(int mealId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Meals WHERE Id = @Id";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", mealId);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Meal> GetMealsByDateRange(string username, DateTime startDate, DateTime endDate)
        {
            var meals = new List<Meal>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT * FROM Meals WHERE Username = @Username AND Date BETWEEN @StartDate AND @EndDate";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        meals.Add(new Meal
                        {
                            Id = (int)(long)reader["Id"],
                            Username = reader["Username"].ToString(),
                            Date = reader["Date"].ToString(),
                            MealType = reader["MealType"].ToString(),
                            Description = reader["Description"].ToString(),
                            Calories = double.Parse(reader["Calories"].ToString()),
                            Proteins = double.Parse(reader["Proteins"].ToString()),
                            Fats = double.Parse(reader["Fats"].ToString()),
                            Carbs = double.Parse(reader["Carbs"].ToString()),
                            Grams = double.Parse(reader["Grams"].ToString())
                        });
                    }
                }
            }
            return meals;
        }
    }
}
