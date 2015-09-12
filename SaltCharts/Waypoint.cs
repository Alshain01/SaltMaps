using System;
using System.ComponentModel;
using System.Drawing;

namespace SaltCharts
{
    [Serializable]
    public class Waypoint : IMapItem, IEquatable<Waypoint>, IComparable<Waypoint>
    {
        const int CELL_SIZE = 40;
        const int GRID_CENTER = 2068;

        public string Name { get; set; }
        public MarkerType Marker { get; set; }
        public IslandType Island { get; set; }
        public string Notes { get; set; }

        [Browsable(false)]
        public Coordinates Location { get; set; }

        // Constructor for deserialization
        public Waypoint()
        {
            this.Name = String.Empty;
            this.Notes = String.Empty;
        }

        // Constructor for new Waypoint Generation
        public Waypoint(Point p, MarkerType marker, IslandType island)
        {
            this.Location = Coordinates.FromPoint(p);
            this.Island = island;
            this.Marker = marker;
            this.Name = string.Empty;
            this.Notes = String.Empty;
       }

        public Point GetLocation()
        {
            return Location.ToPoint();
        }

        public Image GetImage()
        {
            Image waypointImage = SaltCharts.Properties.Resources.Cell;
            Graphics graphic = Graphics.FromImage(waypointImage);

            if (Island != IslandType.None) 
                graphic.DrawImageUnscaled((Image)SaltCharts.Properties.Resources.ResourceManager.GetObject(Island.ToString()), 0, 0);

            if (Marker != MarkerType.None) {
                Image markerImg = (Image)SaltCharts.Properties.Resources.ResourceManager.GetObject(Marker.ToString());
                switch (Island)
                {
                    case IslandType.None:
                        graphic.DrawImageUnscaled(markerImg, 0, 0);
                        break;
                    case IslandType.Single:
                        graphic.DrawImage(markerImg, 7, 7, 25, 25);
                        break;
                    case IslandType.NorthWest:
                        graphic.DrawImage(markerImg, 10, 10, 25, 25);
                        break;
                    case IslandType.NorthEast:
                        graphic.DrawImage(markerImg, 5, 10, 25, 25);
                        break;
                    case IslandType.SouthWest:
                        graphic.DrawImage(markerImg, 10, 5, 25, 25);
                        break;
                    case IslandType.SouthEast:
                        graphic.DrawImage(markerImg, 5, 5, 25, 25);
                        break;
                    case IslandType.DeepSea:
                        graphic.DrawImage(markerImg, 7, 0, 25, 25);
                        break;
                }
            }
            return waypointImage;
        }

        public int CompareTo(Waypoint wp) 
        {
            return Location.CompareTo(wp.Location);
        }

        public override bool Equals(Object o)
        {
            if (o.GetType().Equals(this.GetType()))
                return Equals((Waypoint)o);
            return false;
        }

        public bool Equals(Waypoint wp)
        {
            return Location.Equals(wp.Location);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator >(Waypoint a, Waypoint b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(Waypoint a, Waypoint b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >=(Waypoint a, Waypoint b)
        {
            int result = a.CompareTo(b);
            return result == 0 || result > 0;
        }

        public static bool operator <=(Waypoint a, Waypoint b)
        {
            int result = a.CompareTo(b);
            return result == 0 || result < 0;
        }

        public static bool operator ==(Waypoint a, Waypoint b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Waypoint a, Waypoint b)
        {
            return !a.Equals(b);
        }
    }
}
