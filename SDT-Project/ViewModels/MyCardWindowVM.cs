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
    class MyCardWindowVM : BaseVM
    {
        #region Fields
        private Window mainWindow;
        private ConnectionModel model;
        private string buttonText;
        private string endTime;
        private string cardType;
        private string tripsAmount;
        private Brush frontBrush;
        private Brush backBrush;
        #endregion

        #region Properties
        public string ButtonText
        {
            set => SetProperty(ref buttonText, value);
            get => buttonText;
        }

        public string EndTime
        {
            set => SetProperty(ref endTime, value);
            get => endTime;
        }

        public string CardType
        {
            set => SetProperty(ref cardType, value);
            get => cardType;
        }

        public string TripsAmount
        {
            set => SetProperty(ref tripsAmount, value);
            get => tripsAmount;
        }

        public Brush FrontBrush
        {
            set => SetProperty(ref frontBrush, value);
            get => frontBrush;
        }

        public Brush BackBrush
        {
            set => SetProperty(ref backBrush, value);
            get => backBrush;
        }
        #endregion

        #region Constructors
        public MyCardWindowVM() { }

        public MyCardWindowVM(Window main, ConnectionModel mod) : this()
        {
            mainWindow = main;

            model = mod;
            model.PropertyChanged += (s, e) => { OnPropertyChanged(e.PropertyName); };

            EndTime = GetEndTime();
            CardType = GetCardCategory();
            TripsAmount = GetTripsAmount();
            FrontBrush = GetFrontalBrush();
            BackBrush = GetBackBrush();

            if (model?.Card == null)
            {
                ButtonText = "Приобрести";
                ((MainWindowVM)mainWindow.DataContext).Notification("Невозможно получить данные о проездном билете.");
            }
            else
            {
                ButtonText = "Продлить";
            }
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Extend => new DelegateCommand(() => 
        {
            try
            {
                if (model?.Card == null)
                    ((MainWindowVM)mainWindow.DataContext).OpenBuyCard.Execute();

                if (model?.ExtendCard() ?? false)
                {
                    ((MainWindowVM)mainWindow.DataContext).Notification("Проездной билет продлен на 1 месяц на 30 поездок.");
                    TripsAmount = GetTripsAmount();
                    EndTime = GetEndTime();
                }
                else
                    ((MainWindowVM)mainWindow.DataContext).Notification("Проездной билет не продлен.");

            }
            catch (Exception) { ((MainWindowVM)mainWindow.DataContext).Notification("Проездной билет не продлен."); }
        }, () => model?.Card?.Type != CardTypes.Full);
        public DelegateCommand Pay => new DelegateCommand(() => 
        { 
            if ((model?.Card?.LastDate >= DateTime.Today && model?.Card?.FirstDate <= DateTime.Today) || model?.Card?.Type == CardTypes.Full)
            {
                if (TripsAmount == "∞")
                {
                    ((MainWindowVM)mainWindow.DataContext).Notification("Поездка успешно оплачена!");
                    return;
                }
            
                if (TripsAmount == "0")
                {
                    ((MainWindowVM)mainWindow.DataContext).Notification("Недостаточно поездок на балансе! Поездка не оплачена.");
                    return;
                }

                if (model?.PayForTheTrip() ?? false)
                {
                    TripsAmount = GetTripsAmount();
                    ((MainWindowVM)mainWindow.DataContext).Notification("Поездка успешно оплачена.");
                }
                else
                {
                    ((MainWindowVM)mainWindow.DataContext).Notification("Поездка не оплачена!");
                }

                return;
            }
            else
            {
                ((MainWindowVM)mainWindow.DataContext).Notification("Проездной в данный момент не активен! Поездка не оплачена.");
                return;
            }
        });

        private string GetEndTime()
        {
            if (model?.Card?.LastDate == new DateTime() || model?.Card?.Type == CardTypes.Full)
                return "NONE";

            string result = model?.Card?.LastDate.ToString("d") ?? "NONE";
            // Запрос в БД
            return result;
        }

        private Brush GetFrontalBrush()
        {
            Brush result = GetFrontalBrushFromType(model?.Card?.Type ?? 0);
            // Запрос в БД
            return result;
        }

        private Brush GetFrontalBrushFromType(CardTypes cardType)
        {
            switch (cardType)
            {
                case CardTypes.Base: return new ImageBrush(new BitmapImage(new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\wc_front.png")));
                case CardTypes.Violet: return new ImageBrush(new BitmapImage(new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\wc_vio_front.png")));
                case CardTypes.Red: return new ImageBrush(new BitmapImage(new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\wc_red_front.png")));
                case CardTypes.Full: return new ImageBrush(new BitmapImage(new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\bc_front.png")));
                default: return new ImageBrush(new BitmapImage(new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\img.png")));
            }
        }

        private Brush GetBackBrush()
        {
            Brush result = GetBackBrushFromType(model?.Card?.Type ?? 0);
            // Запрос в БД
            return result;
        }

        private Brush GetBackBrushFromType(CardTypes cardType)
        {
            switch (cardType)
            {
                case CardTypes.Base:
                case CardTypes.Violet:
                case CardTypes.Red: return DrawCardNumber(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\wc_back.png", model?.Card?.Id.ToString(@"D11"));
                case CardTypes.Full: return DrawCardNumber(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\bc_back.png", model?.Card?.Id.ToString(@"D11"), new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(198, 153, 36)));
                default: return new ImageBrush(new BitmapImage(new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\img2.png")));
            }
        }

        private string GetCardCategory()
        {
            if (model?.Card?.Type == CardTypes.Full)
                return "А; Т; М; Трам;";

            string result = GetCardCategoryFromCard(model?.Card?.Category ?? 0);
            // Запрос в БД
            return result;
        }

        private string GetCardCategoryFromCard(CardCategories cardCategory)
        {
            switch (cardCategory)
            {
                case SDT_Project.ServiceServer.CardCategories.A: return "А;";
                case SDT_Project.ServiceServer.CardCategories.T: return "Т;";
                case SDT_Project.ServiceServer.CardCategories.M: return "М;";
                case SDT_Project.ServiceServer.CardCategories.Tram: return "Трам;";
                case SDT_Project.ServiceServer.CardCategories.AT: return "А; Т;";
                case SDT_Project.ServiceServer.CardCategories.ATM: return "А; Т; М;";
                case SDT_Project.ServiceServer.CardCategories.ATTram: return "А; Т; Трам;";
                case SDT_Project.ServiceServer.CardCategories.ATMTram: return "А; Т; М; Трам;";
                default: return "NONE";
            }
        }

        private string GetTripsAmount()
        {
            if (model?.Card?.Type == CardTypes.Full)
                return "∞";

            string result = (model?.Card?.Balance ?? 0).ToString();
            // Запрос в БД
            return result;
        }

        private Brush DrawCardNumber(string pathToCard, string cardId, System.Drawing.SolidBrush color = null)
        {
            return new ImageBrush(SDT_Project.AdditionalStructures.BitmapConverter.ToBitmapImage(CombineBitmap(new System.Drawing.Bitmap(pathToCard), cardId, color)));
        }


        private System.Drawing.Bitmap CombineBitmap(System.Drawing.Bitmap image, string cardId, System.Drawing.SolidBrush color = null)
        {
            if (color == null)
                color = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

            System.Drawing.Bitmap finalImage = null;

            try
            {
                finalImage = new System.Drawing.Bitmap(image.Width, image.Height);

                using (var g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    g.Clear(System.Drawing.Color.Transparent);
                    g.DrawImage(image, new System.Drawing.Rectangle(0, 0, image.Width, image.Height));
                    g.DrawString(cardId, new System.Drawing.Font("Arial", 42, System.Drawing.FontStyle.Bold), color, new System.Drawing.Rectangle(96, 347, 586, 71));
                }
            }
            catch (Exception)
            {
                if (finalImage != null)
                    finalImage.Dispose();
            }

            return finalImage;
        }
        #endregion
    }
}
