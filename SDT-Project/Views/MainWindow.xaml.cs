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
using SDT_Project.ViewModels;

namespace SDT_Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ServiceServer.IServiceServerCallback
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowVM(this);
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

            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();

        }

        public void DataCallback(string data)
        {
            ((IServiceServerCallback)WindowMain).DataCallback(data);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindowVM)DataContext).Disconnect.Execute();
        }
    }
}
