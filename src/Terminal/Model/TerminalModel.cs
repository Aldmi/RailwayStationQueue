using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communication.Settings;
using Communication.TcpIp;
using Library.Xml;
using Terminal.Infrastructure;
using Terminal.Service;
using Terminal.View;


namespace Terminal.Model
{
    public class TerminalModel
    {
        #region prop

        public MasterTcpIp MasterTcpIp { get; set; }
        public PrintTicket PrintTicket { get; set; }

        #endregion





        #region Events

        public event Func<string, string, bool> ConfirmationAdded;

        private bool OnConfirmationAdded(string arg1, string arg2)
        {
            var res = ConfirmationAdded?.Invoke(arg1, arg2);
            return res != null && res.Value;
        }

        #endregion





        #region Methods

        public async Task Start()
        {
            //ЗАГРУЗКА НАСТРОЕК------------------------------------------------------
            XmlMasterSettings xmlTerminal;
            try
            {
                var xmlFile = XmlWorker.LoadXmlFile("Settings", "Setting.xml");
                if (xmlFile == null)
                    return;

                xmlTerminal = XmlMasterSettings.LoadXmlSetting(xmlFile);
            }
            catch (FileNotFoundException ex)
            {
                //MessageBox.Show(ex.ToString());  //TODO: как прокинуть MessageBox
                return;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("ОШИБКА в узлах дерева XML файла настроек:  " + ex);
                return;
            }

            MasterTcpIp = new MasterTcpIp(xmlTerminal);
            PrintTicket = new PrintTicket();              //TODO: get printername

            await MasterTcpIp.ReConnect();
        }




        public async Task TrainSelection(byte numberQueue)
        {
            if (!MasterTcpIp.IsConnect)
                return;

            //ЗАПРОС О СОСТОЯНИИ ОЧЕРЕДИ
            var provider = new Terminal2ServerExchangeDataProvider { InputData = new TerminalInData { NumberQueue = numberQueue, Action = TerminalAction.Info } };
            await MasterTcpIp.RequestAndRespoune(provider);

            if (provider.IsOutDataValid)
            {
                var prefix = provider.OutputData.NumberQueue == 1 ? "П" : "Э";
                var ticketName = prefix + provider.OutputData.NumberElement.ToString("000");
                var countPeople = provider.OutputData.CountElement.ToString();

                //var dialogForm = new DialogForm(ticketName, countPeople);
                //var result = dialogForm.ShowDialog();

                var result= OnConfirmationAdded(ticketName, countPeople);
                if (result)
                {
                    //ЗАПРОС О ДОБАВЛЕНИИ ЭЛЕМЕНТА В ОЧЕРЕДЬ
                    provider = new Terminal2ServerExchangeDataProvider { InputData = new TerminalInData { NumberQueue = numberQueue, Action = TerminalAction.Add } };
                    await MasterTcpIp.RequestAndRespoune(provider);

                    if (provider.IsOutDataValid)
                    {
                        prefix = provider.OutputData.NumberQueue == 1 ? "П" : "Э";
                        ticketName = prefix + provider.OutputData.NumberElement.ToString("000");
                        countPeople = provider.OutputData.CountElement.ToString();

                        //MessageBox.Show($"Печать!!!\n№ билета:{ticketName}\n  Кол-во клиентов перед вами:{countPeople}\n Дата: {provider.OutputData.AddedTime}\n");
                        PrintTicket.Print(ticketName, countPeople, provider.OutputData.AddedTime);
                    }
                }
                else
                {
                    // "НЕ добавлять"
                }
            }
        }

        #endregion
    }
}