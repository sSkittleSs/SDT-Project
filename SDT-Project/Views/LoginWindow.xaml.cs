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
using SDT_Project.Models;
using SDT_Project.ServiceServer;
using SDT_Project.ViewModels;

namespace SDT_Project
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : UserControl, ServiceServer.IServiceServerCallback
    {
        Window mainWindow;
        public LoginWindow()
        {
            InitializeComponent();
        }

        public LoginWindow(Window main, ConnectionModel mod)
        {
            InitializeComponent();

            mainWindow = main;
            DataContext = new LoginWindowVM(mainWindow, mod);
        }

        public void DataCallback(string data)
        {
            throw new NotImplementedException();
        }

        private void LoginWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((LoginWindowVM)DataContext).Disconnect.Execute();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Length > 0)
                PasswordWatermark.Visibility = Visibility.Collapsed;
            else
                PasswordWatermark.Visibility = Visibility.Visible;
        }

        private void ResetPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //TODO: Открываем новое окно для восстановления пароля с овнером (тек. окно)
        }

        private void Register_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((MainWindowVM)mainWindow.DataContext).OpenRegister.Execute();
        }
    }
}
