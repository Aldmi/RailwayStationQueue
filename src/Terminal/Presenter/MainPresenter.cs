using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class MainPresenter : IPresenter
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
            _view.EhGetInfoLong += _view_EhGetInfoLong;
            _view.EhGetInfoVilage += _view_EhGetInfoVilage;

            _model = new TerminalModel();
            _model.MasterTcpIp.PropertyChanged += _model_MasterTcpIp_PropertyChanged;
            _model.ConfirmationAdded += _model_ConfirmationAdded;
            _mainTask = _model.Start();
        }

        #endregion




        #region EventHandler

        private async void _view_EhGetInfoVilage(object sender, EventArgs e)
        {
            const byte numberQueue = 1;
            await _model.TrainSelection(numberQueue);
        }


        private async void _view_EhGetInfoLong(object sender, EventArgs e)
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
                    _view.IsConnect = master.IsConnect;
                    //btnVillage.BackColor = master.IsConnect ? Color.MidnightBlue : Color.Magenta;
                    //btnLongRoad.BackColor = master.IsConnect ? Color.MidnightBlue : Color.Magenta;
                }
                else if (e.PropertyName == "IsRunDataExchange")
                {
                    _view.IsRunDataExchange = !master.IsRunDataExchange;
                    //btnVillage.Enabled = !master.IsRunDataExchange;
                    //btnLongRoad.Enabled = !master.IsRunDataExchange;
                }
            }
        }

        
        private bool _model_ConfirmationAdded(string ticketName, string countPeople)
        {
            //TODO: Работать через Presenter
            var dialogForm = new DialogForm(ticketName, countPeople);
            var result = dialogForm.ShowDialog();

            return true;
        }

        #endregion




        #region Methode

        public void Run()
        {
            _view.Show();
        }

        #endregion
    }
}
