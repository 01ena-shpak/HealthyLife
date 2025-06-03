using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using HealthyLife.Models;
using System.Windows;

namespace HealthyLife.Services
{
    public class UserService
    {
        private static string connectionString = "Data Source=users.db";

        public static bool RegisterUser(User user)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = @"INSERT INTO Users (Username, Email, Password, Height, Weight, Age, Lifestyle, Goal, Gender)
                                       VALUES (@Username, @Email, @Password, @Height, @Weight, @Age, @Lifestyle, @Goal, @Gender)";
                SQLiteCommand cmd = new SQLiteCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Height", user.Height);
                cmd.Parameters.AddWithValue("@Weight", user.Weight);
                cmd.Parameters.AddWithValue("@Age", user.Age);
                cmd.Parameters.AddWithValue("@Lifestyle", user.Lifestyle);
                cmd.Parameters.AddWithValue("@Goal", user.Goal);
                cmd.Parameters.AddWithValue("@Gender", user.Gender);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Реєстрація успішна!");
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка реєстрації: " + ex.Message);
                    return false;
                }
            }
        }
        
        public static bool AuthenticateUser(string username, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public static User GetUserByUsername(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        Height = Convert.ToDouble(reader["Height"]),
                        Weight = Convert.ToDouble(reader["Weight"]),
                        Age = Convert.ToInt32(reader["Age"]),
                        Lifestyle = reader["Lifestyle"].ToString(),
                        Goal = reader["Goal"].ToString(),
                        Gender = reader["Gender"].ToString()
                    };
                }
                return null;
            }
        }

        public static void UpdateUserProfile(User user)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE Users SET 
                            Height = @Height,
                            Weight = @Weight,
                            Age = @Age,
                            Lifestyle = @Lifestyle,
                            Goal = @Goal,
                            Gender = @Gender
                         WHERE Username = @Username";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Height", user.Height);
                cmd.Parameters.AddWithValue("@Weight", user.Weight);
                cmd.Parameters.AddWithValue("@Age", user.Age);
                cmd.Parameters.AddWithValue("@Lifestyle", user.Lifestyle);
                cmd.Parameters.AddWithValue("@Goal", user.Goal);
                cmd.Parameters.AddWithValue("@Gender", user.Gender);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateUserWeight(string username, double weight)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE Users SET Weight = @Weight WHERE Username = @Username";
                var cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@Weight", weight);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
