using Prism.Commands;
using Prism.Mvvm;
using SDT_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SDT_Project.ViewModels
{
    class LoginWindowVM : BaseVM
    {
        #region Connection
        private Window mainWindow;
        private ConnectionModel model;

        public LoginWindowVM()
        {
            Connect = new DelegateCommand<Tuple<string, PasswordBox>>((tuple) =>
            {
                if (tuple.Item1 == string.Empty)
                {
                    ((MainWindowVM)mainWindow.DataContext).Notification("Введите логин!");
                    return;
                }

                if (tuple.Item2.Password == string.Empty)
                {
                    ((MainWindowVM)mainWindow.DataContext).Notification("Введите пароль!");
                    return;
                }

                string resultOfOperation = model?.Connect(tuple.Item1, tuple.Item2);
                if (model?.IsConnected ?? false)
                {
                    ((MainWindowVM)mainWindow.DataContext).Authorize();
                }
                else
                {
                    if (resultOfOperation != "ServerFaulted")
                        ((MainWindowVM)mainWindow.DataContext).Notification("Авторизация не завершена. Возможно неверный логин или пароль.");
                    else
                        ((MainWindowVM)mainWindow.DataContext).Notification("Сервер в данный момент не доступен. Попробуйте позднее.");

                }    
            }, (tuple) => !model.IsConnected);

            Disconnect = new DelegateCommand(() =>
            {
                model?.Disconnect();
                ((MainWindowVM)mainWindow.DataContext).IsConnected = model?.IsConnected ?? false;
            }, () => model.IsConnected);
        }

        public LoginWindowVM(Window main, ConnectionModel mod) : this()
        {
            mainWindow = main;

            model = mod;
            model.PropertyChanged += (s, e) => { OnPropertyChanged(e.PropertyName); };
        }

        public DelegateCommand<Tuple<string, PasswordBox>> Connect { get; }
        public DelegateCommand Disconnect { get; }
        #endregion
    }
}
