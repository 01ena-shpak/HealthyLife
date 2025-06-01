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

        private DateTime selectedDate = DateTime.Now;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDateDisplay();
            LoadAllDataForDate();
        }

        private void PreviousDate_Click(object sender, RoutedEventArgs e)
        {
            selectedDate = selectedDate.AddDays(-1);
            UpdateDateDisplay();
            LoadAllDataForDate();
        }

        private void NextDate_Click(object sender, RoutedEventArgs e)
        {
            selectedDate = selectedDate.AddDays(1);
            UpdateDateDisplay();
            LoadAllDataForDate();
        }

        private void UpdateDateDisplay()
        {
            SelectedDateText.Text = selectedDate.ToString("dd MMMM yyyy");
        }

        private void LoadAllDataForDate()
        {
            LoadWaterAmount();
            LoadCurrentWeight();
            LoadMealSummaries();
            LoadTrainingSummary();
            LoadNorms();
        }

        private void BreakfastButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MealPage(_mainFrame, "Сніданок", selectedDate.ToString("yyyy-MM-dd")));
        }

        private void LunchButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MealPage(_mainFrame, "Обід", selectedDate.ToString("yyyy-MM-dd")));
        }

        private void DinnerButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MealPage(_mainFrame, "Вечеря", selectedDate.ToString("yyyy-MM-dd")));
        }

        private void SnackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MealPage(_mainFrame, "Перекус", selectedDate.ToString("yyyy-MM-dd")));
        }

        private void TrainingButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new TrainingPage(_mainFrame, selectedDate.ToString("yyyy-MM-dd")));
        }

        private void MeasurementsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MeasurementsPage(_mainFrame, selectedDate.ToString("yyyy-MM-dd")));
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
            WaterIntakeService.UpdateWaterIntake(LoginRegisterPage.CurrentUsername, selectedDate.ToString("yyyy-MM-dd"), currentWaterAmount);
        }

        private void MinusWaterButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentWaterAmount >= 250)
            {
                currentWaterAmount -= 250;
                WaterAmountText.Text = $"{currentWaterAmount} мл";
                WaterIntakeService.UpdateWaterIntake(LoginRegisterPage.CurrentUsername, selectedDate.ToString("yyyy-MM-dd"), currentWaterAmount);
            }
        }
        private void LoadWaterAmount()
        {
            currentWaterAmount = WaterIntakeService.GetWaterAmountByDate(LoginRegisterPage.CurrentUsername, selectedDate.ToString("yyyy-MM-dd"));
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
            var trainings = TrainingService.GetTrainingsByDate(LoginRegisterPage.CurrentUsername, selectedDate.ToString("yyyy-MM-dd"));
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
            var meals = MealService.GetMealsByDate(LoginRegisterPage.CurrentUsername, selectedDate.ToString("yyyy-MM-dd"));

            LoadMealSummary("Сніданок", BreakfastSummary);
            LoadMealSummary("Обід", LunchSummary);
            LoadMealSummary("Вечеря", DinnerSummary);
            LoadMealSummary("Перекус", SnackSummary);
        }

        private void LoadMealSummary(string mealType, TextBlock textBlock)
        {
            var meals = MealService.GetMealsByDate(LoginRegisterPage.CurrentUsername, selectedDate.ToString("yyyy-MM-dd"))
                        .Where(m => m.MealType == mealType);
            if (meals.Any())
            {
                var cal = meals.Sum(m => m.Calories);
                var prot = meals.Sum(m => m.Proteins);
                var fats = meals.Sum(m => m.Fats);
                var carbs = meals.Sum(m => m.Carbs);
                textBlock.Text = $"{cal} ккал, {prot}/{fats}/{carbs} БЖВ";
            }
            else
            {
                textBlock.Text = "0 ккал";
            }
        }

        private void LoadTotalNutrients()
        {
            var meals = MealService.GetMealsByDate(LoginRegisterPage.CurrentUsername, selectedDate.ToString("yyyy-MM-dd"));
            var totalCarbs = meals.Sum(m => m.Carbs);
            var totalProteins = meals.Sum(m => m.Proteins);
            var totalFats = meals.Sum(m => m.Fats);

            // оновлюємо 
            CarbsProgressBar.Value = totalCarbs;
            CarbsText.Text = $"{totalCarbs} g";
            ProteinsProgressBar.Value = totalProteins;
            ProteinsText.Text = $"{totalProteins} g";
            FatsProgressBar.Value = totalFats;
            FatsText.Text = $"{totalFats} g";
        }

        private void LoadNorms()
        {
            var user = UserService.GetUserByUsername(LoginRegisterPage.CurrentUsername);
            if (user == null) return;

            // розрахунок норми калорій і бжу
            double bmr = user.Gender == "Жінка"
                ? 10 * user.Weight + 6.25 * user.Height - 5 * user.Age - 161
                : 10 * user.Weight + 6.25 * user.Height - 5 * user.Age + 5;

            double activity = 1.2;
            if (user.Lifestyle == "Помірний")
                activity = 1.55;
            else if (user.Lifestyle == "Активний")
                activity = 1.725;

            double normCal = bmr * activity;
            double normProteins = (normCal * 0.3) / 4;
            double normFats = (normCal * 0.25) / 9;
            double normCarbs = (normCal * 0.45) / 4;

            // загальний спожиток за день
            var meals = MealService.GetMealsByDate(LoginRegisterPage.CurrentUsername, selectedDate.ToString("yyyy-MM-dd"));
            double eatenCal = meals.Sum(m => m.Calories);
            double eatenProt = meals.Sum(m => m.Proteins);
            double eatenFat = meals.Sum(m => m.Fats);
            double eatenCarb = meals.Sum(m => m.Carbs);

            var trainings = TrainingService.GetTrainingsByDate(LoginRegisterPage.CurrentUsername, selectedDate.ToString("yyyy-MM-dd"));
            double burnedCal = trainings.Sum(t => t.Calories);

            // оновлення значень
            CaloriesEatenText.Text = $"{Math.Round(eatenCal)}";
            CaloriesBurnedText.Text = $"{Math.Round(burnedCal)}";
            double calLeft = normCal - eatenCal + burnedCal;
            CaloriesOverText.Text = $"{Math.Round(calLeft)}";
            CaloriesStatusText.Text = calLeft >= 0 ? "Ккал залишилось" : "З’їдено більше";

            // бжу прогресбари
            ProteinsProgressBar.Value = Math.Min(100, eatenProt / normProteins * 100);
            FatsProgressBar.Value = Math.Min(100, eatenFat / normFats * 100);
            CarbsProgressBar.Value = Math.Min(100, eatenCarb / normCarbs * 100);

            ProteinsText.Text = $"{Math.Round(eatenProt)}/{Math.Round(normProteins)} г";
            FatsText.Text = $"{Math.Round(eatenFat)}/{Math.Round(normFats)} г";
            CarbsText.Text = $"{Math.Round(eatenCarb)}/{Math.Round(normCarbs)} г";
        }
    }
}
