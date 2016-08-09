using System;
using System.Windows.Forms;

namespace Terminal.View
{
    public partial class DialogForm : Form, IDialogForm
    {
        #region prop

        public string TicketName
        {
            set { txtBox_ticket.Text = $"Номер вашего билета {value}"; }
        }

        public string CountPeople
        {
            set { txtBox_numberPeople.Text = $"Впереди вас {value} человек"; }
        }

        #endregion




        #region ctor

        public DialogForm()
        {
            InitializeComponent();
        }

        #endregion




        #region Events

        public event EventHandler<EventArgs> EhOk;
        private void btn_OK_Click_Click(object sender, EventArgs e)
        {
            EhOk?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> EhCancel;
        private void btn_Cancel_Click_Click(object sender, EventArgs e)
        {
            EhCancel?.Invoke(this, EventArgs.Empty);
        }

        #endregion




        #region Methods

        public new void Show()
        {
            ShowDialog();
        }

        #endregion 
    }
}
