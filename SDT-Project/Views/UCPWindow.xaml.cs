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
    /// Логика взаимодействия для UCPWindow.xaml
    /// </summary>
    public partial class UCPWindow : UserControl
    {
        public UCPWindow(Window mainWindow, ConnectionModel model)
        {
            Cursor = Cursors.AppStarting;
            InitializeComponent();

            DataContext = new UCPWindowVM(mainWindow, model);
            Cursor = Cursors.Arrow;
        }
    }
}
