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
    public partial class WaypointConflict : Form
    {
        public Waypoint Merged { get; private set;  }
        private Waypoint Incoming;
        private Waypoint Original;

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
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
