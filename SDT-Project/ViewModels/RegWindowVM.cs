using Prism.Commands;
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
    class RegWindowVM : BaseVM
    {
        #region Connection
        private Window mainWindow;
        private ConnectionModel model;

        public RegWindowVM() { }

        public RegWindowVM(Window main, ConnectionModel mod) : this()
        {
            mainWindow = main;

            model = mod;
            model.PropertyChanged += (s, e) => { OnPropertyChanged(e.PropertyName); };

            Register = new DelegateCommand(() =>
            {
                string email = ((RegWindow)((MainWindowVM)mainWindow.DataContext).CurrentView).MailTextBox.Text;
                if (!model?.IsValidEmail(email) ?? false)
                {
                    ((RegWindow)((MainWindowVM)mainWindow.DataContext).CurrentView).MailTextBox.Text = "";
                    ((MainWindowVM)mainWindow.DataContext).Notification("Почта имеет неверный формат!");
                    return;
                }
                string login = ((RegWindow)((MainWindowVM)mainWindow.DataContext).CurrentView).LoginTextBox.Text;

                if (login == string.Empty)
                {
                    ((MainWindowVM)mainWindow.DataContext).Notification("Не введен логин");
                    return;
                }    

                string resultOfOperation = string.Empty;

                if (((RegWindow)((MainWindowVM)mainWindow.DataContext).CurrentView).PasswordBox.Password == string.Empty ||  ((RegWindow)((MainWindowVM)mainWindow.DataContext).CurrentView).RePasswordBox.Password == string.Empty)
                {
                    ((MainWindowVM)mainWindow.DataContext).Notification("Не введен пароль");
                    return;
                }

                if (((RegWindow)((MainWindowVM)mainWindow.DataContext).CurrentView).PasswordBox.Password == ((RegWindow)((MainWindowVM)mainWindow.DataContext).CurrentView).RePasswordBox.Password)
                {
                    resultOfOperation = model?.Register(email, login, ((RegWindow)((MainWindowVM)mainWindow.DataContext).CurrentView).PasswordBox, ((RegWindow)((MainWindowVM)mainWindow.DataContext).CurrentView).RePasswordBox);
                }
                else
                {
                    ((MainWindowVM)mainWindow.DataContext).Notification("Пароли не совпадают!");
                    return;
                }

                if (resultOfOperation == "ServerFaulted")
                {
                    ((MainWindowVM)mainWindow.DataContext).Notification("Сервер в данный момент не доступен. Попробуйте позднее.");
                    return;
                }

                if (model?.IsConnected ?? false)
                    ((MainWindowVM)mainWindow.DataContext).Authorize("Вы были успешно зарегистрированы и авторизованы");
                else
                    ((MainWindowVM)mainWindow.DataContext).Notification("Повторите попытку регистрации (проверьте пароль или почту)");
            }, () => !model.IsConnected);
        }

        public DelegateCommand Register { get; }
        #endregion
    }
}
