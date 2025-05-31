using HealthyLife.Services;
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
    /// Interaction logic for DiaryPage.xaml
    /// </summary>
    public partial class DiaryPage : Page
    {
        private Frame _mainFrame;
        public DiaryPage()
        {
            InitializeComponent();
        }
        public DiaryPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            LoadWaterAmount();
        }

        private void BreakfastButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MealPage(_mainFrame, "Сніданок"));
        }

        private void LunchButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MealPage(_mainFrame, "Обід"));
        }

        private void DinnerButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MealPage(_mainFrame, "Вечеря"));
        }

        private void SnackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MealPage(_mainFrame, "Перекус"));
        }

        private void TrainingButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new TrainingPage(_mainFrame));
        }

        private void MeasurementsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MeasurementsPage(_mainFrame));
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new DashboardPage(_mainFrame));
        }

        private double currentWaterAmount = 0; // збереження кількості води

        private void PlusWaterButton_Click(object sender, RoutedEventArgs e)
        {
            currentWaterAmount += 250;
            WaterAmountText.Text = $"{currentWaterAmount} мл";
            WaterIntakeService.UpdateWaterIntake(LoginRegisterPage.CurrentUsername, DateTime.Now.ToString("yyyy-MM-dd"), currentWaterAmount);
        }

        private void MinusWaterButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentWaterAmount >= 250)
            {
                currentWaterAmount -= 250;
                WaterAmountText.Text = $"{currentWaterAmount} мл";
                WaterIntakeService.UpdateWaterIntake(LoginRegisterPage.CurrentUsername, DateTime.Now.ToString("yyyy-MM-dd"), currentWaterAmount);
            }
        }
        private void LoadWaterAmount()
        {
            currentWaterAmount = WaterIntakeService.GetWaterAmountByDate(LoginRegisterPage.CurrentUsername, DateTime.Now.ToString("yyyy-MM-dd"));
            WaterAmountText.Text = $"{currentWaterAmount} мл";
        }
    }
}
