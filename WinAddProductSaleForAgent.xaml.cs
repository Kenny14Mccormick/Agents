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
    /// Логика взаимодействия для WinAddProductSaleForAgent.xaml
    /// </summary>
    public partial class WinAddProductSaleForAgent : Window
    {
        ProductSale productSale;

        List<string> nameProduct;

        List<Product> products;

        // Добавляем свойство IsSaved
        public bool IsSaved { get; private set; }

        public WinAddProductSaleForAgent(ProductSale productSale)
        {
            InitializeComponent();
            List<Product> temp = MainWindow.АгенствоПопрыгун.Product.ToList();

            nameProduct = new List<string>();

            foreach (var item in temp)
            {
                nameProduct.Add(item.Title);
            }

            products = MainWindow.АгенствоПопрыгун.Product.ToList();

            cbProduct.ItemsSource = nameProduct;

            this.productSale = productSale;
        }

        private void btn_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbSearch.Text.ToLower();
            List<string> filteredProducts = nameProduct.Where(p => p.ToLower().Contains(searchText)).ToList();
            cbProduct.ItemsSource = filteredProducts;
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            productSale.Product = products[cbProduct.SelectedIndex];

            productSale.SaleDate = DateTime.Now;

            try
            {
                productSale.ProductCount = int.Parse(productCount.Text);

                if (int.Parse(productCount.Text) <= 0)
                {
                    throw new Exception("Количество не может быть отрицательным!");
                }
            }
            catch
            {
                productSale.ProductCount = 0;
            }

            // Устанавливаем IsSaved в true, когда сохраняем изменения
            IsSaved = true;

            MessageBox.Show("Успешно!");

            Close();
        }
    }
}