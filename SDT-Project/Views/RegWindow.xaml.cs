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
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : UserControl, ServiceServer.IServiceServerCallback
    {
        Window mainWindow;

        public RegWindow()
        {
            InitializeComponent();
        }

        public RegWindow(Window own, ConnectionModel model)
        {
            InitializeComponent();

            mainWindow = own;

            DataContext = new RegWindowVM(mainWindow, model);
        }

        //private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        //{
        //    Cursor = Cursors.AppStarting;
        //    if (LoginTextBox.Text == String.Empty || PasswordBox.Password == String.Empty || RePasswordBox.Password == String.Empty)
        //    {
        //        StringBuilder notify = new StringBuilder();
        //        if (LoginTextBox.Text == String.Empty)
        //            notify.Append("логин");
        //        if (PasswordBox.Password == String.Empty)
        //            if (notify.Length > 0)
        //                notify.Append(", пароль");
        //            else notify.Append("пароль");

        //        if (RePasswordBox.Password == String.Empty)
        //            if (notify.Length > 0)
        //                notify.Append(" и подтверждение пароля");
        //            else notify.Append("подтверждение пароля");

        //        //NotifyTextBlock.Text = $"Вы не ввели {notify}.";
        //        Cursor = Cursors.Arrow;
        //        return;
        //    }

        //    if (!isConnected)
        //    {
        //        client = client ?? new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
        //        id = client.Registering(LoginTextBox.Text, PasswordBox.Password);
        //        // TODO: Добавить уведомление о некорректном пароле/логине.
        //        if (id == 0)
        //        {
        //            client = null;
        //            //NotifyTextBlock.Text = "Регистрация не произошла.";
        //            Cursor = Cursors.Arrow;
        //            return;
        //        }
        //        //MessageBox.Show($"UserID: {id}", "Notify");
        //        //NotifyTextBlock.Text = "Вы были зарегистрированы.";
        //        ButtonRegister.IsEnabled = false;
        //        isConnected = true;


        //        //Close();
        //    }
        //    Cursor = Cursors.Arrow;
        //}

        
        public void DataCallback(string data)
        {
            throw new NotImplementedException();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Length > 0)
                PasswordWatermark.Visibility = Visibility.Collapsed;
            else
                PasswordWatermark.Visibility = Visibility.Visible;
        }

        private void OnRePasswordChanged(object sender, RoutedEventArgs e)
        {
            if (RePasswordBox.Password.Length > 0)
                RePasswordWatermark.Visibility = Visibility.Collapsed;
            else
                RePasswordWatermark.Visibility = Visibility.Visible;
        }
    }
}
