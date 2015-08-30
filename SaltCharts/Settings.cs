using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaltCharts
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Shown(object sender, EventArgs e)
        {
            chkAutoSave.Checked = SaltCharts.Properties.Settings.Default.AutoSave;
            chkDebug.Checked = SaltCharts.Properties.Settings.Default.Debug;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaltCharts.Properties.Settings.Default.AutoSave = chkAutoSave.Checked;
            SaltCharts.Properties.Settings.Default.Debug = chkDebug.Checked;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
