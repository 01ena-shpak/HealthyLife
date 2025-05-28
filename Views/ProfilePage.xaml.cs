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
        }

        public ProfilePage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new DashboardPage(_mainFrame));
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LoginRegisterPage(_mainFrame));
        }

    }
}
