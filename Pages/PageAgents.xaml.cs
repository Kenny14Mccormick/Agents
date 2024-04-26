
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
        private List<Agent> allAgents;
        private List<Agent> filteredAgents;
        private List<Agent> sortedAgents;

        int count = 0;
        List<Agent> allSelect;

        private int currentPageIndex = 0;
        private int itemsPerPage = 10;

        public PageAgents()
        {
            InitializeComponent();
            allAgents = MainWindow.АгенствоПопрыгун.Agent.ToList();
            sortedAgents = allAgents.ToList();
            filteredAgents = new List<Agent>();

            allSelect = new List<Agent>();

            ShowCurrentPage();

            tbAgentTitle.TextChanged += FilterAgents;
            tbEmailPhone.TextChanged += FilterAgents;
            cbType.SelectionChanged += FilterAgents;
            cbTitleSort.SelectionChanged += FilterAgents;
            cbDiscountSort.SelectionChanged += FilterAgents;
            cbPrioritetSort.SelectionChanged += FilterAgents;

            var agentTypes = allAgents.Select(agent => agent.AgentType.Title).Distinct().ToList();

            // Добавляем "Все типы" как первый элемент в список
            agentTypes.Insert(0, "Все типы");

            // Заполняем комбобокс
            cbType.ItemsSource = agentTypes;
            cbType.SelectedIndex = 0;
            cbTitleSort.SelectionChanged += TitleSort_SelectionChanged;
            cbDiscountSort.SelectionChanged += DiscountSort_SelectionChanged;
            cbPrioritetSort.SelectionChanged += PrioritySort_SelectionChanged;
        }

        private void TitleSort_SelectionChanged(object sender, EventArgs e)
        {
            ApplySort();
        }

        private void DiscountSort_SelectionChanged(object sender, EventArgs e)
        {
            ApplySort();
        }

        private void PrioritySort_SelectionChanged(object sender, EventArgs e)
        {
            ApplySort();
        }

        private void ApplySort()
        {
            // Применяем сортировку к уже отфильтрованным агентам
            var query = filteredAgents.AsQueryable();

            // Применяем сортировку для каждого поля
            if (cbTitleSort.SelectedIndex > 0)
            {
                if (cbTitleSort.SelectedIndex == 1)
                    query = query.OrderBy(agent => agent.Title);
                else
                    query = query.OrderByDescending(agent => agent.Title);
            }

            if (cbDiscountSort.SelectedIndex > 0)
            {
                if (cbDiscountSort.SelectedIndex == 1)
                    query = query.OrderBy(agent => agent.Discount);
                else
                    query = query.OrderByDescending(agent => agent.Discount);
            }

            if (cbPrioritetSort.SelectedIndex > 0)
            {
                if (cbPrioritetSort.SelectedIndex == 1)
                    query = query.OrderBy(agent => agent.Priority);
                else
                    query = query.OrderByDescending(agent => agent.Priority);
            }

            // Преобразуем результат сортировки обратно в список
            filteredAgents = query.ToList();

            // Показываем текущую страницу
            ShowCurrentPage();
        }




        private void FilterAgents(object sender, EventArgs e)
        {
            // Применяем фильтры
            filteredAgents = allAgents.ToList();

            if (!string.IsNullOrEmpty(tbAgentTitle.Text))
                filteredAgents = filteredAgents.Where(agent => agent.Title.ToLower().Contains(tbAgentTitle.Text.ToLower())).ToList();

            if (!string.IsNullOrEmpty(tbEmailPhone.Text))
                filteredAgents = filteredAgents.Where(agent => agent.Email.ToLower().Contains(tbEmailPhone.Text.ToLower()) || agent.Phone.ToLower().Contains(tbEmailPhone.Text.ToLower())).ToList();

            if (cbType.SelectedIndex > 0)
                filteredAgents = filteredAgents.Where(agent => agent.AgentType.Title == cbType.SelectedItem.ToString()).ToList();


            lvAgents.ItemsSource = filteredAgents.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage).ToList();

            ShowCurrentPage();
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


        private void ChangePriority_Click(object sender, RoutedEventArgs e)
        {
            new ChangePriorityModalWindow(allSelect).ShowDialog();

            lvAgents.ItemsSource = null;

            ShowCurrentPage();

            allSelect.Clear();

            count = 0;
            btnChangePriority.IsEnabled = false;
        }

        private void lvAgents_DoulbeChanged(object sender, MouseButtonEventArgs e)
        {
            count++;

            var item = sender as ListView;

            var selectedItem = item.SelectedItem as Agent;

            if (count >= 1) btnChangePriority.IsEnabled = true;

            allSelect.Add(selectedItem);
        }

        private void AddAgent_Click(object sender, RoutedEventArgs e)
        {
            var agent = new Agent();
            WinEditAgent winEditAgent = new WinEditAgent(agent);
            winEditAgent.ShowDialog();
            // Обновляем DataGrid
            lvAgents.ItemsSource = null;
            allAgents = MainWindow.АгенствоПопрыгун.Agent.ToList();
            sortedAgents = allAgents.ToList();
            filteredAgents = allAgents.ToList();
            ShowCurrentPage();
        }

        private void EditAgent_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedAgent = editButton.DataContext as Agent;
            WinEditAgent winEditAgent = new WinEditAgent(selectedAgent);
            //editButton.IsEnabled = false;

            foreach (var agent in filteredAgents)
            {
                agent.IsEditEnabled = false;
            }
            winEditAgent.ShowDialog();

            //editButton.IsEnabled = true;
            foreach (var agent in filteredAgents)
            {
                agent.IsEditEnabled = true;
            }
            // Обновляем DataGrid
            lvAgents.ItemsSource = null;

            allAgents = MainWindow.АгенствоПопрыгун.Agent.ToList();
            sortedAgents = allAgents.ToList();
            filteredAgents = allAgents.ToList();

            ShowCurrentPage();
        }

        private void Sales_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedAgent = editButton.DataContext as Agent;
            AgentSales agentSales = new AgentSales(selectedAgent);
            editButton.IsEnabled = false;
            agentSales.ShowDialog();
            editButton.IsEnabled = true;
            // Обновляем DataGrid
            lvAgents.ItemsSource = null;

            ShowCurrentPage();
        }
    }
}
