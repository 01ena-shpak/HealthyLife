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
    /// Interaction logic for LoginRegisterPage.xaml
    /// </summary>
    public partial class LoginRegisterPage : Page
    {
        private Frame _mainFrame;
        public LoginRegisterPage()
        {
            InitializeComponent();
        }

        public LoginRegisterPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new DashboardPage(_mainFrame));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new DashboardPage(_mainFrame));
        }
    }
}
