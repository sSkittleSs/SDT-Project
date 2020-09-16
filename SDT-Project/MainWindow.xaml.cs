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

        private void EllipseExit_MouseEnter(object sender, MouseEventArgs e)
        {
            EllipseExit.Fill = Brushes.DarkRed;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void EllipseExit_MouseLeave(object sender, MouseEventArgs e)
        {
            EllipseExit.Fill = Brushes.Red;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void EllipseMaximize_MouseEnter(object sender, MouseEventArgs e)
        {
            EllipseMaximize.Fill = Brushes.Gray;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void EllipseMaximize_MouseLeave(object sender, MouseEventArgs e)
        {
            EllipseMaximize.Fill = Brushes.LightSlateGray;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void EllipseMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            EllipseMinimize.Fill = Brushes.LightGray;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void EllipseMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            EllipseMinimize.Fill = Brushes.White;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void EllipseExit_Click(object sender, MouseEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void EllipseMaximize_Click(object sender, MouseEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
        private void EllipseMinimize_Click(object sender, MouseEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void FreeNotify_Click(object sender, MouseEventArgs e) // TODO: Удалить после реализации изменения размера окна.
        {
            WindowState = WindowState.Normal;
        }
    }
}
