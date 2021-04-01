using Prism.Mvvm;
using SDT_Project.AdditionalStructures;
using SDT_Project.ServiceServer;
using SDT_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SDT_Project.Models
{
    public class ConnectionModel : BaseVM, IServiceServerCallback
    {
        private ServiceServerClient client;
        private ServerUser user = new ServerUser();
        private SDT_Project.ServiceServer.TravelCard card = new SDT_Project.ServiceServer.TravelCard();
        private SDT_Project.ServiceServer.Transaction userTransaction = new SDT_Project.ServiceServer.Transaction();
        private uint id;

        public bool IsConnected { private set; get; } = false;
        public uint Id
        {
            set
            {
                IsConnected = value == uint.MaxValue ? false : true;
                id = value;
                OnPropertyChanged(nameof(IsConnected));
                OnPropertyChanged(nameof(Id));
            }
            get => id;
        }
        public ServerUser User
        {
            set => SetProperty(ref user, value);
            get => user;
        }
        public SDT_Project.ServiceServer.TravelCard Card
        {
            set => SetProperty(ref card, value);
            get => card;
        }
        public SDT_Project.ServiceServer.Transaction UserTransaction
        {
            set => SetProperty(ref userTransaction, value);
            get => userTransaction;
        }

        public ConnectionModel()
        {
            client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
        }

        public string Connect(string userName, PasswordBox passwordBox)
        {
            if (client?.State == CommunicationState.Created)
            {
                try
                {
                    client?.Open();
                }
                catch (Exception) { return "ServerFaulted"; }
            }

            if (client?.State == CommunicationState.Faulted)
            {
                try
                {
                    client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                }
                catch (Exception) { return "ServerFaulted"; }
            }

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
                try
                {
                    Id = client.Connect(userName, passwordBox.Password);
                    // TODO: Добавить уведомление о некорректном пароле/логине.
                    if (Id == 0)
                    {
                        IsConnected = false;
                        return "incorrect";
                    }

                    //MessageBox.Show($"UserID: {id}", "Notify");
                    //NotifyTextBlock.Text = "Вы были подключены к серверу.";
                    //ButtonTest.IsEnabled = false;

                    User.Id = Id;
                    User.Username = userName;
                    User.Email = client.GetEmail(User.Id);
                    User.UserType = (AdditionalStructures.UserTypes)client.GetUserType(User.Id);
                    Card = client.GetCard(User.Id);
                    UserTransaction = client.GetLastTransaction(User.Id);

                    IsConnected = true;

                }
                catch (Exception) { }
                // TODO: Реализация переключения окна на главное с закрытием текущего.
                // !!! Примечание: При закрытии текущего окна присваивать полю id значение 0 !!!
            }

            return string.Empty;
        }

        public string Register(string email, string username, PasswordBox passwordBox, PasswordBox passwordBoxConfirm)
        {
            if (client?.State == CommunicationState.Created)
            {
                try
                {
                    client?.Open();
                }
                catch (Exception) { return "ServerFaulted"; }
            }

            if (client?.State == CommunicationState.Faulted)
            {
                try
                {
                    client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                }
                catch (Exception) { return "ServerFaulted"; }
            }

            if (username == String.Empty || passwordBox.Password == String.Empty || passwordBoxConfirm.Password == String.Empty || email == String.Empty)
            {
                string notify = string.Empty;
                if (email == String.Empty)
                    notify += "email ";
                if (username == String.Empty)
                    notify += "login ";
                if (passwordBox.Password == String.Empty)
                    notify += "password ";
                if (passwordBoxConfirm.Password == String.Empty || passwordBox.Password != passwordBoxConfirm.Password)
                    notify += "confirm";

                //NotifyTextBlock.Text = $"Вы не ввели {notify}.";
                return notify;
            }

            if (!IsConnected)
            {
                try
                {
                    if (client.Registering(username, passwordBox.Password, email) != 0)
                        return "incorrect";
                    else
                        Connect(username, passwordBox);
                }
                catch (Exception) { }
            }

            return string.Empty;
        }

        public void Disconnect()
        {
            if (client?.State == CommunicationState.Created)
            {
                try
                {
                    client?.Open();
                }
                catch (Exception) { return; }
            }

            if (client?.State == CommunicationState.Faulted)
            {
                try
                {
                    client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                }
                catch (Exception) { return; }
            }

            try
            {
                IsConnected = false;
                User = new ServerUser();
                Card = new SDT_Project.ServiceServer.TravelCard();
                UserTransaction = null;
                client?.Disconnect(Id);
            }
            catch (Exception) { }
        }

        public void DataCallback(string data)
        {
            throw new NotImplementedException();
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool PayForTheTrip()
        {
            if (client?.State == CommunicationState.Created)
            {
                try
                {
                    client?.Open();
                }
                catch (Exception) { return false; }
            }

            if (client?.State == CommunicationState.Faulted)
            {
                try
                {
                    client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                }
                catch (Exception) { return false; }
            }

            if (Card == null)
                return false;

            if (client?.PayForTheTrip((uint)Card.Id, Card.Balance - 1) ?? false)
            {
                Card.Balance -= 1;
                return true;
            }

            return false;
        }

        public bool ChangeUsername(string newUsername)
        {
            if (client?.State == CommunicationState.Created)
            {
                try
                {
                    client?.Open();
                }
                catch (Exception) { return false; }
            }

            if (client?.State == CommunicationState.Faulted)
            {
                try
                {
                    client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                }
                catch (Exception) { return false; }
            }

            if (User == null)
                return false;

            if (client?.ChangeUsername((uint)User.Id, newUsername) ?? false)
            {
                User.Username = newUsername;
                return true;
            }

            return false;
        }

        public bool ChangeEmail(string newEmail)
        {
            if (client?.State == CommunicationState.Created)
            {
                try
                {
                    client?.Open();
                }
                catch (Exception) { return false; }
            }

            if (client?.State == CommunicationState.Faulted)
            {
                try
                {
                    client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                }
                catch (Exception) { return false; }
            }

            if (User == null || User.Email == newEmail)
                return false;

            if (client?.ChangeEmail((uint)User.Id, newEmail) ?? false)
            {
                User.Email = newEmail;
                return true;
            }

            return false;
        }

        public bool ExtendCard()
        {
            if (Card == null)
                return false;

            if (client?.State == CommunicationState.Created)
            {
                try
                {
                    client?.Open();
                }
                catch (Exception) { return false; }
            }

            if (client?.State == CommunicationState.Faulted)
            {
                try
                {
                    client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                }
                catch (Exception) { return false; }
            }

            if (client?.ExtendCard((uint)Card.Id, Card.Balance + 30, Card.LastDate.AddMonths(1)) ?? false)
            {
                Card.Balance += 30;
                Card.LastDate = Card.LastDate.AddMonths(1);
                return true;
            }

            return false;
        }

        public bool BuyCard(double sum, SDT_Project.ServiceServer.CardTypes type, SDT_Project.ServiceServer.CardCategories category, int balance)
        {
            if (User == null)
                return false;

            if (client?.State == CommunicationState.Created)
            {
                try
                {
                    client?.Open();
                }
                catch (Exception) { return false; }
            }

            if (client?.State == CommunicationState.Faulted)
            {
                try
                {
                    client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                }
                catch (Exception) { return false; }
            }

            if (client?.BuyCard(User.Id, sum, type, category, balance) ?? false)
            {
                Card = client.GetCard(User.Id);
                return true;
            }

            return false;
        }

        public bool GetLastTransaction()
        {
            if (User == null)
                return false;

            if (client?.State == CommunicationState.Created)
            {
                try
                {
                    client?.Open();
                }
                catch (Exception) { return false; }
            }

            if (client?.State == CommunicationState.Faulted)
            {
                try
                {
                    client = new ServiceServerClient(new System.ServiceModel.InstanceContext(this));
                }
                catch (Exception) { return false; }
            }
            try
            {
                UserTransaction = client?.GetLastTransaction(User.Id);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
