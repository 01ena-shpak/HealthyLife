using HealthyLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Services
{
    public class MeasurementService
    {
        private static string connectionString = "Data Source=users.db";

        public static void AddMeasurement(Measurement measurement)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Measurements (Username, Date, Weight, Chest, Waist, Hips)
                                 VALUES (@Username, @Date, @Weight, @Chest, @Waist, @Hips)";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", measurement.Username);
                cmd.Parameters.AddWithValue("@Date", measurement.Date);
                cmd.Parameters.AddWithValue("@Weight", measurement.Weight);
                cmd.Parameters.AddWithValue("@Chest", measurement.Chest);
                cmd.Parameters.AddWithValue("@Waist", measurement.Waist);
                cmd.Parameters.AddWithValue("@Hips", measurement.Hips);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Measurement> GetMeasurementsByDate(string username, string date)
        {
            var measurements = new List<Measurement>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Measurements WHERE Username = @Username AND Date = @Date";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Date", date);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    measurements.Add(new Measurement
                    {
                        Id = (int)(long)reader["Id"],
                        Username = reader["Username"].ToString(),
                        Date = reader["Date"].ToString(),
                        Weight = double.Parse(reader["Weight"].ToString()),
                        Chest = double.Parse(reader["Chest"].ToString()),
                        Waist = double.Parse(reader["Waist"].ToString()),
                        Hips = double.Parse(reader["Hips"].ToString())
                    });
                }
            }
            return measurements;
        }
    }
}
