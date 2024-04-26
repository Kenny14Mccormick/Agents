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
            if (agent.ID == 0) btnDelete.IsEnabled = false;
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

        private void btnLogo_Click(object sender, RoutedEventArgs e)
        {
            // Создаем диалог выбора файлов
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Устанавливаем фильтр для типов файлов, которые могут быть открыты
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";

            // Открываем диалог и получаем результат
            Nullable<bool> result = dlg.ShowDialog();

            // Если файл выбран, отображаем путь к файлу в текстовом поле
            if (result == true)
            {
                // Получаем выбранный файл
                string filename = dlg.FileName;

                // Отображаем путь к файлу в текстовом поле
                tbLogo.Text = filename;

                // Сохраняем путь к файлу в свойстве Logo агента
                agent.Logo = filename;
            }
        }

        private void btnDeleteAgent(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли у агента информация о реализации продукции
            if (MainWindow.АгенствоПопрыгун.ProductSale.Any(ps => ps.AgentID == agent.ID))
            {
                MessageBox.Show("Удаление агента запрещено, так как у агента есть информация о реализации продукции!");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Вы точно хотите удалить агента?",
                "Подтверждение об удалении",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Удаляем все записи об изменении приоритета для этого агента
                var historyRecords = MainWindow.АгенствоПопрыгун.AgentPriorityHistory.Where(aph => aph.AgentID == agent.ID);
                MainWindow.АгенствоПопрыгун.AgentPriorityHistory.RemoveRange(historyRecords);

                // Удаляем агента
                MainWindow.АгенствоПопрыгун.Agent.Remove(agent);
                MainWindow.АгенствоПопрыгун.SaveChanges();
                MessageBox.Show("Запись удалена!");
                Close();
            }
        }

    }
}
