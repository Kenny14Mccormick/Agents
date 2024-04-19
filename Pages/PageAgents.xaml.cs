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

namespace Попрыженок.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAgents.xaml
    /// </summary>
    public partial class PageAgents : Page
    {
        private List<ProductSale> allAgents;
        public PageAgents()
        {
            InitializeComponent();
            allAgents = MainWindow.АгенствоПопрыгун.ProductSale.ToList();
            lvAgents.ItemsSource = allAgents;
        }
    }
}
