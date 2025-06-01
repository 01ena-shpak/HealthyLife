using HealthyLife.Models;
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
    /// Interaction logic for MealPage.xaml
    /// </summary>
    public partial class MealPage : Page
    {
        private Frame _mainFrame;
        private string _mealType;
        private string _selectedDate;

        public MealPage()
        {
            InitializeComponent();
        }

        public MealPage(Frame mainFrame, string mealType, string selectedDate)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _mealType = mealType;
            _selectedDate = selectedDate;
            MealTitle.Text = mealType;
            LoadMeals();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.GoBack();
        }

        private void ClearFields()
        {
            DescriptionTextBox.Text = "";
            GramsTextBox.Text = "";
            CaloriesTextBox.Text = "";
            ProteinsTextBox.Text = "";
            FatsTextBox.Text = "";
            CarbsTextBox.Text = "";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text) || string.IsNullOrWhiteSpace(CaloriesTextBox.Text) || string.IsNullOrWhiteSpace(GramsTextBox.Text))
            {
                MessageBox.Show("Будь ласка, заповніть назву страви, калорійність та вагу порції.");
                return;
            }

            // зчитування ваги порції
            var grams = double.TryParse(GramsTextBox.Text, out var gr) ? gr : 100;

            // розрахунок кбжу на основі введеної ваги
            var meal = new Meal
            {
                Username = LoginRegisterPage.CurrentUsername,
                Date = _selectedDate,
                MealType = _mealType,
                Description = DescriptionTextBox.Text,
                Grams = grams,
                Calories = (double.TryParse(CaloriesTextBox.Text, out var cal) ? cal : 0) * grams / 100,
                Proteins = (double.TryParse(ProteinsTextBox.Text, out var prot) ? prot : 0) * grams / 100,
                Fats = (double.TryParse(FatsTextBox.Text, out var fat) ? fat : 0) * grams / 100,
                Carbs = (double.TryParse(CarbsTextBox.Text, out var carb) ? carb : 0) * grams / 100
            };

            MealService.AddMeal(meal);
            MessageBox.Show("Прийом їжі додано!");
            ClearFields();
            LoadMeals();
        }

        private void LoadMeals()
        {
            var meals = MealService.GetMealsByDate(LoginRegisterPage.CurrentUsername, _selectedDate)
                           .Where(m => m.MealType == _mealType)
                           .Select(m => new
                           {
                               Id = m.Id,
                               Display = $"{m.Description}: {m.Calories} ккал, {m.Proteins}/{m.Fats}/{m.Carbs} БЖВ, {m.Grams} г"
                           }).ToList();

            MealsListBox.ItemsSource = meals;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is int mealId)
            {
                if (MessageBox.Show("Видалити цю страву?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    MealService.DeleteMeal(mealId);
                    LoadMeals();
                }
            }
        }
    }
}
