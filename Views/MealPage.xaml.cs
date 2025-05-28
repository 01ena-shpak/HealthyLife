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
        public MealPage()
        {
            InitializeComponent();
        }

        public MealPage(Frame mainFrame, string mealType)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            MealTitle.Text = mealType;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.GoBack();
        }

    }
}
