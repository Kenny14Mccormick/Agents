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
    /// Логика взаимодействия для WinEditAgent.xaml
    /// </summary>
    public partial class WinEditAgent : Window
    {
        private Agent agent;
        public WinEditAgent(Agent agent)
        {
            InitializeComponent();
            this.agent = agent;
            DataContext = agent;
            cbType.ItemsSource = MainWindow.АгенствоПопрыгун.AgentType.ToList();
        }
        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (agent.ID == 0) MainWindow.АгенствоПопрыгун.Agent.Add(agent);
            MainWindow.АгенствоПопрыгун.SaveChanges();

            this.Close();
            MessageBox.Show("Успешно!");
        }

        private void btn_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
