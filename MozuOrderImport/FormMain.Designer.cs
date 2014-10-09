namespace MozuOrderImport
{
    partial class FormMain
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
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.textBoxCustomerFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonSelectCustomerImportFile = new System.Windows.Forms.Button();
            this.buttonImportCustomers = new System.Windows.Forms.Button();
            this.richTextBoxResults = new System.Windows.Forms.RichTextBox();
            this.textBoxOrderFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSelectOrderImportFile = new System.Windows.Forms.Button();
            this.buttonImportOrderHistory = new System.Windows.Forms.Button();
            this.progressBarStatus = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.loadAllUsers = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxProductOptionsFile = new System.Windows.Forms.TextBox();
            this.buttonSelectProductOptionsFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(130, 7);
            this.textBoxEmail.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(260, 20);
            this.textBoxEmail.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Email Login to Mozu";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            this.label2.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(130, 32);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(158, 20);
            this.textBoxPassword.TabIndex = 1;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(130, 54);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(92, 24);
            this.buttonLogin.TabIndex = 2;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // textBoxCustomerFile
            // 
            this.textBoxCustomerFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCustomerFile.Location = new System.Drawing.Point(130, 91);
            this.textBoxCustomerFile.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxCustomerFile.Name = "textBoxCustomerFile";
            this.textBoxCustomerFile.Size = new System.Drawing.Size(468, 20);
            this.textBoxCustomerFile.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 91);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Customer Import File";
            this.label3.Click += new System.EventHandler(this.label1_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "orderImportFile";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // buttonSelectCustomerImportFile
            // 
            this.buttonSelectCustomerImportFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectCustomerImportFile.Location = new System.Drawing.Point(602, 90);
            this.buttonSelectCustomerImportFile.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSelectCustomerImportFile.Name = "buttonSelectCustomerImportFile";
            this.buttonSelectCustomerImportFile.Size = new System.Drawing.Size(25, 19);
            this.buttonSelectCustomerImportFile.TabIndex = 4;
            this.buttonSelectCustomerImportFile.Text = "...";
            this.buttonSelectCustomerImportFile.UseVisualStyleBackColor = true;
            this.buttonSelectCustomerImportFile.Click += new System.EventHandler(this.buttonSelectCustomerImportFile_Click);
            // 
            // buttonImportCustomers
            // 
            this.buttonImportCustomers.Location = new System.Drawing.Point(130, 114);
            this.buttonImportCustomers.Margin = new System.Windows.Forms.Padding(2);
            this.buttonImportCustomers.Name = "buttonImportCustomers";
            this.buttonImportCustomers.Size = new System.Drawing.Size(116, 24);
            this.buttonImportCustomers.TabIndex = 5;
            this.buttonImportCustomers.Text = "Import Customers";
            this.buttonImportCustomers.UseVisualStyleBackColor = true;
            this.buttonImportCustomers.Click += new System.EventHandler(this.buttonImportCustomers_Click);
            // 
            // richTextBoxResults
            // 
            this.richTextBoxResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxResults.Location = new System.Drawing.Point(130, 312);
            this.richTextBoxResults.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxResults.Name = "richTextBoxResults";
            this.richTextBoxResults.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richTextBoxResults.Size = new System.Drawing.Size(497, 247);
            this.richTextBoxResults.TabIndex = 9;
            this.richTextBoxResults.Text = "";
            // 
            // textBoxOrderFile
            // 
            this.textBoxOrderFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOrderFile.Location = new System.Drawing.Point(130, 166);
            this.textBoxOrderFile.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxOrderFile.Name = "textBoxOrderFile";
            this.textBoxOrderFile.Size = new System.Drawing.Size(468, 20);
            this.textBoxOrderFile.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 166);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Order Import File";
            this.label4.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonSelectOrderImportFile
            // 
            this.buttonSelectOrderImportFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectOrderImportFile.Location = new System.Drawing.Point(602, 165);
            this.buttonSelectOrderImportFile.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSelectOrderImportFile.Name = "buttonSelectOrderImportFile";
            this.buttonSelectOrderImportFile.Size = new System.Drawing.Size(25, 19);
            this.buttonSelectOrderImportFile.TabIndex = 7;
            this.buttonSelectOrderImportFile.Text = "...";
            this.buttonSelectOrderImportFile.UseVisualStyleBackColor = true;
            this.buttonSelectOrderImportFile.Click += new System.EventHandler(this.buttonSelectOrderImportFile_Click);
            // 
            // buttonImportOrderHistory
            // 
            this.buttonImportOrderHistory.Location = new System.Drawing.Point(130, 232);
            this.buttonImportOrderHistory.Margin = new System.Windows.Forms.Padding(2);
            this.buttonImportOrderHistory.Name = "buttonImportOrderHistory";
            this.buttonImportOrderHistory.Size = new System.Drawing.Size(116, 24);
            this.buttonImportOrderHistory.TabIndex = 8;
            this.buttonImportOrderHistory.Text = "Import Order History";
            this.buttonImportOrderHistory.UseVisualStyleBackColor = true;
            this.buttonImportOrderHistory.Click += new System.EventHandler(this.buttonImportOrderHistory_Click);
            // 
            // progressBarStatus
            // 
            this.progressBarStatus.Location = new System.Drawing.Point(172, 291);
            this.progressBarStatus.Margin = new System.Windows.Forms.Padding(2);
            this.progressBarStatus.Name = "progressBarStatus";
            this.progressBarStatus.Size = new System.Drawing.Size(346, 17);
            this.progressBarStatus.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(127, 295);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Status";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(545, 291);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(50, 17);
            this.buttonCancel.TabIndex = 16;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // loadAllUsers
            // 
            this.loadAllUsers.AutoSize = true;
            this.loadAllUsers.Checked = true;
            this.loadAllUsers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadAllUsers.Location = new System.Drawing.Point(13, 349);
            this.loadAllUsers.Name = "loadAllUsers";
            this.loadAllUsers.Size = new System.Drawing.Size(94, 17);
            this.loadAllUsers.TabIndex = 17;
            this.loadAllUsers.Text = "Load All Users";
            this.loadAllUsers.UseVisualStyleBackColor = true;
            this.loadAllUsers.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 211);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Product Options File";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // textBoxProductOptionsFile
            // 
            this.textBoxProductOptionsFile.Location = new System.Drawing.Point(130, 204);
            this.textBoxProductOptionsFile.Name = "textBoxProductOptionsFile";
            this.textBoxProductOptionsFile.Size = new System.Drawing.Size(468, 20);
            this.textBoxProductOptionsFile.TabIndex = 19;
            // 
            // buttonSelectProductOptionsFile
            // 
            this.buttonSelectProductOptionsFile.Location = new System.Drawing.Point(602, 204);
            this.buttonSelectProductOptionsFile.Name = "buttonSelectProductOptionsFile";
            this.buttonSelectProductOptionsFile.Size = new System.Drawing.Size(25, 20);
            this.buttonSelectProductOptionsFile.TabIndex = 20;
            this.buttonSelectProductOptionsFile.Text = "...";
            this.buttonSelectProductOptionsFile.UseVisualStyleBackColor = true;
            this.buttonSelectProductOptionsFile.Click += new System.EventHandler(this.buttonSelectProductOptionsFile_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 568);
            this.Controls.Add(this.buttonSelectProductOptionsFile);
            this.Controls.Add(this.textBoxProductOptionsFile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.loadAllUsers);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.progressBarStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.richTextBoxResults);
            this.Controls.Add(this.buttonImportOrderHistory);
            this.Controls.Add(this.buttonSelectOrderImportFile);
            this.Controls.Add(this.buttonImportCustomers);
            this.Controls.Add(this.buttonSelectCustomerImportFile);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxOrderFile);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxCustomerFile);
            this.Controls.Add(this.textBoxEmail);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormMain";
            this.Text = "Mozu Import Utility";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.TextBox textBoxCustomerFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button buttonSelectCustomerImportFile;
        private System.Windows.Forms.Button buttonImportCustomers;
        private System.Windows.Forms.RichTextBox richTextBoxResults;
        private System.Windows.Forms.TextBox textBoxOrderFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSelectOrderImportFile;
        private System.Windows.Forms.Button buttonImportOrderHistory;
        private System.Windows.Forms.ProgressBar progressBarStatus;
        private System.Windows.Forms.Label label5;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox loadAllUsers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxProductOptionsFile;
        private System.Windows.Forms.Button buttonSelectProductOptionsFile;
    }
}

