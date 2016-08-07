namespace Terminal.View
{
    partial class MainForm
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
            this.btnVillage = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnLongRoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnVillage
            // 
            this.btnVillage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVillage.BackColor = System.Drawing.Color.DimGray;
            this.btnVillage.Font = new System.Drawing.Font("Arial", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnVillage.ForeColor = System.Drawing.Color.White;
            this.btnVillage.Location = new System.Drawing.Point(71, 264);
            this.btnVillage.Name = "btnVillage";
            this.btnVillage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnVillage.Size = new System.Drawing.Size(705, 75);
            this.btnVillage.TabIndex = 0;
            this.btnVillage.Text = "Электропоезд";
            this.btnVillage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnVillage.UseVisualStyleBackColor = false;
            this.btnVillage.Click += new System.EventHandler(this.btnVillage_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.Crimson;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.No;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(1, 1);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(843, 220);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "\r\nКакой билет вы хотите приобрести ?";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            // 
            // btnLongRoad
            // 
            this.btnLongRoad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLongRoad.BackColor = System.Drawing.Color.DimGray;
            this.btnLongRoad.Font = new System.Drawing.Font("Arial", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLongRoad.ForeColor = System.Drawing.Color.White;
            this.btnLongRoad.Location = new System.Drawing.Point(71, 368);
            this.btnLongRoad.Name = "btnLongRoad";
            this.btnLongRoad.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnLongRoad.Size = new System.Drawing.Size(705, 75);
            this.btnLongRoad.TabIndex = 4;
            this.btnLongRoad.Text = "Поезд дальнего следования";
            this.btnLongRoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLongRoad.UseVisualStyleBackColor = false;
            this.btnLongRoad.Click += new System.EventHandler(this.btnLongRoad_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 475);
            this.Controls.Add(this.btnLongRoad);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnVillage);
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnVillage;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnLongRoad;
    }
}

