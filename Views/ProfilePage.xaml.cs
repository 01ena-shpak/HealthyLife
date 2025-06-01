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
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private Frame _mainFrame;
        public ProfilePage()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        public ProfilePage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            this.Loaded += Page_Loaded;
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new DashboardPage(_mainFrame));
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LoginRegisterPage(_mainFrame));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var user = Services.UserService.GetUserByUsername(LoginRegisterPage.CurrentUsername);
            if (user != null)
            {
                AgeTextBox.Text = user.Age > 0 ? user.Age.ToString() : "";
                HeightTextBox.Text = user.Height > 0 ? user.Height.ToString() : "";
                WeightTextBox.Text = user.Weight > 0 ? user.Weight.ToString() : "";

                // пошук потрібного елемента у ComboBox
                foreach (ComboBoxItem item in ActivityComboBox.Items)
                {
                    if (item.Content.ToString() == user.Lifestyle)
                    {
                        ActivityComboBox.SelectedItem = item;
                        break;
                    }
                }

                foreach (ComboBoxItem item in GoalComboBox.Items)
                {
                    if (item.Content.ToString() == user.Goal)
                    {
                        GoalComboBox.SelectedItem = item;
                        break;
                    }
                }

                foreach (ComboBoxItem item in GenderComboBox.Items)
                {
                    if (item.Content.ToString() == user.Gender)
                    {
                        GenderComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var user = new Models.User
            {
                Username = LoginRegisterPage.CurrentUsername,
                Age = int.TryParse(AgeTextBox.Text, out var age) ? age : 0,
                Height = double.TryParse(HeightTextBox.Text, out var height) ? height : 0,
                Weight = double.TryParse(WeightTextBox.Text, out var weight) ? weight : 0,
                Lifestyle = (ActivityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Goal = (GoalComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString()
            };

            Services.UserService.UpdateUserProfile(user);
            MessageBox.Show("Профіль оновлено!");
        }
    }
}
