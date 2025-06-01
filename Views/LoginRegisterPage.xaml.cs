using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HealthyLife.Views
{
    /// <summary>
    /// Interaction logic for LoginRegisterPage.xaml
    /// </summary>
    public partial class LoginRegisterPage : Page
    {
        private Frame _mainFrame;
        public LoginRegisterPage()
        {
            InitializeComponent();
        }

        public LoginRegisterPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        public static string CurrentUsername; // стат змінна для збереження логіну
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(LoginUsername.Text))
            {
                LoginUsername.Background = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
                isValid = false;
            }
            else
            {
                LoginUsername.ClearValue(TextBox.BackgroundProperty);
            }

            if (string.IsNullOrWhiteSpace(LoginPassword.Password))
            {
                LoginPassword.Background = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
                isValid = false;
            }
            else
            {
                LoginPassword.ClearValue(PasswordBox.BackgroundProperty);
            }

            if (!isValid)
            {
                MessageBox.Show("Будь ласка, заповніть усі поля для входу.");
                return;
            }

            if (Services.UserService.AuthenticateUser(LoginUsername.Text, LoginPassword.Password))
            {
                CurrentUsername = LoginUsername.Text; // зберігаємо логін
                MessageBox.Show("Вхід успішний!");
                _mainFrame.Navigate(new DashboardPage(_mainFrame));
            }
            else
            {
                // підсвічуємо обидва поля червоним
                LoginUsername.Background = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
                LoginPassword.Background = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            // перевірка логіну
            if (string.IsNullOrWhiteSpace(RegisterUsername.Text))
            {
                RegisterUsername.Background = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0)); // червоний фон
                isValid = false;
            }
            else
            {
                RegisterUsername.ClearValue(TextBox.BackgroundProperty);
            }

            // перевірка електронної пошти
            if (string.IsNullOrWhiteSpace(RegisterEmail.Text) ||
                !RegisterEmail.Text.Contains("@") ||
                !RegisterEmail.Text.Contains("."))
            {
                RegisterEmail.Background = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
                isValid = false;
            }
            else
            {
                RegisterEmail.ClearValue(TextBox.BackgroundProperty);
            }

            // перевірка пароля
            if (string.IsNullOrWhiteSpace(RegisterPassword.Password) || RegisterPassword.Password.Length < 5)
            {
                RegisterPassword.Background = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
                isValid = false;
            }
            else
            {
                RegisterPassword.ClearValue(PasswordBox.BackgroundProperty);
            }

            // перевірка підтвердження пароля
            if (string.IsNullOrWhiteSpace(RegisterConfirmPassword.Password) ||
                RegisterConfirmPassword.Password != RegisterPassword.Password)
            {
                RegisterConfirmPassword.Background = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
                isValid = false;
            }
            else
            {
                RegisterConfirmPassword.ClearValue(PasswordBox.BackgroundProperty);
            }

            if (!isValid)
            {
                MessageBox.Show("Будь ласка, заповніть усі обов’язкові поля або перевірте паролі.");
                return;
            }

            var user = new Models.User
            {
                Username = RegisterUsername.Text,
                Email = RegisterEmail.Text,
                Password = RegisterPassword.Password,
                Height = 0,
                Weight = 0,
                Age = 0,
                Lifestyle = "Не визначено",
                Goal = "Не визначено",
                Gender = "Не визначено"
            };

            if (Services.UserService.RegisterUser(user))
            {
                _mainFrame.Navigate(new DashboardPage(_mainFrame));
            }
        }
    }
}
