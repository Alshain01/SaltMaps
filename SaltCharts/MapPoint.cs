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
    public class MapPoint
    {
        const int CELL_SIZE = 40;
        const int GRID_CENTER = 2022;

        public MapPoint()
        {
            this.Name = string.Empty;
        }

        // Constructor for grabbing mouse hover coordinates for display.
        public MapPoint(Point p)
        {
            this.getCoordinates(p.X, p.Y);
            this.Name = string.Empty;
            this.Notes = string.Empty;
            this.PoiType = POIType.MarkerX;
            this.PoiSubType = POISubType.None;
        }

        // Constructor for new POI Generation
        public MapPoint(Point p, POIType type, POISubType subType)
        {
            this.getCoordinates(p.X, p.Y);
            this.PoiType = type;
            this.PoiSubType = subType;
            this.Name = string.Empty;
       }

        private void getCoordinates(int xPix, int yPix)
        {
            //calc east/west
            int xCoordinate = xPix - GRID_CENTER;
            if (xCoordinate != 0)
                xCoordinate = (xCoordinate + Math.Abs(xCoordinate) / xCoordinate * (CELL_SIZE / 2)) / CELL_SIZE;

            //calc north south
            int yCoordinate = yPix - GRID_CENTER;
            if (yCoordinate != 0)
                yCoordinate = (yCoordinate + Math.Abs(yCoordinate) / yCoordinate * (CELL_SIZE / 2)) / CELL_SIZE;

            this.x = xCoordinate;
            this.y = yCoordinate;
        }

        public Point getPosition()
        {
            int yPos = y * CELL_SIZE + GRID_CENTER - CELL_SIZE / 2;
            if (isSouth()) yPos++; // There is a 1 pixel inconsistency in the grid image along the 0 x-axis.

            return new Point (x * CELL_SIZE + GRID_CENTER - CELL_SIZE /2, yPos);
        }

        public string Name { get; set; }

        public POIType PoiType { get; set; }

        public POISubType PoiSubType { get; set; }

        public string Notes { get; set; }

        [Browsable(false)]
        public int x { get; set; }

        [Browsable(false)]
        public int y { get; set; }

        public bool isSouth() { return this.y >= 0; }

        public bool isEast() { return this.x >= 0; }

        public override string ToString()
        {
            string template = "{0} {1}, {2} {3}";

            return string.Format(template, Math.Abs(x).ToString(), (this.isEast() ? "East" : "West"), Math.Abs(y).ToString(), (this.isSouth() ? "South" : "North"));
        }
    }
}
