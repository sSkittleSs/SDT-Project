using Prism.Commands;
using SDT_Project.Models;
using SDT_Project.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SDT_Project.ViewModels
{
    class MainWindowVM : BaseVM
    {
        #region Fields
        private Window mainWindow;
        private string notify;
        private bool isConnected = false;
        private string selectedItem = "1";
        private UserControl currentView;

        public readonly ConnectionModel Model = new ConnectionModel();
        #endregion

        #region Propeties
        public string Notify
        {
            set => SetProperty(ref notify, value);
            get => notify;
        }
        public bool IsConnected
        {
            set => SetProperty(ref isConnected, value);
            get => isConnected;
        }
        public string SelectedItem
        {
            set => SetProperty(ref selectedItem, value);
            get => selectedItem;
        }
        public UserControl CurrentView
        {
            set => SetProperty(ref currentView, value);
            get => currentView;
        }
        #endregion

        #region Constructors
        public MainWindowVM() => CurrentView = new LoginWindow();
        public MainWindowVM(Window main)
        {
            mainWindow = main;
            CurrentView = new LoginWindow(mainWindow, Model);
        }
        #endregion

        #region Methods
        #endregion

        #region Commands
        public DelegateCommand OpenBuyCard => new DelegateCommand(() =>
        {
            SelectedItem = "1";
            CurrentView = new BuyCardWindow(mainWindow, Model);
        });

        public DelegateCommand OpenMyCard => new DelegateCommand(() =>
        {
            SelectedItem = "2";
            CurrentView = new MyCardWindow(mainWindow, Model);
        });

        public DelegateCommand OpenUCP => new DelegateCommand(() =>
        {
            SelectedItem = "3";
            CurrentView = new UCPWindow(mainWindow, Model);
        });

        public DelegateCommand OpenAuthorize => new DelegateCommand(() =>
        {
            SelectedItem = "1";
            CurrentView = new LoginWindow(mainWindow, Model);
        });

        public DelegateCommand OpenRegister => new DelegateCommand(() =>
        {
            SelectedItem = "2";
            CurrentView = new RegWindow(mainWindow, Model);
        });

        public DelegateCommand Disconnect => new DelegateCommand(() =>
        {
            Model.Disconnect();
            IsConnected = false;
            OpenAuthorize.Execute();
        });
    

        public void Authorize(string notify = "Вы были успешно подключены!")
        {
            OpenUCP.Execute();
            IsConnected = true;
            Notify = notify;
        }

        public void Notification(string notify = "Место для уведомлений") => Notify = notify;
        #endregion

    }
}
