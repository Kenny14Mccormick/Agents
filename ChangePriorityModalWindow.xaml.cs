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
            for (int i = 0; i < selectAgents.Count(); i++)
            {
                selectAgents[i].Priority = int.Parse(tbNewPriority.Text);
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
