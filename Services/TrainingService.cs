using HealthyLife.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLife.Services
{
    public class TrainingService
    {
        private static string connectionString = "Data Source=users.db";

        public static void AddTraining(Training training)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Trainings (Username, Date, Type, Duration, Calories)
                                 VALUES (@Username, @Date, @Type, @Duration, @Calories)";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", training.Username);
                cmd.Parameters.AddWithValue("@Date", training.Date);
                cmd.Parameters.AddWithValue("@Type", training.Type);
                cmd.Parameters.AddWithValue("@Duration", training.Duration);
                cmd.Parameters.AddWithValue("@Calories", training.Calories);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Training> GetTrainingsByDate(string username, string date)
        {
            var trainings = new List<Training>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Trainings WHERE Username = @Username AND Date = @Date";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Date", date);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    trainings.Add(new Training
                    {
                        Id = (int)(long)reader["Id"],
                        Username = reader["Username"].ToString(),
                        Date = reader["Date"].ToString(),
                        Type = reader["Type"].ToString(),
                        Duration = double.Parse(reader["Duration"].ToString()),
                        Calories = double.Parse(reader["Calories"].ToString())
                    });
                }
            }
            return trainings;
        }

        public static void DeleteTraining(int trainingId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Trainings WHERE Id = @Id";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", trainingId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
