using HealthyLife.Models;
using HealthyLife.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
    /// Interaction logic for MeasurementsPage.xaml
    /// </summary>
    public partial class MeasurementsPage : Page
    {
        private Frame _mainFrame;
        private string _selectedDate;
        public MeasurementsPage()
        {
            InitializeComponent();
        }

        public MeasurementsPage(Frame mainFrame, string selectedDate)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _selectedDate = selectedDate;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.GoBack();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(WeightTextBox.Text))
            {
                MessageBox.Show("Введіть вагу");
                return;
            }

            var measurement = new Measurement
            {
                Username = LoginRegisterPage.CurrentUsername,
                Date = _selectedDate,
                Weight = double.TryParse(WeightTextBox.Text, out var weight) ? weight : 0,
                Chest = double.TryParse(ChestTextBox.Text, out var chest) ? chest : 0,
                Waist = double.TryParse(WaistTextBox.Text, out var waist) ? waist : 0,
                Hips = double.TryParse(HipsTextBox.Text, out var hips) ? hips : 0
            };
            MeasurementService.AddMeasurement(measurement);

            // оновлюємо тільки вагу користувача 
            UserService.UpdateUserWeight(LoginRegisterPage.CurrentUsername, measurement.Weight);

            MessageBox.Show("Заміри збережено та вага профілю оновлена!");
            _mainFrame.Navigate(new DiaryPage(_mainFrame));
        }
    }
}

