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
    internal class UserService
    {
        // шлях до бази даних
        private static string connectionString = "Data Source=users.db";

        // ініціалізація таблиці (створення, якщо не існує)
        public static void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS Users (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Username TEXT NOT NULL UNIQUE,
                                            Email TEXT NOT NULL,
                                            Password TEXT NOT NULL,
                                            Height REAL,
                                            Weight REAL,
                                            Age INTEGER,
                                            Lifestyle TEXT
                                        )";
                SQLiteCommand command = new SQLiteCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
            }
        }

        // Реєстрація нового користувача
        public static bool RegisterUser(User user)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = @"INSERT INTO Users (Username, Email, Password, Height, Weight, Age, Lifestyle)
                                       VALUES (@Username, @Email, @Password, @Height, @Weight, @Age, @Lifestyle)";
                SQLiteCommand cmd = new SQLiteCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Height", user.Height);
                cmd.Parameters.AddWithValue("@Weight", user.Weight);
                cmd.Parameters.AddWithValue("@Age", user.Age);
                cmd.Parameters.AddWithValue("@Lifestyle", user.Lifestyle);

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
    }
}
