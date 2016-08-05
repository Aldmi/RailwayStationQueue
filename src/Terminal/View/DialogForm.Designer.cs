namespace Terminal.View
{
    partial class DialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_OK_Click = new System.Windows.Forms.Button();
            this.btn_Cancel_Click = new System.Windows.Forms.Button();
            this.txtBox_ticket = new System.Windows.Forms.TextBox();
            this.txtBox_numberPeople = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_OK_Click
            // 
            this.btn_OK_Click.Location = new System.Drawing.Point(70, 735);
            this.btn_OK_Click.Name = "btn_OK_Click";
            this.btn_OK_Click.Size = new System.Drawing.Size(480, 113);
            this.btn_OK_Click.TabIndex = 0;
            this.btn_OK_Click.Text = "ПРИНЯТЬ";
            this.btn_OK_Click.UseVisualStyleBackColor = true;
            this.btn_OK_Click.Click += new System.EventHandler(this.btn_OK_Click_Click);
            // 
            // btn_Cancel_Click
            // 
            this.btn_Cancel_Click.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel_Click.Location = new System.Drawing.Point(893, 735);
            this.btn_Cancel_Click.Name = "btn_Cancel_Click";
            this.btn_Cancel_Click.Size = new System.Drawing.Size(480, 113);
            this.btn_Cancel_Click.TabIndex = 1;
            this.btn_Cancel_Click.Text = "ОТМЕНА";
            this.btn_Cancel_Click.UseVisualStyleBackColor = true;
            this.btn_Cancel_Click.Click += new System.EventHandler(this.btn_Cancel_Click_Click);
            // 
            // txtBox_ticket
            // 
            this.txtBox_ticket.BackColor = System.Drawing.Color.AliceBlue;
            this.txtBox_ticket.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBox_ticket.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_ticket.Location = new System.Drawing.Point(70, 181);
            this.txtBox_ticket.Name = "txtBox_ticket";
            this.txtBox_ticket.Size = new System.Drawing.Size(1303, 42);
            this.txtBox_ticket.TabIndex = 2;
            this.txtBox_ticket.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtBox_numberPeople
            // 
            this.txtBox_numberPeople.BackColor = System.Drawing.Color.AliceBlue;
            this.txtBox_numberPeople.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBox_numberPeople.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtBox_numberPeople.Location = new System.Drawing.Point(70, 278);
            this.txtBox_numberPeople.Name = "txtBox_numberPeople";
            this.txtBox_numberPeople.Size = new System.Drawing.Size(1303, 42);
            this.txtBox_numberPeople.TabIndex = 3;
            this.txtBox_numberPeople.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.btn_Cancel_Click;
            this.ClientSize = new System.Drawing.Size(1437, 893);
            this.Controls.Add(this.txtBox_numberPeople);
            this.Controls.Add(this.txtBox_ticket);
            this.Controls.Add(this.btn_Cancel_Click);
            this.Controls.Add(this.btn_OK_Click);
            this.Cursor = System.Windows.Forms.Cursors.No;
            this.Name = "DialogForm";
            this.ShowIcon = false;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_OK_Click;
        private System.Windows.Forms.Button btn_Cancel_Click;
        private System.Windows.Forms.TextBox txtBox_ticket;
        private System.Windows.Forms.TextBox txtBox_numberPeople;
    }
}