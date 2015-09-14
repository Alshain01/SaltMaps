using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SaltCharts
{
    [Serializable]
    class Map
    {
        public List<Waypoint> waypoints = new List<Waypoint>();
        public List<Stamp> stamps = new List<Stamp>();
        
        public Waypoint AddWaypoint(IslandType i, MarkerType m, Point p)
        {
            if (i == IslandType.None && m == MarkerType.None || waypoints.Exists(x => x.GetLocation() == p))
                return null;

            Waypoint wp = new Waypoint(p, m, i);
            waypoints.Add(wp);
            return wp;
        }

        public bool AddWaypoint(Waypoint wp)
        {
            if (HasWaypoint(wp.Location))
                return false;
                
            waypoints.Add(wp);
            return true;
        
        }
        
        public Waypoint GetWaypoint(Point p)
        {
            return waypoints.Find(x => x.GetLocation().X == p.X && x.GetLocation().Y == p.Y);
        }

        public Waypoint GetWaypoint(Coordinates c)
        {
            return waypoints.Find(x => x.Location.X == c.X && x.Location.Y == c.Y);
        }

        public bool HasWaypoint(Point p)
        {
            return waypoints.Exists(x => x.GetLocation().X == p.X && x.GetLocation().Y == p.Y);
        }

        public bool HasWaypoint(Coordinates c)
        {
            return waypoints.Exists(x => x.Location.X == c.X && x.Location.Y == c.Y);
        }

        public void RemoveWaypoint(Point p)
        {
            if (HasWaypoint(p))
                waypoints.Remove(GetWaypoint(p));
        }

        public Stamp AddStamp(MarkerType m, int size, Point p)
        {
            if (m == MarkerType.None || HasStamp(p))
                return null;

            Stamp stamp = new Stamp(p, size, m);
            stamps.Add(stamp);
            return stamp;
        }

        public bool AddStamp(Stamp s)
        {
            if (HasStamp(s.Location))
                return false;

            stamps.Add(s);
            return true;

        }

        public Stamp GetStamp(Point p)
        {
            return stamps.Find(x => x.Location.X == p.X && x.Location.Y == p.Y);
        }

        public bool HasStamp(Point p)
        {
            return stamps.Exists(x => x.Location.X == p.X && x.Location.Y == p.Y);
        }

        public void RemoveStamp(Point p)
        {
            if(HasStamp(p))
                stamps.Remove(GetStamp(p));
        }

        public List<IMapItem> GetItems()
        {
            List<IMapItem> l = new List<IMapItem>();
            l.AddRange(waypoints);
            l.AddRange(stamps);
            return l;
        }

        public List<Waypoint> GetWaypoints()
        {
            return waypoints;
        }

        public List<Stamp> GetStamps() 
        {
            return stamps;
        }
    }
}
