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
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWaterAmount();
            LoadCurrentWeight();
            LoadMealSummaries();
            LoadTrainingSummary();
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

        private void LoadCurrentWeight()
        {
            var user = UserService.GetUserByUsername(LoginRegisterPage.CurrentUsername);
            if (user != null)
            {
                CurrentWeightText.Text = user.Weight > 0 ? $"{user.Weight} кг" : "Немає запису";
            }
        }

        private void LoadTrainingSummary()
        {
            var trainings = TrainingService.GetTrainingsByDate(LoginRegisterPage.CurrentUsername, DateTime.Now.ToString("yyyy-MM-dd"));
            if (trainings.Any())
            {
                var totalCalories = trainings.Sum(t => t.Calories);
                var totalDuration = trainings.Sum(t => t.Duration);
                TrainingSummaryText.Text = $"{totalDuration} хв, {totalCalories} ккал";
            }
            else
            {
                TrainingSummaryText.Text = "Немає запису";
            }
        }

        private void LoadMealSummaries()
        {
            var meals = MealService.GetMealsByDate(LoginRegisterPage.CurrentUsername, DateTime.Now.ToString("yyyy-MM-dd"));

            LoadMealSummary("Сніданок", BreakfastSummary);
            LoadMealSummary("Обід", LunchSummary);
            LoadMealSummary("Вечеря", DinnerSummary);
            LoadMealSummary("Перекус", SnackSummary);
        }

        private void LoadMealSummary(string mealType, TextBlock textBlock)
        {
            var meals = MealService.GetMealsByDate(LoginRegisterPage.CurrentUsername, DateTime.Now.ToString("yyyy-MM-dd"))
                        .Where(m => m.MealType == mealType);
            if (meals.Any())
            {
                var cal = meals.Sum(m => m.Calories);
                var prot = meals.Sum(m => m.Proteins);
                var fats = meals.Sum(m => m.Fats);
                var carbs = meals.Sum(m => m.Carbs);
                textBlock.Text = $"{cal} ккал, {prot}/{fats}/{carbs} БЖУ";
            }
            else
            {
                textBlock.Text = "0 ккал";
            }
        }


    }
}
