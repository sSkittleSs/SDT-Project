using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using MySql.Data.MySqlClient;
using SDT_Project.ServiceServer;

namespace SDT_Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ServiceServer.IServiceServerCallback
    {
        bool isConnected = false;
        ServiceServerClient client;
        int id;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Тестовая кнопка с логикой подключения к серверу.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {

            if (isConnected)
            {
                client.Disconnect(id);
                MessageBox.Show($"Connection closed.", "Notify");
                ButtonTest.Content = "Connect";
                isConnected = false;
                client = null;
            }
            else
            {
                client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                id = client.Connect("Skittles", "admin123");
                MessageBox.Show($"UserID: {id}", "Notify");
                ButtonTest.Content = "Disconnect";
                isConnected = true;
            }
        }

        /// <summary>
        /// Логика при нажатии на кнопку выхода.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseExit_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Логика при нажатии на кнопку максимизирования.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseMaximize_Click(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        /// <summary>
        /// Логика при нажатии на кнопку минимизирования.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseMinimize_Click(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Логика при удержании левой кнопки мыши в области верхней панели.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridUpper_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (this.WindowState == WindowState.Maximized)
            //    this.WindowState = WindowState.Normal;

            this.DragMove();
        }

        public void DataCallback(string data)
        {
            ((IServiceServerCallback)WindowMain).DataCallback(data);
        }
    }
}
