using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using ServerUIWpf.Model;


namespace ServerUIWpf.ViewModels
{
    public class AppViewModel : Screen
    {


        #region ctor

        public AppViewModel()
        {
            TicketItems.Add(new TicketItem {CashierName = "Касса 6", TicketName = "билет № П001"});
        }

        #endregion




        #region prop

        public BindableCollection<TicketItem> TicketItems { get; set; } = new BindableCollection<TicketItem>();


        private SolidColorBrush _colorBackground = Brushes.SlateGray;
        public SolidColorBrush ColorBackground
        {
            get { return _colorBackground; }
            set
            {
                _colorBackground = value;
                NotifyOfPropertyChange(() => ColorBackground);
            }
        }

        #endregion




        #region EventHandler


        #endregion




        #region Methode

        public  void Add()
        {
            TicketItems.Add(new TicketItem { CashierName = "Касса 6", TicketName = "билет № П001" });
        }



        public  void Remove()
        {
            if(TicketItems.Any())
               TicketItems.Remove(TicketItems.FirstOrDefault());
        }

        #endregion
    }
}