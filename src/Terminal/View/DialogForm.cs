using System;
using System.Windows.Forms;

namespace Terminal.View
{
    public partial class DialogForm : Form
    {
        private DialogForm()
        {
            InitializeComponent();
        }


        public DialogForm(string ticketName, string countPeople) : this()
        {
            txtBox_ticket.Text = $"Номер вашего билета {ticketName}";
            txtBox_numberPeople.Text = $"Впереди вас {countPeople} человек";
        }

        private void btn_OK_Click_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btn_Cancel_Click_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
