namespace MariEtFemme.Agendamento
{
    partial class UserControl1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancelClient = new System.Windows.Forms.Button();
            this.btnApplyClient = new System.Windows.Forms.Button();
            this.btnAddService = new System.Windows.Forms.Button();
            this.btnRemoveService = new System.Windows.Forms.Button();
            this.cbServices = new System.Windows.Forms.ComboBox();
            this.lblServices = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.lblComments = new System.Windows.Forms.Label();
            this.btnSearchClient = new System.Windows.Forms.Button();
            this.txtClientName = new System.Windows.Forms.TextBox();
            this.lblClientName = new System.Windows.Forms.Label();
            this.dataGridViewServices = new System.Windows.Forms.DataGridView();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.txtColor = new System.Windows.Forms.TextBox();
            this.txtBorderColor = new System.Windows.Forms.TextBox();
            this.btnAttendance = new System.Windows.Forms.Button();
            this.dataGridViewClients = new System.Windows.Forms.DataGridView();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.lblBorderColor = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClients)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancelClient
            // 
            this.btnCancelClient.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancelClient.Location = new System.Drawing.Point(165, 285);
            this.btnCancelClient.Name = "btnCancelClient";
            this.btnCancelClient.Size = new System.Drawing.Size(75, 23);
            this.btnCancelClient.TabIndex = 37;
            this.btnCancelClient.Text = "Cancelar";
            this.btnCancelClient.UseVisualStyleBackColor = false;
            this.btnCancelClient.Click += new System.EventHandler(this.btnCancelClient_Click);
            // 
            // btnApplyClient
            // 
            this.btnApplyClient.BackColor = System.Drawing.SystemColors.Control;
            this.btnApplyClient.Location = new System.Drawing.Point(245, 285);
            this.btnApplyClient.Name = "btnApplyClient";
            this.btnApplyClient.Size = new System.Drawing.Size(75, 23);
            this.btnApplyClient.TabIndex = 36;
            this.btnApplyClient.Text = "Confirmar";
            this.btnApplyClient.UseVisualStyleBackColor = false;
            this.btnApplyClient.Click += new System.EventHandler(this.btnApplyClient_Click);
            // 
            // btnAddService
            // 
            this.btnAddService.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddService.Location = new System.Drawing.Point(285, 35);
            this.btnAddService.Name = "btnAddService";
            this.btnAddService.Size = new System.Drawing.Size(35, 23);
            this.btnAddService.TabIndex = 34;
            this.btnAddService.Text = "+";
            this.btnAddService.UseVisualStyleBackColor = false;
            this.btnAddService.Click += new System.EventHandler(this.btnAddService_Click);
            // 
            // btnRemoveService
            // 
            this.btnRemoveService.BackColor = System.Drawing.SystemColors.Control;
            this.btnRemoveService.Location = new System.Drawing.Point(246, 35);
            this.btnRemoveService.Name = "btnRemoveService";
            this.btnRemoveService.Size = new System.Drawing.Size(35, 23);
            this.btnRemoveService.TabIndex = 33;
            this.btnRemoveService.Text = "-";
            this.btnRemoveService.UseVisualStyleBackColor = false;
            this.btnRemoveService.Click += new System.EventHandler(this.btnRemoveService_Click);
            // 
            // cbServices
            // 
            this.cbServices.FormattingEnabled = true;
            this.cbServices.Location = new System.Drawing.Point(85, 35);
            this.cbServices.Name = "cbServices";
            this.cbServices.Size = new System.Drawing.Size(150, 21);
            this.cbServices.TabIndex = 32;
            // 
            // lblServices
            // 
            this.lblServices.AutoSize = true;
            this.lblServices.Location = new System.Drawing.Point(0, 40);
            this.lblServices.Name = "lblServices";
            this.lblServices.Size = new System.Drawing.Size(57, 13);
            this.lblServices.TabIndex = 31;
            this.lblServices.Text = "Serviços:  ";
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.SystemColors.Control;
            this.btnRemove.Location = new System.Drawing.Point(85, 285);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 30;
            this.btnRemove.Text = "Excluir";
            this.btnRemove.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.Location = new System.Drawing.Point(165, 285);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 29;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Location = new System.Drawing.Point(245, 285);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "Confirmar";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(85, 156);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(235, 80);
            this.txtComments.TabIndex = 27;
            // 
            // lblComments
            // 
            this.lblComments.AutoSize = true;
            this.lblComments.Location = new System.Drawing.Point(0, 156);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(79, 13);
            this.lblComments.TabIndex = 26;
            this.lblComments.Text = "Observações:  ";
            // 
            // btnSearchClient
            // 
            this.btnSearchClient.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearchClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchClient.Location = new System.Drawing.Point(245, 0);
            this.btnSearchClient.Name = "btnSearchClient";
            this.btnSearchClient.Size = new System.Drawing.Size(75, 23);
            this.btnSearchClient.TabIndex = 25;
            this.btnSearchClient.Text = ". . .";
            this.btnSearchClient.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSearchClient.UseVisualStyleBackColor = false;
            this.btnSearchClient.Click += new System.EventHandler(this.btnSearchClient_Click);
            // 
            // txtClientName
            // 
            this.txtClientName.Location = new System.Drawing.Point(85, 1);
            this.txtClientName.Name = "txtClientName";
            this.txtClientName.Size = new System.Drawing.Size(150, 20);
            this.txtClientName.TabIndex = 24;
            // 
            // lblClientName
            // 
            this.lblClientName.AutoSize = true;
            this.lblClientName.Location = new System.Drawing.Point(0, 5);
            this.lblClientName.Name = "lblClientName";
            this.lblClientName.Size = new System.Drawing.Size(48, 13);
            this.lblClientName.TabIndex = 23;
            this.lblClientName.Text = "Cliente:  ";
            // 
            // dataGridViewServices
            // 
            this.dataGridViewServices.AllowUserToResizeRows = false;
            this.dataGridViewServices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewServices.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewServices.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewServices.ColumnHeadersVisible = false;
            this.dataGridViewServices.Location = new System.Drawing.Point(85, 61);
            this.dataGridViewServices.MultiSelect = false;
            this.dataGridViewServices.Name = "dataGridViewServices";
            this.dataGridViewServices.ReadOnly = true;
            this.dataGridViewServices.RowHeadersVisible = false;
            this.dataGridViewServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewServices.Size = new System.Drawing.Size(235, 80);
            this.dataGridViewServices.TabIndex = 38;
            // 
            // txtColor
            // 
            this.txtColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtColor.Location = new System.Drawing.Point(85, 251);
            this.txtColor.Name = "txtColor";
            this.txtColor.ReadOnly = true;
            this.txtColor.Size = new System.Drawing.Size(65, 20);
            this.txtColor.TabIndex = 40;
            // 
            // txtBorderColor
            // 
            this.txtBorderColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtBorderColor.Location = new System.Drawing.Point(255, 251);
            this.txtBorderColor.Name = "txtBorderColor";
            this.txtBorderColor.ReadOnly = true;
            this.txtBorderColor.Size = new System.Drawing.Size(65, 20);
            this.txtBorderColor.TabIndex = 42;
            // 
            // btnAttendance
            // 
            this.btnAttendance.BackColor = System.Drawing.SystemColors.Control;
            this.btnAttendance.Location = new System.Drawing.Point(4, 285);
            this.btnAttendance.Name = "btnAttendance";
            this.btnAttendance.Size = new System.Drawing.Size(75, 23);
            this.btnAttendance.TabIndex = 43;
            this.btnAttendance.Text = "Atender";
            this.btnAttendance.UseVisualStyleBackColor = false;
            // 
            // dataGridViewClients
            // 
            this.dataGridViewClients.AllowUserToAddRows = false;
            this.dataGridViewClients.AllowUserToDeleteRows = false;
            this.dataGridViewClients.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewClients.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewClients.Location = new System.Drawing.Point(1, 1);
            this.dataGridViewClients.MultiSelect = false;
            this.dataGridViewClients.Name = "dataGridViewClients";
            this.dataGridViewClients.ReadOnly = true;
            this.dataGridViewClients.RowHeadersVisible = false;
            this.dataGridViewClients.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewClients.Size = new System.Drawing.Size(320, 275);
            this.dataGridViewClients.TabIndex = 35;
            // 
            // lblBackColor
            // 
            this.lblBackColor.AutoSize = true;
            this.lblBackColor.Location = new System.Drawing.Point(0, 255);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(74, 13);
            this.lblBackColor.TabIndex = 44;
            this.lblBackColor.Text = "Cor do Fundo:";
            // 
            // lblBorderColor
            // 
            this.lblBorderColor.AutoSize = true;
            this.lblBorderColor.Location = new System.Drawing.Point(173, 255);
            this.lblBorderColor.Name = "lblBorderColor";
            this.lblBorderColor.Size = new System.Drawing.Size(72, 13);
            this.lblBorderColor.TabIndex = 45;
            this.lblBorderColor.Text = "Cor da Borda:";
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.dataGridViewClients);
            this.Controls.Add(this.lblBorderColor);
            this.Controls.Add(this.lblBackColor);
            this.Controls.Add(this.btnAttendance);
            this.Controls.Add(this.txtBorderColor);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.btnCancelClient);
            this.Controls.Add(this.btnApplyClient);
            this.Controls.Add(this.btnAddService);
            this.Controls.Add(this.btnRemoveService);
            this.Controls.Add(this.cbServices);
            this.Controls.Add(this.lblServices);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.lblComments);
            this.Controls.Add(this.btnSearchClient);
            this.Controls.Add(this.txtClientName);
            this.Controls.Add(this.lblClientName);
            this.Controls.Add(this.dataGridViewServices);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(325, 320);
            this.Load += new System.EventHandler(this.EditAppointment_Load);
            this.Enter += new System.EventHandler(this.EditAppointment_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClients)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancelClient;
        private System.Windows.Forms.Button btnApplyClient;
        public System.Windows.Forms.Button btnAddService;
        public System.Windows.Forms.Button btnRemoveService;
        public System.Windows.Forms.ComboBox cbServices;
        public System.Windows.Forms.Label lblServices;
        public System.Windows.Forms.Button btnRemove;
        public System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.TextBox txtComments;
        public System.Windows.Forms.Label lblComments;
        public System.Windows.Forms.Button btnSearchClient;
        public System.Windows.Forms.TextBox txtClientName;
        public System.Windows.Forms.Label lblClientName;
        public System.Windows.Forms.DataGridView dataGridViewServices;
        private System.Windows.Forms.ColorDialog colorDialog;
        public System.Windows.Forms.TextBox txtColor;
        public System.Windows.Forms.TextBox txtBorderColor;
        public System.Windows.Forms.Button btnAttendance;
        private System.Windows.Forms.DataGridView dataGridViewClients;
        private System.Windows.Forms.Label lblBackColor;
        private System.Windows.Forms.Label lblBorderColor;
    }
}
