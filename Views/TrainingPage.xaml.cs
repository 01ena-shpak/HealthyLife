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
    /// Interaction logic for TrainingPage.xaml
    /// </summary>
    public partial class TrainingPage : Page
    {
        private Frame _mainFrame;
        private string _selectedDate;
        public TrainingPage()
        {
            InitializeComponent();
        }

        public TrainingPage(Frame mainFrame, string selectedDate)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _selectedDate = selectedDate;
            LoadTrainings();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.GoBack();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (TypeComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(DurationTextBox.Text) || string.IsNullOrWhiteSpace(CaloriesTextBox.Text))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.");
                return;
            }

            var training = new Training
            {
                Username = LoginRegisterPage.CurrentUsername,
                Date = _selectedDate,
                Type = (TypeComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                Duration = double.TryParse(DurationTextBox.Text, out var duration) ? duration : 0,
                Calories = double.TryParse(CaloriesTextBox.Text, out var calories) ? calories : 0
            };

            TrainingService.AddTraining(training);
            MessageBox.Show("Тренування додано!");
            ClearFields();
            LoadTrainings();
        }

        private void ClearFields()
        {
            TypeComboBox.SelectedIndex = -1;
            DurationTextBox.Text = "";
            CaloriesTextBox.Text = "";
        }

        private void LoadTrainings()
        {
            var trainings = TrainingService.GetTrainingsByDate(LoginRegisterPage.CurrentUsername, DateTime.Now.ToString("yyyy-MM-dd"))
                           .Select(t => new
                           {
                               Id = t.Id,
                               Display = $"{t.Type}: {t.Calories} ккал, {t.Duration} хв"
                           }).ToList();

            TrainingListBox.ItemsSource = trainings;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is int trainingId)
            {
                if (MessageBox.Show("Видалити це тренування?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    TrainingService.DeleteTraining(trainingId);
                    LoadTrainings();
                }
            }
        }
    }
}
