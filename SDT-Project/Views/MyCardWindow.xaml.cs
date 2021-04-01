using SDT_Project.Models;
using SDT_Project.ViewModels;
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

namespace SDT_Project.Views
{
    /// <summary>
    /// Логика взаимодействия для OpenMyCard.xaml
    /// </summary>
    public partial class MyCardWindow : UserControl
    {
        public MyCardWindow(Window mainWindow, ConnectionModel model)
        {
            Cursor = Cursors.AppStarting;
            InitializeComponent();

            DataContext = new MyCardWindowVM(mainWindow, model);
            Cursor = Cursors.Arrow;
        }

        public void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cardModel.OnMouseLeftButtonDown(cardModel, e);
        }

        public void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            cardModel.OnMouseUp(cardModel, e);
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            cardModel.OnMouseMove(cardModel, e);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            cardModel.OnMouseWheel(cardModel, e);
        }
    }
}
