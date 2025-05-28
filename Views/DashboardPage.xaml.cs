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
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        private Frame _mainFrame;
        public DashboardPage()
        {
            InitializeComponent();
        }
        public DashboardPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void DiaryButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new DiaryPage(_mainFrame));
        }

        private void StatsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new StatsPage(_mainFrame));
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new ProfilePage(_mainFrame));
        }
    }
}
