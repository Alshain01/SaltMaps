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
            this.x = getCoordinate(p.X);
            this.y = getCoordinate(p.Y);
            this.Name = string.Empty;
            this.Notes = string.Empty;
            this.PoiType = POIType.MarkerX;
            this.PoiSubType = POISubType.None;
        }

        // Constructor for new POI Generation
        public MapPoint(Point p, POIType type, POISubType subType)
        {
            this.x = getCoordinate(p.X);
            this.y = getCoordinate(p.Y);
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
            if (y >= 0) yPos++; // There is a 1 pixel inconsistency in the grid image along the 0 x-axis.

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

        public override string ToString()
        {
            return getFormatted(this.x, this.y);
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

        private static int getCoordinate(int pixel) {
             int coordinate = pixel - GRID_CENTER;
             if (coordinate != 0)
                 coordinate = (coordinate + Math.Abs(coordinate) / coordinate * (CELL_SIZE / 2)) / CELL_SIZE;

             return coordinate;
        }
    }
}
