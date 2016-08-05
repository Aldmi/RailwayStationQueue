using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communication.Settings;
using Communication.TcpIp;
using Library.Xml;
using Terminal.Infrastructure;
using Terminal.Service;

namespace Terminal.View
{
    //TODO: реализовать паттерн MVP
    public partial class MainForm : Form
    {
        #region prop

        public MasterTcpIp MasterTcpIp { get; set; }
        public PrintTicket PrintTicket { get; set; }

        #endregion




        #region ctor

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion




        #region Events

        private async void Form1_Load(object sender, EventArgs e)
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
                MessageBox.Show(ex.ToString());
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ОШИБКА в узлах дерева XML файла настроек:  " + ex);
                return;
            }

            MasterTcpIp = new MasterTcpIp(xmlTerminal);
            PrintTicket = new PrintTicket();              //TODO: get printername

            SubscriptionEventsUI();

            await MasterTcpIp.ReConnect();
        }


        //Запрет фокус у TextBox
        private void textBox1_Enter(object sender, EventArgs e)
        {
            btnVillage.Focus();
        }


        private async void btnVillage_Click(object sender, EventArgs e)
        {
            const byte numberQueue = 1;
            await TrainSelection(numberQueue);
        }


        private async void btnLongRoad_Click(object sender, EventArgs e)
        {
            const byte numberQueue = 2;
            await TrainSelection(numberQueue);
        }

        #endregion




        #region Methods

        private async Task TrainSelection(byte numberQueue)
        {
            //ЗАПРОС О СОСТОЯНИИ ОЧЕРЕДИ
            var provider = new Terminal2ServerExchangeDataProvider { InputData = new TerminalInData { NumberQueue = numberQueue, Action = TerminalAction.Info } };
            await MasterTcpIp.RequestAndRespoune(provider);

            if (provider.IsOutDataValid)
            {
                var prefix = provider.OutputData.NumberQueue == 1 ? "П" : "Э";
                var ticketName = prefix + provider.OutputData.NumberElement.ToString("000");
                var countPeople = provider.OutputData.CountElement.ToString();

                var dialogForm = new DialogForm(ticketName, countPeople);
                var result = dialogForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    //ЗАПРОС О ДОБАВЛЕНИИ ЭЛЕМЕНТА В ОЧЕРЕДЬ
                    provider = new Terminal2ServerExchangeDataProvider { InputData = new TerminalInData { NumberQueue = numberQueue, Action = TerminalAction.Add } };
                    await MasterTcpIp.RequestAndRespoune(provider);

                    if (provider.IsOutDataValid)
                    {
                        prefix = provider.OutputData.NumberQueue == 1 ? "П" : "Э";
                        ticketName = prefix + provider.OutputData.NumberElement.ToString("000");
                        countPeople = provider.OutputData.CountElement.ToString();

                        MessageBox.Show($"Печать!!!\n№ билета:{ticketName}\n  Кол-во клиентов перед вами:{countPeople}\n Дата: {provider.OutputData.AddedTime}\n");
                        PrintTicket.Print(ticketName, countPeople, provider.OutputData.AddedTime);
                    }
                }
                else
                {
                    // MessageBox.Show("НЕ добавлять");
                }
            }
            else
            {
                MessageBox.Show(MasterTcpIp.StatusString);
            }
        }


        private void SubscriptionEventsUI()
        {
            MasterTcpIp.PropertyChanged += (o, e) =>
            {
                var master = o as MasterTcpIp;
                if (master != null)
                {
                    if (e.PropertyName == "IsConnect")
                    {
                        btnVillage.BackColor = master.IsConnect ? Color.MidnightBlue : Color.Magenta;
                        btnLongRoad.BackColor = master.IsConnect ? Color.MidnightBlue : Color.Magenta;
                    }
                    else
                    if (e.PropertyName == "IsRunDataExchange")
                    {
                        btnVillage.Enabled = !master.IsRunDataExchange;
                        btnLongRoad.Enabled = !master.IsRunDataExchange;
                    }
                }
            };
        }

        #endregion
    }
}
