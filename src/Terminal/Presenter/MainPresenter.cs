using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communication.Settings;
using Communication.TcpIp;
using Library.Xml;
using Terminal.Infrastructure;
using Terminal.Model;
using Terminal.Service;
using Terminal.View;


namespace Terminal.Presenter
{
    public class MainPresenter : IPresenter, IDisposable
    {
        #region field

        private readonly IMainForm _view;
        private readonly TerminalModel _model;
        private readonly Task _mainTask;

        #endregion




        #region ctor

        public MainPresenter(IMainForm view)
        {
            _view = view;
            _view.EhGetInfoLongRoad += ViewEhGetInfoLongRoad;
            _view.EhGetInfoVilage += _view_EhGetInfoVilage;

            _model = new TerminalModel();
            _model.PropertyChanged += _model_PropertyChanged;
            _model.ConfirmationAdded += _model_ConfirmationAdded;

            _model.LoadSetting();
            if (_model.MasterTcpIp != null)
            {
                _model.MasterTcpIp.PropertyChanged += _model_MasterTcpIp_PropertyChanged;
                _mainTask = _model.Start();
            }
        }

        #endregion




        #region EventHandler

        private async void _view_EhGetInfoVilage(object sender, EventArgs e)
        {
            const byte numberQueue = 1;
            await _model.TrainSelection(numberQueue);
        }


        private async void ViewEhGetInfoLongRoad(object sender, EventArgs e)
        {
            const byte numberQueue = 2;
            await _model.TrainSelection(numberQueue);
        }


        private void _model_MasterTcpIp_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var master = sender as MasterTcpIp;
            if (master != null)
            {
                if (e.PropertyName == "IsConnect")
                {
                    _view.BackgroundColorDataGrid = master.IsConnect ? Color.MidnightBlue : Color.Magenta;
                }
                else if (e.PropertyName == "IsRunDataExchange")
                {
                    _view.BattonEnable = master.IsRunDataExchange;
                }
            }
        }


        private void _model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var terminal = sender as TerminalModel;
            if (terminal != null)
            {
                if (e.PropertyName == "ErrorString")
                {
                    _view.ErrorString = terminal.ErrorString;
                }
            }
        }


        private bool _model_ConfirmationAdded(string ticketName, string countPeople)
        {
            var dialogPresenter = new DialogPresenter(new DialogForm(), ticketName, countPeople);
            dialogPresenter.Run();

            return dialogPresenter.Act == Act.Ok;
        }

        #endregion




        #region Methode

        public void Run()
        {
            _view.Show();
        }

        #endregion





        #region IDisposable

        public void Dispose()
        {
            _model.Dispose();
        }

        #endregion
    }
}
