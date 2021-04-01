using Prism.Commands;
using SDT_Project.Models;
using SDT_Project.ServiceServer;
using SDT_Project.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SDT_Project.ViewModels
{
    class UCPWindowVM : BaseVM
    {
        #region Fields
        private Window mainWindow;
        private ConnectionModel model;
        private string username;
        private string email;
        private string id;
        private string transactionId;
        private string transactionDate;
        private string transactionSum;
        public Uri previewImage;
        #endregion

        #region Properties
        public string Username
        {
            set => SetProperty(ref username, value);
            get => username;
        }

        public string Email
        {
            set => SetProperty(ref email, value);
            get => email;
        }

        public string Id
        {
            set => SetProperty(ref id, value);
            get => id;
        }

        public string TransactionId
        {
            set => SetProperty(ref transactionId, value);
            get => transactionId;
        }

        public string TransactionDate
        {
            set => SetProperty(ref transactionDate, value);
            get => transactionDate;
        }

        public string TransactionSum
        {
            set => SetProperty(ref transactionSum, value);
            get => transactionSum;
        }

        public Uri PreviewImage
        {
            set => SetProperty(ref previewImage, value);
            get => previewImage;
        }
        #endregion

        #region Constructors
        public UCPWindowVM() { }

        public UCPWindowVM(Window main, ConnectionModel mod) : this()
        {
            mainWindow = main;

            model = mod;
            model.PropertyChanged += (s, e) => { OnPropertyChanged(e.PropertyName); };

            Username = model.User.Username;
            Email = model.User.Email;
            Id = model.User.Id.ToString();

            if (model?.UserTransaction == null)
                ((MainWindowVM)mainWindow.DataContext).Notification("Невозможно получить данные о последней транзакции.");

            model?.GetLastTransaction();

            TransactionId = GetTransactionId();
            TransactionDate = GetTransactionDate();
            TransactionSum = GetTransactionSum();

            PreviewImage = GetFrontalBrushFromType(model?.Card?.Type ?? 0);
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand<TextBox> Edit => new DelegateCommand<TextBox>((textBox) => 
        {
            textBox.IsEnabled = !textBox.IsEnabled;
            if (textBox.IsEnabled)
            {
                ((MainWindowVM)mainWindow.DataContext).Notification("Редактирование данных");
                textBox.Focus();
            }
            else
            {
                if (MessageBox.Show("Вы желаете сохранить изменения?", "", button: MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (textBox.Name == "TextBoxUsername")
                    {
                        if (model?.ChangeUsername(textBox.Text) ?? false)
                            ((MainWindowVM)mainWindow.DataContext).Notification("Логин успешно изменен!");
                        else
                            ((MainWindowVM)mainWindow.DataContext).Notification("Логин не изменен!");

                        Username = model?.User.Username;
                    }
                    else
                    {
                        if (!model?.IsValidEmail(textBox.Text) ?? false)
                        {
                            ((UCPWindow)((MainWindowVM)mainWindow.DataContext).CurrentView).TextBoxEmail.Text = "";
                            ((MainWindowVM)mainWindow.DataContext).Notification("Почта имеет неверный формат!");
                        }
                        else
                        {
                            if (model?.ChangeEmail(textBox.Text) ?? false)
                                ((MainWindowVM)mainWindow.DataContext).Notification("Почта успешно изменена!");
                            else
                                ((MainWindowVM)mainWindow.DataContext).Notification("Почта не изменена!");
                        }

                        Email = model?.User.Email;
                    }
                }
                else
                {
                    if (textBox.Name == "TextBoxUsername")
                        textBox.Text = model.User.Username;
                    else
                        textBox.Text = model.User.Email;
                }
            }
        });

        private Uri GetFrontalBrushFromType(CardTypes cardType)
        {
            switch (cardType)
            {
                case CardTypes.Base: return new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\wc_front.png");
                case CardTypes.Violet: return new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\wc_vio_front.png");
                case CardTypes.Red: return new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\wc_red_front.png");
                case CardTypes.Full: return new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\bc_front.png");
                default: return new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\img.png");
            }
        }

        private string GetTransactionId()
        {
            return model?.UserTransaction?.Id.ToString() ?? "NONE";
        }

        private string GetTransactionDate()
        {
            return model?.UserTransaction?.Date.ToString("g") ?? "NONE";
        }

        private string GetTransactionSum()
        {
            string result = model?.UserTransaction?.Sum.ToString() ?? "NONE";

            if (result != "NONE")
                result += " р.";

            return result;
        }
        #endregion
    }
}
