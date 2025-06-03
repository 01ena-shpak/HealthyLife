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

        public static void UpdateWaterIntake(string username, string date, double amount)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string checkQuery = "SELECT COUNT(*) FROM WaterIntake WHERE Username=@Username AND Date=@Date";
                var checkCmd = new SQLiteCommand(checkQuery, connection);
                checkCmd.Parameters.AddWithValue("@Username", username);
                checkCmd.Parameters.AddWithValue("@Date", date);
                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (exists > 0)
                {
                    string updateQuery = "UPDATE WaterIntake SET Amount=@Amount WHERE Username=@Username AND Date=@Date";
                    var updateCmd = new SQLiteCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.Parameters.AddWithValue("@Username", username);
                    updateCmd.Parameters.AddWithValue("@Date", date);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    string insertQuery = "INSERT INTO WaterIntake (Username, Date, Amount) VALUES (@Username, @Date, @Amount)";
                    var insertCmd = new SQLiteCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Date", date);
                    insertCmd.Parameters.AddWithValue("@Amount", amount);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        public static double GetWaterAmountByDate(string username, string date)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Amount FROM WaterIntake WHERE Username=@Username AND Date=@Date";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Date", date);
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDouble(result) : 0;
            }
        }

        public static List<WaterIntake> GetWaterIntakeByDateRange(string username, DateTime startDate, DateTime endDate)
        {
            var waterList = new List<WaterIntake>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT * FROM WaterIntake WHERE Username = @Username AND Date BETWEEN @StartDate AND @EndDate";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
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
