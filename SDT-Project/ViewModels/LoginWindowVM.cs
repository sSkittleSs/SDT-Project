using Prism.Commands;
using Prism.Mvvm;
using SDT_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SDT_Project.ViewModels
{
    class LoginWindowVM : BindableBase
    {
        #region Connection
        private string _notify = "Место для уведомлений"; 
        readonly ConnectionModel _model = new ConnectionModel();

        public string Notify
        {
            set
            {
                _notify = value;
                RaisePropertyChanged(nameof(Notify));
            }
            get => (string)_notify.Clone();
        }

        public LoginWindowVM()
        {
            _model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            Connect = new DelegateCommand<Tuple<string, PasswordBox>>((tuple) =>
            {
                Notify = _model?.Connect(tuple.Item1, tuple.Item2);
            }, (tuple) => !_model.IsConnected);
        }

        public DelegateCommand<Tuple<string, PasswordBox>> Connect { get; }
        #endregion
    }
}
