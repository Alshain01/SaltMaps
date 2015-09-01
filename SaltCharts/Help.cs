using System;
using System.IO;
using System.Windows.Forms;

namespace SaltCharts
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            this.webHelp.Url = new Uri(String.Format("file:///{0}/Help.html", curDir));
        }
    }
}
