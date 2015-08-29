using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltCharts
{
    [Serializable]
    public class MapPoint
    {

        public MapPoint()
        {
            X = 0;
            Y = 0;
            IsX_East = true;
            IsY_South = true;
            this.Name = string.Empty;
        }

        public MapPoint(int x, int y, bool IsXEast, bool IsYSouth, string name, POIType type, string notes)
        {
            X = x;
            Y = y;
            this.IsX_East = IsXEast;
            this.IsY_South = IsYSouth;
            this.Name = name;
            this.PoiType = type;
            this.Notes = notes;
        }

        public string Name { get; set; }

        public POIType PoiType { get; set; }

        public POISubType PoiSubType { get; set; }

        public string Notes { get; set; }

        [Browsable(false)]
        public int X { get; set; }

        [Browsable(false)]
        public int Y { get; set; }

        [Browsable(false)]
        public bool IsX_East { get; set; }

        [Browsable(false)]
        public bool IsY_South { get; set; }

        public override string ToString()
        {
            string template = "{0} {1}, {2} {3}";

            return string.Format(template, X.ToString(), (IsX_East ? "East" : "West"), Y.ToString(), (IsY_South ? "South" : "North"));
        }
    }
}
