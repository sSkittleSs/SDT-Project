using Prism.Commands;
using SDT_Project.Models;
using SDT_Project.ServiceServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SDT_Project.ViewModels
{
    class BuyCardWindowVM : BaseVM
    {
        #region Fields
        private Window mainWindow;
        private ConnectionModel model;
        private int cardCategory;
        private string cardType;
        private string cardBalance;
        private int cardTypeInt;
        private string price;
        private bool isAdmin;
        public Uri previewImage;
        #endregion

        #region Properties
        public int CardCategory
        {
            set
            {
                SetProperty(ref cardCategory, value);
                CardType = value == 8 ? "Top-level" : GetCardTypeFromCategory((CardCategories)(value + 1));
            }
            get => cardCategory;
        }

        public string CardType
        {
            private set
            {
                SetProperty(ref cardType, value);
                cardTypeInt = GetCardTypeFromString(value);
                PreviewImage = GetFrontalBrushFromType((CardTypes)cardTypeInt);
                Price = CalculatePrice();
            }
            get => cardType;
        }

        public string CardBalance
        {
            set
            {
                if (!int.TryParse(value, out int x))
                    return;

                SetProperty(ref cardBalance, value);
                Price = CalculatePrice();
            }
            get => cardBalance;
        }

        public string Price
        {
            set => SetProperty(ref price, value);
            get => price;
        }

        public bool IsAdmin
        {
            set => SetProperty(ref isAdmin, value);
            get => isAdmin;
        }

        public Uri PreviewImage
        {
            private set => SetProperty(ref previewImage, value);
            get => previewImage;
        }
        #endregion

        #region Constructors
        public BuyCardWindowVM() { }

        public BuyCardWindowVM(Window main, ConnectionModel mod) : this()
        {
            mainWindow = main;

            model = mod;
            model.PropertyChanged += (s, e) => { OnPropertyChanged(e.PropertyName); };

            IsAdmin = model?.User?.UserType == SDT_Project.AdditionalStructures.UserTypes.SysAdmin;

            CardCategory = 0;
            CardBalance = "0";

            PreviewImage = GetFrontalBrushFromType((CardTypes)cardTypeInt);
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Order => new DelegateCommand(() =>
        {
            try
            {
                if (model?.BuyCard(Convert.ToDouble(Price), (CardTypes)cardTypeInt, (CardCategories)(CardCategory + (CardCategory == 8 ? 0 : 1)), Convert.ToInt32(CardBalance)) ?? false)
                {
                    ((MainWindowVM)mainWindow.DataContext).OpenMyCard.Execute();
                    ((MainWindowVM)mainWindow.DataContext).Notification("Проездной билет заказан.");
                }
                else
                    ((MainWindowVM)mainWindow.DataContext).Notification("Проездной билет не заказан.");
            }
            catch (Exception) { ((MainWindowVM)mainWindow.DataContext).Notification("Произошла непредвиденная ошибка. Проездной билет не заказан."); }
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

        private int GetCardTypeFromString(string type)
        {
            switch(type)
            {
                case "Base-level": return 1;
                case "Middle-level": return 2;
                case "High-level": return 3;
                case "Top-level": return 4;
                default: return 0;
            }
        }

        private string GetCardTypeFromCategory(CardCategories category)
        {
            switch (category)
            {
                case CardCategories.A:
                case CardCategories.T:
                case CardCategories.M:
                case CardCategories.Tram: return "Base-level";
                case CardCategories.AT:
                case CardCategories.ATM:
                case CardCategories.ATTram: return "Middle-level";
                case CardCategories.ATMTram: return "High-level";
                default: return "NONE";
            }
        }

        private double GetPriceForCardCategory(CardCategories category)
        {
            switch (category)
            {
                case CardCategories.A:
                case CardCategories.T: return 2.99;
                case CardCategories.M: return 4.12;
                case CardCategories.Tram: return 12;
                case CardCategories.AT: return 3.87;
                case CardCategories.ATM: return 7.99;
                case CardCategories.ATTram: return 5.67;
                case CardCategories.ATMTram: return 12.22;
                default: return 0;
            }
        }

        private double GetPriceForCardType(CardTypes category)
        {
            switch (category)
            {
                case CardTypes.Base: return 1.01;
                case CardTypes.Violet: return 3.51;
                case CardTypes.Red: return 8.86;
                case CardTypes.Full: return 17.99;
                default: return 0;
            }
        }

        private string CalculatePrice()
        {
            double result = IsAdmin ? 0 : GetPriceForCardCategory((CardCategories)(CardCategory + 1)) + GetPriceForCardType((CardTypes)cardTypeInt) + Convert.ToInt32(CardBalance) * 0.7;

            return result.ToString();
        }
        #endregion
    }
}
