﻿using System;
using System.Drawing;

namespace SaltCharts
{
    [Serializable]
    public class Coordinates
    {
        const int CELL_SIZE = 40;
        const int GRID_CENTER = 2068;

        public int X { get; set; }
        public int Y { get; set; }

        public Coordinates()
        {
            this.X = 0;
            this.Y = 0;
        }

        public Coordinates(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Coordinates FromPoint(Point p) 
        {
            return new Coordinates(GetCoordinate(p.X), GetCoordinate(p.Y));
        }

        public Point ToPoint()
        {
            int yPos = Y * CELL_SIZE + GRID_CENTER - CELL_SIZE / 2;
            // These lines correct for slight inconsitencies in the map.
            if (Y >= 0) yPos++;
            if (Y >= 40) yPos++;

            return new Point(X * CELL_SIZE + GRID_CENTER - CELL_SIZE / 2 - 1, yPos);
        }

        private static int GetCoordinate(int pixel)
        {
            int coordinate = pixel - GRID_CENTER;
            if (coordinate != 0)
                coordinate = (coordinate + Math.Abs(coordinate) / coordinate * (CELL_SIZE / 2)) / CELL_SIZE;

            return coordinate;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}, {2} {3}", Math.Abs(X).ToString(), (X >= 0 ? "East" : "West"), Math.Abs(Y).ToString(), (Y >= 0) ? "South" : "North");
        }
    }
}
