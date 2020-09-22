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
using SDT_Project.ServiceServer;


namespace SDT_Project
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, ServiceServer.IServiceServerCallback
    {
        bool isConnected = false;
        ServiceServerClient client;
        uint id;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {

            if (!isConnected)
            {
                client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                id = client.Connect("Skittles", "admin12");
                if (id == 0)
                {
                    // TODO: Добавить реализацию уведомления о неподключении.
                    NotifyTextBlock.Text = "Вы не были подключены к серверу.";
                    return;
                }
                //MessageBox.Show($"UserID: {id}", "Notify");
                NotifyTextBlock.Text = "Вы были подключены к серверу.";
                ButtonTest.IsEnabled = false;
                isConnected = true;

                // TODO: Реализация переключения окна на главное с закрытием текущего.
                // !!! Примечание: При закрытии текущего окна присваивать полю id значение 0 !!!
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

            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();

        }

        public void DataCallback(string data)
        {
            throw new NotImplementedException();
        }

        private void LoginWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            client?.Disconnect(id);
        }
    }
}
