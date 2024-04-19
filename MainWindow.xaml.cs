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

namespace Попрыженок
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static АгенствоПопрыгун АгенствоПопрыгун;
        public MainWindow()
        {
            InitializeComponent();
            АгенствоПопрыгун = new АгенствоПопрыгун();
        }

        private void BtnPageAgents_Click(object sender, RoutedEventArgs e)
        {
            btnElse.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F9969E"));
            BtnPageAgents.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#43DCFE"));

            MainFrame.NavigationService.Navigate(new Pages.PageAgents());
        }

        private void btnElse_Click(object sender, RoutedEventArgs e)
        {
            btnElse.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#43DCFE"));
            BtnPageAgents.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F9969E"));
            MainFrame.Content = null;
        }
    }
}
