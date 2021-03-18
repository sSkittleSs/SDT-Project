using Prism.Mvvm;
using SDT_Project.ServiceServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SDT_Project.Models
{
    class ConnectionModel : BindableBase, IServiceServerCallback
    {
        private readonly ServiceServerClient client;
        private uint id;

        public bool IsConnected { private set; get; } = false;
        public uint Id
        {
            set
            {
                IsConnected = value == uint.MaxValue ? false : true;
                id = value;
                RaisePropertyChanged(nameof(IsConnected));
                RaisePropertyChanged(nameof(Id));
            }
            get => id;
        }

        public ConnectionModel()
        {
            client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
        }

        public string Connect(string userName, PasswordBox passwordBox)
        {
            if (userName == String.Empty || passwordBox.Password == String.Empty)
            {
                string notify = string.Empty;
                if (userName == String.Empty)
                    notify += "login";
                if (passwordBox.Password == String.Empty)
                    notify += "password";

                //NotifyTextBlock.Text = $"Вы не ввели {notify}.";
                return notify;
            }

            if (!IsConnected)
            {
                Id = client.Connect(userName, passwordBox.Password);
                // TODO: Добавить уведомление о некорректном пароле/логине.
                if (!IsConnected)
                    return "incorrect";

                //MessageBox.Show($"UserID: {id}", "Notify");
                //NotifyTextBlock.Text = "Вы были подключены к серверу.";
                //ButtonTest.IsEnabled = false;

                IsConnected = true;

                // TODO: Реализация переключения окна на главное с закрытием текущего.
                // !!! Примечание: При закрытии текущего окна присваивать полю id значение 0 !!!
            }

            return string.Empty;
        }

        public void DataCallback(string data)
        {
            throw new NotImplementedException();
        }
    }
}
