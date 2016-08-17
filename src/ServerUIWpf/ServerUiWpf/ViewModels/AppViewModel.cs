using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using Communication.TcpIp;
using TempServer.Entitys;
using TempServer.Model;
using TicketItem = ServerUi.Model.TicketItem;

namespace ServerUi.ViewModels
{
    public class AppViewModel : Screen
    {
        #region field

        private readonly ServerModel _model;
        private readonly Task _mainTask;

        #endregion



        #region ctor

        public AppViewModel()
        {

            _model = new ServerModel();
            _model.PropertyChanged += _model_PropertyChanged;

            _model.LoadSetting();
            foreach (var cashier in _model.Сashiers)
            {
                cashier.PropertyChanged += Cashier_PropertyChanged;
            }

            if (_model.Listener != null)
            {
                _model.Listener.PropertyChanged += Listener_PropertyChanged;
                _mainTask = _model.Start();
            }
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

        private async void Cashier_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var c = sender as Сashier;
            if (c != null)
            {
                if (e.PropertyName == "CurrentTicket")
                {
                    if (c.CurrentTicket != null)      //добавить элемент к списку
                    {

                        TicketItems.Add(new TicketItem { CashierName = "Касса " + c.CurrentTicket.Сashbox,
                                                         TicketName =  $"Талон {c.CurrentTicket.Prefix}{c.CurrentTicket.NumberElement.ToString("000")}" });
                        //_view.AddRow($"Талон {c.CurrentTicket.Prefix}{c.CurrentTicket.NumberElement.ToString("000")}", "Касса " + c.CurrentTicket.Сashbox);
                        var task = _model.LogTicket?.Add(c.CurrentTicket.ToString());
                        if (task != null) await task;
                    }
                    else                             //удалить элемент из списка
                    {
                        var removeItem = TicketItems.FirstOrDefault((elem) => elem.CashierName.Contains(c.Id.ToString()));
                        TicketItems.Remove(removeItem);
                        // _view.RemoveRow(c.Id.ToString());
                    }
                }
            }
        }


        private void Listener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var listener = sender as ListenerTcpIp;
            if (listener != null)
            {
                if (e.PropertyName == "IsConnect")
                {
                    ColorBackground = listener.IsConnect ? Brushes.SlateGray : Brushes.Magenta;
                    // _view.BackgroundColorDataGrid = listener.IsConnect ? Color.DimGray : Color.Brown;
                }
            }
        }


        private void _model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var server = sender as ServerModel;
            if (server != null)
            {
                if (e.PropertyName == "ErrorString")
                {
                    //_view.ErrorString = server.ErrorString;
                    MessageBox.Show(server.ErrorString); //TODO: как вызвать MessageBox
                }
            }
        }

        #endregion




        #region Methode

        protected override void OnDeactivate(bool close)
        {
            _model.Dispose();
            base.OnDeactivate(close);
        }

        #endregion








        #region DEBUG

        public void V1_Add()
        {
            _model.Сashiers[0].StartHandling();
            _model.Сashiers[0].SuccessfulStartHandling();
        }

        public void V1_Sucsess()
        {
            _model.Сashiers[0].SuccessfulHandling();
        }

        public void V1_Error()
        {
            _model.Сashiers[0].ErrorHandling();
        }


        public void V2_Add()
        {
            _model.Сashiers[1].StartHandling();
            _model.Сashiers[1].SuccessfulStartHandling();
        }

        public void V2_Sucsess()
        {
            _model.Сashiers[1].SuccessfulHandling();
        }

        public void V2_Error()
        {
            _model.Сashiers[1].ErrorHandling();
        }


        public void V3_Add()
        {
            _model.Сashiers[2].StartHandling();
            _model.Сashiers[2].SuccessfulStartHandling();
        }

        public void V3_Sucsess()
        {
            _model.Сashiers[2].SuccessfulHandling();
        }

        public void V3_Error()
        {
            _model.Сashiers[2].ErrorHandling();
        }

        #endregion

    }
}