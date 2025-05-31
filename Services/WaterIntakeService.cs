using HealthyLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Services
{
    public class WaterIntakeService
    {
        private static string connectionString = "Data Source=users.db";

        public static void AddWaterIntake(WaterIntake water)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO WaterIntake (Username, Date, Amount)
                                 VALUES (@Username, @Date, @Amount)";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", water.Username);
                cmd.Parameters.AddWithValue("@Date", water.Date);
                cmd.Parameters.AddWithValue("@Amount", water.Amount);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<WaterIntake> GetWaterIntakeByDate(string username, string date)
        {
            var waterList = new List<WaterIntake>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM WaterIntake WHERE Username = @Username AND Date = @Date";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Date", date);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    waterList.Add(new WaterIntake
                    {
                        Id = (int)(long)reader["Id"],
                        Username = reader["Username"].ToString(),
                        Date = reader["Date"].ToString(),
                        Amount = double.Parse(reader["Amount"].ToString())
                    });
                }
            }
            return waterList;
        }
    }
}
