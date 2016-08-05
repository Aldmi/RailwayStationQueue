using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communication.Interfaces;
using Communication.SerialPort;
using Communication.Settings;
using Communication.TcpIp;
using Library.Library;
using Library.Logs;
using Library.Xml;
using Server.Infrastructure;
using Server.Model;
using Server.Service;
using Server.Settings;
using Terminal.Infrastructure;


namespace ListViewTest
{
    public partial class MainForm : Form
    {
        #region prop

        public TicketFactory TicketFactoryVilage { get; } = new TicketFactory("П");
        public TicketFactory TicketFactoryLong { get; } = new TicketFactory("Э");

        public Queue<TicketItem> QueueVilage { get; set; } = new Queue<TicketItem>();
        public Queue<TicketItem> QueueLong { get; set; } = new Queue<TicketItem>();

        public Log LogTicket { get; set; }

        public ListenerTcpIp Listener { get; set; }
        public IExchangeDataProvider<TerminalInData, TerminalOutData> ProviderTerminal { get; set; }

        public MasterSerialPort MasterSerialPort { get; set; }
        public List<Сashier> Сashiers { get; set; } = new List<Сashier>();
        public CashierExchangeService CashierExchangeService { get; set; }

        public List<Task> BackGroundTasks { get; set; } = new List<Task>();

        #endregion




        #region ctor

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion




        #region Events

        private async void MainForm_Load(object sender, EventArgs e)
        {
            //ЗАГРУЗКА НАСТРОЕК----------------------------------------------------------------
            XmlListenerSettings xmlListener;
            XmlSerialSettings xmlSerial;
            XmlLogSettings xmlLog;
            XmlProgrammSettings xmlProgramm;
            try
            {
                var xmlFile = XmlWorker.LoadXmlFile("Settings", "Setting.xml"); //все настройки в одном файле
                if (xmlFile == null)
                    return;

                xmlListener = XmlListenerSettings.LoadXmlSetting(xmlFile);
                xmlSerial = XmlSerialSettings.LoadXmlSetting(xmlFile);
                xmlLog = XmlLogSettings.LoadXmlSetting(xmlFile);
                xmlProgramm = XmlProgrammSettings.LoadXmlSetting(xmlFile);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ОШИБКА в узлах дерева XML файла настроек:  " + ex);
                return;
            }


            //СОЗДАНИЕ ЛОГА--------------------------------------------------------------------------
            LogTicket = new Log("TicketLog.txt", xmlLog);


            //СОЗДАНИЕ СЛУШАТЕЛЯ ДЛЯ ТЕРМИНАЛОВ-------------------------------------------------------
            Listener = new ListenerTcpIp(xmlListener);
            ProviderTerminal = new Server2TerminalExchangeDataProvider();


            //СОЗДАНИЕ КАССИРОВ-----------------------------------------------------------------------
            Сashiers.Add(new Сashier(0x01, QueueVilage, xmlProgramm.CashierMaxCountTryHanding));
            Сashiers.Add(new Сashier(0x02, QueueVilage, xmlProgramm.CashierMaxCountTryHanding));
            Сashiers.Add(new Сashier(0x03, QueueVilage, xmlProgramm.CashierMaxCountTryHanding));
            Сashiers.Add(new Сashier(0x04, QueueLong, xmlProgramm.CashierMaxCountTryHanding));
            Сashiers.Add(new Сashier(0x05, QueueLong, xmlProgramm.CashierMaxCountTryHanding));
            Сashiers.Add(new Сashier(0x06, QueueLong, xmlProgramm.CashierMaxCountTryHanding));


            //ПОДПИСКА НА СОБЫТИЯ
            SubscriptionEventsUI();


            //ЗАПУСК СЛУШАТЕЛЯ ДЛЯ ТЕРМИНАЛОВ---------------------------------------------------------
            var taskListener = Listener.RunServer(ProviderTerminal);
            BackGroundTasks.Add(taskListener);


            //ЗАПУСК ОПРОСА КАССИРОВ-------------------------------------------------------------------
            MasterSerialPort = new MasterSerialPort(xmlSerial);
            CashierExchangeService = new CashierExchangeService(Сashiers, xmlSerial.TimeRespoune);
            MasterSerialPort.AddFunc(CashierExchangeService.ExchangeService);
            var taskSerialPort = Task.Factory.StartNew(async () =>
            {
                if (await MasterSerialPort.CycleReConnect(xmlSerial.TimeCycleReConnect))
                {
                    var taskCashierEx = MasterSerialPort.RunExchange();
                    BackGroundTasks.Add(taskCashierEx);
                }
            });
            BackGroundTasks.Add(taskSerialPort);


            //КОНТРОЛЬ ФОНОВЫХ ЗАДАЧ
            var taskFirst = await Task.WhenAny(BackGroundTasks);
            if (taskFirst.Exception != null) //критическая ошибка фоновой задачи
                MessageBox.Show(taskFirst.Exception.ToString());
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Listener?.Dispose();
            MasterSerialPort?.Dispose();
        }

        #endregion




        #region Method

        delegate void EmptyMethodHandler();
        private void SubscriptionEventsUI()
        {
            ProviderTerminal.PropertyChanged += (o, e) =>
            {
                var provider = o as Server2TerminalExchangeDataProvider;
                if (provider != null)
                {
                    if (e.PropertyName == "InputData")
                    {
                        TicketItem ticket;
                        provider.OutputData = provider.OutputData ?? new TerminalOutData();
                        switch (provider.InputData.NumberQueue)
                        {
                            //ПРИГОРОДНЫЕ КАСС
                            case 1:
                                switch (provider.InputData.Action)
                                {
                                    //ИНФОРМАЦИЯ ОБ ОЧЕРЕДИ
                                    case TerminalAction.Info:
                                        provider.OutputData.NumberQueue = provider.InputData.NumberQueue;
                                        provider.OutputData.CountElement = (ushort)QueueVilage.Count;
                                        provider.OutputData.NumberElement = (ushort)(TicketFactoryVilage.GetCurrentTicketNumber + 1);
                                        provider.OutputData.AddedTime = DateTime.Now;
                                        break;

                                    //ДОБАВИТЬ БИЛЕТ В ОЧЕРЕДЬ
                                    case TerminalAction.Add:
                                        ticket = TicketFactoryVilage.Create((ushort)QueueVilage.Count);

                                        provider.OutputData.NumberQueue = provider.InputData.NumberQueue;
                                        provider.OutputData.CountElement = ticket.CountElement;
                                        provider.OutputData.NumberElement = (ushort)ticket.NumberElement;
                                        provider.OutputData.AddedTime = ticket.AddedTime;

                                        QueueVilage.Enqueue(ticket);
                                        break;
                                }
                                break;

                            //ДАЛЬНЕГО СЛЕДОВАНИЯ КАССЫ
                            case 2:
                                switch (provider.InputData.Action)
                                {
                                    //ИНФОРМАЦИЯ ОБ ОЧЕРЕДИ
                                    case TerminalAction.Info:
                                        provider.OutputData.NumberQueue = provider.InputData.NumberQueue;
                                        provider.OutputData.CountElement = (ushort)QueueLong.Count;
                                        provider.OutputData.NumberElement = (ushort)(TicketFactoryLong.GetCurrentTicketNumber + 1);
                                        provider.OutputData.AddedTime = DateTime.Now;
                                        break;

                                    //ДОБАВИТЬ БИЛЕТ В ОЧЕРЕДЬ
                                    case TerminalAction.Add:
                                        ticket = TicketFactoryLong.Create((ushort)QueueLong.Count);

                                        provider.OutputData.NumberQueue = provider.InputData.NumberQueue;
                                        provider.OutputData.CountElement = ticket.CountElement;
                                        provider.OutputData.NumberElement = (ushort)ticket.NumberElement;
                                        provider.OutputData.AddedTime = ticket.AddedTime;

                                        QueueLong.Enqueue(ticket);
                                        break;
                                }
                                break;
                        }
                    }
                }
            };


            Listener.PropertyChanged += (o, e) =>
            {
                var listener = o as ListenerTcpIp;
                if (listener != null)
                {
                    if (e.PropertyName == "IsConnect")
                    {
                        dataGridView1.Invoke(new EmptyMethodHandler(() => { dataGridView1.BackgroundColor = listener.IsConnect ? Color.DimGray : Color.Brown; }));
                    }
                }
            };


            foreach (var cashier in Сashiers)
            {
                cashier.PropertyChanged += async (o, e) =>
                {
                    var c = o as Сashier;
                    if (c != null)
                    {
                        if (e.PropertyName == "CurrentTicket")
                        {
                            if (c.CurrentTicket != null)      //добавить элемент к списку
                            {
                                dataGridView1.Rows.Add($"Талон {c.CurrentTicket.Prefix}{c.CurrentTicket.NumberElement.ToString("000")}", "Касса " + c.CurrentTicket.Сashbox);
                                var task = LogTicket?.Add(c.CurrentTicket.ToString());
                                if (task != null) await task;
                            }
                            else                             //удалить элемент из списка
                            {
                                for (var i = 0; i < dataGridView1.Rows.Count; i++)
                                {
                                    string val = dataGridView1.Rows[i].Cells[1].Value as string;
                                    if (val != null && val.Last().ToString() == c.Id.ToString())
                                    {
                                        dataGridView1.Rows.RemoveAt(i);
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }


        #endregion





        #region DEBUG

        private void btn_vilage_Click(object sender, EventArgs e)
        {
            var ticket = TicketFactoryVilage.Create((ushort)QueueVilage.Count);
            QueueVilage.Enqueue(ticket);
        }

        private void btn_long_Click(object sender, EventArgs e)
        {
            var ticket = TicketFactoryLong.Create((ushort)QueueLong.Count);
            QueueLong.Enqueue(ticket);
        }




        BlockClick blockClick1 = new BlockClick(1000);
        private void btn_V1_Add_Click(object sender, EventArgs e)
        {
            //if (blockClick1.IsClicHot) return;
            //blockClick1.BlockClickStart();

            Сashiers[0].StartHandling();
            Сashiers[0].SuccessfulStartHandling();
        }

        private void btn_V1_Sucsess_Click(object sender, EventArgs e)
        {
            Сashiers[0].SuccessfulHandling();
        }

        private void btn_V1_Error_Click(object sender, EventArgs e)
        {
            Сashiers[0].ErrorHandling();
        }


        private void btn_V2_Add_Click(object sender, EventArgs e)
        {
            Сashiers[1].StartHandling();
            Сashiers[1].SuccessfulStartHandling();
        }

        private void btn_V2_Sucsess_Click(object sender, EventArgs e)
        {
            Сashiers[1].SuccessfulHandling();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Сashiers[1].ErrorHandling();
        }




        private void btn_V3_Add_Click(object sender, EventArgs e)
        {
            Сashiers[2].StartHandling();
            Сashiers[2].SuccessfulStartHandling();
        }

        private void btn_V3_Sucsess_Click(object sender, EventArgs e)
        {
            Сashiers[2].SuccessfulHandling();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Сashiers[2].ErrorHandling();
        }

        #endregion

    }
}
