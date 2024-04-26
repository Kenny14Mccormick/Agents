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
    /// Логика взаимодействия для ChangePriorityModalWindow.xaml
    /// </summary>
    public partial class ChangePriorityModalWindow : Window
    {
        List<Agent> selectAgents;

        public ChangePriorityModalWindow(List<Agent> selectAgents)
        {
            InitializeComponent();
            this.selectAgents = selectAgents;

            int max = selectAgents.Select(item => item.Priority).Max();

            tbNewPriority.Text = max.ToString();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            int newPriority;
            if (!int.TryParse(tbNewPriority.Text, out newPriority) || newPriority < 0)
            {
                MessageBox.Show("Приоритет не может быть отрицательным!");
                return;
            }

            for (int i = 0; i < selectAgents.Count(); i++)
            {
                // Создаем новую запись об изменении приоритета
                AgentPriorityHistory history = new AgentPriorityHistory
                {
                    AgentID = selectAgents[i].ID,
                    ChangeDate = DateTime.Now,
                    PriorityValue = newPriority
                };

                // Добавляем запись в базу данных
                MainWindow.АгенствоПопрыгун.AgentPriorityHistory.Add(history);

                // Обновляем приоритет агента
                selectAgents[i].Priority = newPriority;
            }

            MainWindow.АгенствоПопрыгун.SaveChanges();
            Close();
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
