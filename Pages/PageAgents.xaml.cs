
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
    public partial class PageAgents : Page
    {
        private List<ProductSale> allAgents;
        private List<ProductSale> filteredAgents;
        private List<ProductSale> sortedAgents;

        private int currentPageIndex = 0;
        private int itemsPerPage = 10;

        public PageAgents()
        {
            InitializeComponent();
            allAgents = MainWindow.АгенствоПопрыгун.ProductSale.ToList();
            sortedAgents = allAgents.ToList();
            filteredAgents = new List<ProductSale>();

            UpdateProductCount();
            ShowCurrentPage();

            tbAgentTitle.TextChanged += FilterAgents;
            tbEmailPhone.TextChanged += FilterAgents;
            cbType.SelectionChanged += FilterAgents;
            cbTitleSort.SelectionChanged += FilterAgents;
            cbDiscountSort.SelectionChanged += FilterAgents;
            cbPrioritetSort.SelectionChanged += FilterAgents;

            var agentTypes = allAgents.Select(agent => agent.Agent.AgentType.Title).Distinct().ToList();

            // Добавляем "Все типы" как первый элемент в список
            agentTypes.Insert(0, "Все типы");

            // Заполняем комбобокс
            cbType.ItemsSource = agentTypes;
            cbType.SelectedIndex = 0;
        }

        private void FilterAgents(object sender, EventArgs e)
        {
            // Фильтрация агентов
            filteredAgents = allAgents.ToList();

            if (!string.IsNullOrEmpty(tbAgentTitle.Text))
                filteredAgents = filteredAgents.Where(agent => agent.Agent.Title.ToLower().Contains(tbAgentTitle.Text.ToLower())).ToList();

            if (!string.IsNullOrEmpty(tbEmailPhone.Text))
                filteredAgents = filteredAgents.Where(agent => agent.Agent.Email.ToLower().Contains(tbEmailPhone.Text.ToLower()) || agent.Agent.Phone.ToLower().Contains(tbEmailPhone.Text.ToLower())).ToList();

            if (cbType.SelectedIndex > 0)
                filteredAgents = filteredAgents.Where(agent => agent.Agent.AgentType.Title == cbType.SelectedItem.ToString()).ToList();

            // Сортировка
            if (cbTitleSort.SelectedIndex == 1)
                filteredAgents = filteredAgents.OrderByDescending(agent => agent.Agent.Title).ToList();
            else
                filteredAgents = filteredAgents.OrderBy(agent => agent.Agent.Title).ToList();

            if (cbDiscountSort.SelectedIndex == 1)
                filteredAgents = filteredAgents.OrderByDescending(agent => agent.Discount).ToList();
            else
                filteredAgents = filteredAgents.OrderBy(agent => agent.Discount).ToList();

            if (cbPrioritetSort.SelectedIndex == 1)
                filteredAgents = filteredAgents.OrderByDescending(agent => agent.Agent.Priority).ToList();
            else
                filteredAgents = filteredAgents.OrderBy(agent => agent.Agent.Priority).ToList();

            // Показываем отфильтрованные и отсортированные агенты
            lvAgents.ItemsSource = filteredAgents.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();
            ShowCurrentPage();
        }



        private void UpdateProductCount()
        {
            DateTime sixYearsAgo = DateTime.Today.AddYears(-10);

            foreach (var agent in allAgents)
            {
                if (agent.SaleDate < sixYearsAgo)
                {
                    agent.ProductCount = 0;
                }
            }
        }
        private void ShowCurrentPage()
        {
            lvAgents.ItemsSource = filteredAgents.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();


            wpPageNumbers.Children.Clear();
            for (int i = 0; i < (filteredAgents.Count - 1) / itemsPerPage + 1; i++)
            {
                Button pageButton = new Button
                {
                    Content = (i + 1).ToString(),
                    Margin = new Thickness(10, 0, 10, 0),
                    Background = i == currentPageIndex ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#43DCFE")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F9969E")),
                    Foreground = new SolidColorBrush(Colors.White)
                };
                pageButton.Click += PageButton_Click;
                wpPageNumbers.Children.Add(pageButton);
            }
        }


        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            currentPageIndex = int.Parse(button.Content.ToString()) - 1;
            ShowCurrentPage();
        }


        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex < (allAgents.Count - 1) / itemsPerPage)
            {
                currentPageIndex++;
                ShowCurrentPage();
            }
        }


        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                ShowCurrentPage();
            }
        }
    }
}
