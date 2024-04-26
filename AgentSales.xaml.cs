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
using System.Windows.Shapes;

namespace Попрыженок
{
    /// <summary>
    /// Логика взаимодействия для AgentSales.xaml
    /// </summary>
    public partial class AgentSales : Window
    {
        private Agent agent;
        public AgentSales(Agent agent)
        {
            InitializeComponent();
            this.agent = agent;
            DataContext = agent;
            dgSales.ItemsSource = MainWindow.АгенствоПопрыгун.ProductSale.Where(ps=>ps.AgentID==agent.ID).ToList();
        }

        private void btnDeleteSale_Click(object sender, RoutedEventArgs e)
        {
            ProductSale selectedSale = dgSales.SelectedItem as ProductSale;

            if (selectedSale != null)
            {

                if (selectedSale != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Вы точно хотите удалить продажу?",
                        "Подтверждение об удалении",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        MainWindow.АгенствоПопрыгун.ProductSale.Remove(selectedSale);
                        MainWindow.АгенствоПопрыгун.SaveChanges();
                        MessageBox.Show("Запись удалена!");
                        dgSales.ItemsSource = null;
                        dgSales.ItemsSource = MainWindow.АгенствоПопрыгун.ProductSale.Where(ps => ps.AgentID == agent.ID).ToList();

                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddProductSale(object sender, RoutedEventArgs e)
        {
            ProductSale productSale = new ProductSale();

            productSale.Agent = agent;
            WinAddProductSaleForAgent winAddProductSaleForAgent = new WinAddProductSaleForAgent(productSale);
            winAddProductSaleForAgent.ShowDialog();

            // Проверяем, были ли внесены изменения
            if (winAddProductSaleForAgent.IsSaved)
            {
                MainWindow.АгенствоПопрыгун.ProductSale.Add(productSale);
                MainWindow.АгенствоПопрыгун.SaveChanges();
                dgSales.ItemsSource = null;
                dgSales.ItemsSource = MainWindow.АгенствоПопрыгун.ProductSale.Where(ps => ps.AgentID == agent.ID).ToList();
            }
        }

    }
}

