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
    public enum ConflictResponse
    {
        Merged,
        Original,
        New
    }

    public partial class WaypointConflict : Form
    {
        public Waypoint Merged { get; private set;  }
        private Waypoint Incoming;
        private Waypoint Original;
        public bool ContinueWithRemaining { get; private set; }
        public ConflictResponse Response { get; private set; }

        public WaypointConflict(Waypoint original, Waypoint incoming)
        {
            InitializeComponent();
            Merged = new Waypoint(original.Location, original.Marker, original.Island);
            if (original.Name != String.Empty)
                Merged.Name = original.Name;
            else
                Merged.Name = incoming.Name;

            Merged.Notes = original.Notes;
            if (Merged.Notes != String.Empty && incoming.Notes != String.Empty)
                Merged.Notes += "; ";
            Merged.Notes += incoming.Notes;

            Original = original;
            Incoming = incoming;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.pgOriginal.SelectedObject = Original;
            this.pgNew.SelectedObject = Incoming;
            this.pgMerged.SelectedObject = Merged;
            btnOK.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Response = ConflictResponse.Merged;
            this.ContinueWithRemaining = chkContinue.Checked;
            this.Close();
        }

        private void btnUseOriginal_Click(object sender, EventArgs e)
        {
            this.Response = ConflictResponse.Original;
            this.Close();
        }

        private void chkContinue_CheckedChanged(object sender, EventArgs e)
        {
            this.ContinueWithRemaining = chkContinue.Checked;
        }

        private void btnUseNew_Click(object sender, EventArgs e)
        {
            this.Response = ConflictResponse.New;
            this.Close();
        }
    }
}
