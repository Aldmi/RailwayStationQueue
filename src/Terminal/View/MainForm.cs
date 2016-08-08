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

    public partial class MainForm : Form, IMainForm
    {
        #region prop

        public bool IsConnect
        {
            set
            {
                btnVillage.BackColor = value ? Color.MidnightBlue : Color.Magenta;
                btnLongRoad.BackColor = value ? Color.MidnightBlue : Color.Magenta;
            }
        }

        public bool IsRunDataExchange
        {
            set
            {
                btnVillage.Enabled = !value;
                btnLongRoad.Enabled = !value;
            }
        }

        public string ErrorString
        {
            set
            {
              if (!string.IsNullOrEmpty(value))
                MessageBox.Show(value);
            }
        }

        #endregion




        #region ctor

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion




        #region Events

        //Запрет фокус у TextBox
        private void textBox1_Enter(object sender, EventArgs e)
        {
            btnVillage.Focus();
        }


        public event EventHandler<EventArgs> EhGetInfoVilage;
        private void btnVillage_Click(object sender, EventArgs e)
        {
           EhGetInfoVilage?.Invoke(this, EventArgs.Empty);
        }


        public event EventHandler<EventArgs> EhGetInfoLongRoad;
        private void btnLongRoad_Click(object sender, EventArgs e)
        {
            EhGetInfoLongRoad?.Invoke(this, EventArgs.Empty);
        }

        #endregion




        #region Methods

        public new void Show()
        {
            Application.Run(this);
        }

        #endregion
    }
}
