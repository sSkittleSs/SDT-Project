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
//using MySql.Data.MySqlClient;

namespace SDT_Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Логика при наведении мыши на кнопку выхода.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseExit_MouseEnter(object sender, MouseEventArgs e)
        {
            this.EllipseExit.Fill = Brushes.DarkRed;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        /// <summary>
        /// Логика при выведении мышки с кнопки выхода.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseExit_MouseLeave(object sender, MouseEventArgs e)
        {
            this.EllipseExit.Fill = Brushes.Red;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Логика при наведении мышки на кнопку максимизирования.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseMaximize_MouseEnter(object sender, MouseEventArgs e)
        {
            this.EllipseMaximize.Fill = Brushes.Gray;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        /// <summary>
        /// Логика при выведении мышки с кнопки максимизирования.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseMaximize_MouseLeave(object sender, MouseEventArgs e)
        {
            this.EllipseMaximize.Fill = Brushes.LightSlateGray;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Логика при наведении мышки на кнопку минизирования.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            this.EllipseMinimize.Fill = Brushes.LightGray;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        /// <summary>
        /// Логика при выведении мышки с кнопки минимизирования.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            this.EllipseMinimize.Fill = Brushes.White;
            Mouse.OverrideCursor = Cursors.Arrow;
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
    }
}
