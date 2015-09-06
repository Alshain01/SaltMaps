using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltCharts
{
    [Serializable]
    public class Waypoint
    {
        const int CELL_SIZE = 40;
        const int GRID_CENTER = 2068;

        public string Name { get; set; }
        public MarkerType Marker { get; set; }
        public IslandType Island { get; set; }
        public string Notes { get; set; }

        [Browsable(false)]
        public int X { get; set; }

        [Browsable(false)]
        public int Y { get; set; }

        public Waypoint()
        {
            this.Name = string.Empty;
        }

        // Constructor for new Waypoint Generation
        public Waypoint(Point p, MarkerType marker, IslandType island)
        {
            this.X = getCoordinate(p.X);
            this.Y = getCoordinate(p.Y);
            this.Marker = marker;
            this.Island = island;
            this.Name = string.Empty;
       }

        public Point getLocation()
        {
            int yPos = Y * CELL_SIZE + GRID_CENTER - CELL_SIZE / 2;
            // These lines correct for slight inconsitencies in the map.
            if (Y >= 0) yPos++; 
            if (Y >= 40) yPos++;

            return new Point (X * CELL_SIZE + GRID_CENTER - CELL_SIZE /2 -1, yPos);
        }

        public Image getImage()
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
                        graphic.DrawImage(markerImg, 0, 0, 40, 40);
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
                }
            }
            return waypointImage;
        }

        public override string ToString()
        {
            return getFormatted(this.X, this.Y);
        }

        public static string getFormatted(Point p)
        {
            return getFormatted(getCoordinate(p.X), getCoordinate(p.Y));
        }

        public static string getFormatted(int x, int y) 
        {
            string template = "{0} {1}, {2} {3}";

            return string.Format(template, Math.Abs(x).ToString(), (x >= 0 ? "East" : "West"), Math.Abs(y).ToString(), (y >= 0) ? "South" : "North");
        }

        public static int getCoordinate(int pixel) {
             int coordinate = pixel - GRID_CENTER;
             if (coordinate != 0)
                 coordinate = (coordinate + Math.Abs(coordinate) / coordinate * (CELL_SIZE / 2)) / CELL_SIZE;

             return coordinate;
        }
    }
}
