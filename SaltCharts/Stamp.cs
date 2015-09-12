using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaltCharts
{
    [Serializable]
    class Stamp : IMapItem
    {
        public Point Location { get; set; }
        public MarkerType Marker { get; set; }
        public int Size { get; set; }

        public Stamp() { }

        public Stamp(Point p, int size, MarkerType marker)
        {
            this.Size = size;
            this.Location = p;
            this.Marker = marker;
        }

        public Point GetLocation()
        {
            return Location;
        }

        public Image GetImage()
        {
           return (Image)(new Bitmap((Image)SaltCharts.Properties.Resources.ResourceManager.GetObject(Marker.ToString()), new Size(Size, Size)));
        }
    }
}
