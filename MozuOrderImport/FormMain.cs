using Mozu.Api;
using Mozu.Api.Contracts.Tenant;
using Mozu.Api.Security;
using MozuImport;
using MozuImport.Processes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MozuOrderImport
{
    public partial class FormMain : Form
    {
        private ApiContext _apiContext = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string emailAddress = textBoxEmail.Text;
                string password = textBoxPassword.Text;

                var accountInfo = Account.Login(emailAddress, password);
                DisplayStatus("Log in successful.\r\nSelect sandbox...");

                SandboxSelector dialog = new SandboxSelector(accountInfo);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    Scope selected = dialog.Sandbox;

                    Tenant tenant = Account.GetTenant(selected.Id);

                    Site site = tenant.Sites[0];
                    MasterCatalog masterCatalog = tenant.MasterCatalogs[0];

                    _apiContext = new ApiContext(tenant.Id, site.Id, masterCatalogId: masterCatalog.Id,
                        catalogId: site.CatalogId);

                    DisplayStatus(string.Format("Tenant: {0}\r\nName:{1}\r\nIsDevTenant:{2}\r\nSite:{3}\r\nMasterCatalog:{4}", 
                        tenant.Id,
                        tenant.Name,
                        tenant.IsDevTenant,
                        tenant.Sites[0].Name,
                        tenant.MasterCatalogs[0].Name
                        ));
                }
            }
            catch (Exception ex)
            {
                DisplayStatus(ex);
            }

            // Reset the cursor
            this.Cursor = Cursors.Default;
        }

        private void buttonSelectCustomerImportFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Excel Files|*.xlsx";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.textBoxCustomerFile.Text = openFileDialog.FileName;
            }

        }


        private void buttonSelectOrderImportFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Excel Files|*.xlsx";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.textBoxOrderFile.Text = openFileDialog.FileName;
            }
        }

        private void buttonSelectProductOptionsFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Excel Files|*.xlsx";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.textBoxProductOptionsFile.Text = openFileDialog.FileName;
            }
        }

        private void buttonImportCustomers_Click(object sender, EventArgs e)
        {
            DisplayStatus(null); // clear out the status window'

            // disable things that should be disabled before the process starts
            this.buttonImportCustomers.Enabled = false;
            this.buttonCancel.Enabled = true;
            this.Cursor = Cursors.WaitCursor;

            // Set up event handlers
            this.backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            this.backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            this.backgroundWorker.DoWork += backgroundWorker_DoWork;

            // Set up parameters
            var context = new ProcessContext()
            {
                ApiContext = _apiContext,
                Process = new CustomerImportProcess()
            };
            context.Parameters["CustomerImportFile"] = this.textBoxCustomerFile.Text;

            // Run the process
            this.backgroundWorker.RunWorkerAsync(context);

            // Handle things back at the ranch
            while (backgroundWorker.IsBusy)
            {
                // Keep UI messages moving, so the form remains  
                // responsive during the asynchronous operation.
                Application.DoEvents();
            }

            // Reset the cursor
            this.Cursor = Cursors.Default;

            // Tear down event handlers
            this.backgroundWorker.DoWork -= backgroundWorker_DoWork;
            this.backgroundWorker.RunWorkerCompleted -= backgroundWorker_RunWorkerCompleted;
            this.backgroundWorker.ProgressChanged -= backgroundWorker_ProgressChanged;

            // reenable things back at the ranch
            this.buttonImportCustomers.Enabled = true;
            this.buttonCancel.Enabled = false;
        }
        
        private void buttonImportOrderHistory_Click(object sender, EventArgs e)
        {
            DisplayStatus(null); // clear out the status window'

            // disable things that should be disabled before the process starts
            this.buttonImportOrderHistory.Enabled = false;
            this.buttonCancel.Enabled = true;
            this.Cursor = Cursors.WaitCursor;

            // Set up event handlers
            this.backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            this.backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            this.backgroundWorker.DoWork += backgroundWorker_DoWork;

            // Set up parameters
            var context = new ProcessContext()
            {
                ApiContext = _apiContext,
                Process = new OrderHistoryImportProcess()
            };
            context.Parameters["OrderHistoryImportFile"] = this.textBoxOrderFile.Text;
            context.Parameters["ProductOptionsFile"] = this.textBoxProductOptionsFile.Text;
            context.Parameters["loadAllUsers"] = this.loadAllUsers.Checked.ToString();

            // Run the process
            this.backgroundWorker.RunWorkerAsync(context);

            // Handle things back at the ranch
            while (backgroundWorker.IsBusy)
            {
                // Keep UI messages moving, so the form remains  
                // responsive during the asynchronous operation.
                Application.DoEvents();
            }

            // Reset the cursor
            this.Cursor = Cursors.Default;

            // Tear down event handlers
            this.backgroundWorker.DoWork -= backgroundWorker_DoWork;
            this.backgroundWorker.RunWorkerCompleted -= backgroundWorker_RunWorkerCompleted;
            this.backgroundWorker.ProgressChanged -= backgroundWorker_ProgressChanged;

            // reenable things back at the ranch
            this.buttonImportOrderHistory.Enabled = true;
            this.buttonCancel.Enabled = false;
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Perform process work as a background process
            var context = (IProcessContext)e.Argument;
            context.Worker = sender as BackgroundWorker;
            context.Process.Run(context);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBarStatus.Value = e.ProgressPercentage;
            DisplayStatus(e.UserState);
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.UserState != null)
                DisplayStatus(e.UserState);
        }

        private void DisplayStatus(object obj)
        {
            if (obj != null)
            {
                richTextBoxResults.AppendText(obj.ToString() + "\r\n");
                richTextBoxResults.SelectionStart = richTextBoxResults.Text.Length;
                richTextBoxResults.ScrollToCaret();
            }
            else
            {
                this.richTextBoxResults.Text = string.Empty;
            }
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }



    }
}
