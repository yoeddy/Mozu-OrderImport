using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mozu.Api.Security;
using System.Collections.Generic;
using Mozu.Api.Contracts.Core;
using Mozu.Api.Security;
using MozuImport;

namespace MozuOrderImport
{
    public partial class SandboxSelector : Form
    {
        // The sandbox scope selected
        public Scope Sandbox { get; set; }

        private Dictionary<Int32, Scope> _sandboxes;

        public SandboxSelector(AuthenticationProfile authenticationProfile)
        {
            InitializeComponent();

            // Add all of the sandboxes to the list
            _sandboxes = Account.GetSandboxes(authenticationProfile);
            foreach (var scope in _sandboxes.Values)
            {
                this.listSandboxes.Items.Add(new ViewScope(scope));
            }
        }

        private void listSandboxes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Sandbox = ((ViewScope) listSandboxes.SelectedItem).Wrapped;
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }

    internal class ViewScope
    {
        public Scope Wrapped { get; private set; }

        public ViewScope(Scope scope)
        {
            Wrapped = scope;
        }


        public override string ToString()
        {
            return Wrapped.Id + " " + Wrapped.Name;
        }
    }
}
