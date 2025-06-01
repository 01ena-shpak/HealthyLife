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
using HealthyLife.Strategies;
using HealthyLife.Services;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Wpf;

namespace HealthyLife.Views
{
    /// <summary>
    /// Interaction logic for StatsPage.xaml
    /// </summary>
    public partial class StatsPage : Page
    {
        private Frame _mainFrame;
        private IStatsStrategy _currentStrategy;
        public StatsPage()
        {
            InitializeComponent();
        }

        public StatsPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _currentStrategy = new WeekStrategy(); // за на тиждень
            LoadStats();
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new DashboardPage(_mainFrame));
        }

        private void PeriodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PeriodComboBox.SelectedIndex == 0)
                _currentStrategy = new WeekStrategy();
            else if (PeriodComboBox.SelectedIndex == 1)
                _currentStrategy = new MonthStrategy();
            else
                _currentStrategy = new AllTimeStrategy();

            LoadStats();
        }

        private void LoadStats()
        {
            string username = LoginRegisterPage.CurrentUsername;

            var meals = MealService.GetMealsByDateRange(username, _currentStrategy.StartDate, _currentStrategy.EndDate);
            var water = WaterIntakeService.GetWaterIntakeByDateRange(username, _currentStrategy.StartDate, _currentStrategy.EndDate);
            var trainings = TrainingService.GetTrainingsByDateRange(username, _currentStrategy.StartDate, _currentStrategy.EndDate);
            var measurements = MeasurementService.GetMeasurementsByDateRange(username, _currentStrategy.StartDate, _currentStrategy.EndDate);

            // групування для графіків
            CaloriesChart.Content = CreatePlot(
                meals.GroupBy(m => m.Date)
                     .Select(g => new DataPoint(DateTimeAxis.ToDouble(DateTime.Parse(g.Key)), g.Sum(m => m.Calories))),
                "Калорії");

            WaterChart.Content = CreatePlot(
                water.GroupBy(w => w.Date)
                     .Select(g => new DataPoint(DateTimeAxis.ToDouble(DateTime.Parse(g.Key)), g.Sum(w => w.Amount))),
                "Вода (мл)");

            TrainingChart.Content = CreatePlot(
                trainings.GroupBy(t => t.Date)
                         .Select(g => new DataPoint(DateTimeAxis.ToDouble(DateTime.Parse(g.Key)), g.Sum(t => t.Calories))),
                "Тренування (ккал)");

            WeightChart.Content = CreatePlot(
                measurements.GroupBy(m => m.Date)
                            .Select(g => new DataPoint(DateTimeAxis.ToDouble(DateTime.Parse(g.Key)), g.Last().Weight)),
                "Вага");

            MeasurementsChart.Content = CreateMeasurementPlot(measurements);
        }

        private PlotView CreatePlot(IEnumerable<DataPoint> dataPoints, string title)
        {
            var model = new PlotModel { Title = $"Графік {title}" };
            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd.MM",
                IntervalType = DateTimeIntervalType.Days,
                MinorIntervalType = DateTimeIntervalType.Days
            });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var series = new LineSeries
            {
                Color = OxyColors.Green,
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                MarkerSize = 3
            };
            series.Points.AddRange(dataPoints.OrderBy(p => p.X));
            model.Series.Add(series);

            return new OxyPlot.Wpf.PlotView { Model = model, Height = 300 };
        }

        private PlotView CreateMeasurementPlot(IEnumerable<Models.Measurement> measurements)
        {
            var model = new PlotModel { Title = "Груди/Талія/Стегна" };

            var chestSeries = new LineSeries { Title = "Груди", Color = OxyColors.Red, MarkerType = MarkerType.Circle };
            var waistSeries = new LineSeries { Title = "Талія", Color = OxyColors.Orange, MarkerType = MarkerType.Circle };
            var hipsSeries = new LineSeries { Title = "Стегна", Color = OxyColors.Green, MarkerType = MarkerType.Circle };

            var grouped = measurements
                .Select(m => new
                {
                    Date = DateTime.Parse(m.Date).Date,  // лише дата без часу
                    Measurement = m
                })
                .GroupBy(x => x.Date)
                .Select(g => g.OrderByDescending(x => x.Measurement.Id).First().Measurement)
                .OrderBy(m => DateTime.Parse(m.Date).Date);  // сортуємо за датою

            foreach (var m in grouped)
            {
                var dateX = DateTimeAxis.ToDouble(DateTime.Parse(m.Date));
                chestSeries.Points.Add(new DataPoint(dateX, m.Chest));
                waistSeries.Points.Add(new DataPoint(dateX, m.Waist));
                hipsSeries.Points.Add(new DataPoint(dateX, m.Hips));
            }

            model.Series.Add(chestSeries);
            model.Series.Add(waistSeries);
            model.Series.Add(hipsSeries);
            model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "dd.MM", MajorGridlineStyle = LineStyle.Solid });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MajorGridlineStyle = LineStyle.Solid });

            return new OxyPlot.Wpf.PlotView { Model = model, Height = 300 };
        }
    }
}
