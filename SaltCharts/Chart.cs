using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Yaml.Serialization;
using Microsoft.VisualBasic;

namespace SaltCharts
{
     public partial class Chart : Form
    {
        private const string MAP_EXTENSION = ".map";

        private Map map;
        private Point mouseDownLoc;
        private Point mapBoxLoc;

        public Chart()
        {
            InitializeComponent();
            map = FileIO.Load(Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + SaltCharts.Properties.Settings.Default.LastMapFile + MAP_EXTENSION);
            PlotMapPoints();
            btnSave.Enabled = false;
        }

        private void setDebug(bool state)
        {
            // Set Debug Options On
            statusPoint.Visible = state;
            statusRawCoord.Visible = state;
            statusChartLocation.Visible = state;
        }

        private void setDebug()
        {
            setDebug(SaltCharts.Properties.Settings.Default.Debug);
            #if DEBUG // Override settings for compiler directive
                setDebug(true);
            #endif
        }

        /*
         * Clears all POIs from the map.
         */
        private void ClearMapPoints()
        {
            //clear all the map points
            foreach (PictureBox p in SeaChart.Controls)
                p.Dispose();
        }

        /*
         * Plots all stored map points on the map. 
         */
        private void PlotMapPoints()
        {
            foreach (var wp in map.GetWaypoints())
                AddWaypointToMap(wp);
            foreach (var s in map.GetStamps())
                AddStampToMap(s);
        }

        /*
         * Place the center of the map image in the center of the map panel.
         */
        private void CenterMap()
        {
            SeaChart.Location = new Point(-(SeaChart.Image.Width / 2 - panelChart.Width / 2), -(SeaChart.Image.Height / 2 - panelChart.Height / 2));
        }

        public void AutoSave()
        {
            if (SaltCharts.Properties.Settings.Default.AutoSave)
                FileIO.Save(Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + SaltCharts.Properties.Settings.Default.LastMapFile + MAP_EXTENSION, map);
            else
                btnSave.Enabled = true;
        }

        private void AddStampToMap(Stamp stamp)
        {
            // Draw the waypoint image over the map.
            var stampImg = new PictureBox();
            // Properties
            stampImg.SizeMode = PictureBoxSizeMode.AutoSize;
            stampImg.Parent = SeaChart;
            stampImg.BackColor = Color.Transparent;
            stampImg.Image = stamp.GetImage();
            //Stamps should be centered at the mouse click
            stampImg.Location = new Point(stamp.Location.X - stampImg.Size.Width / 2, stamp.Location.Y - stampImg.Size.Height / 2);
            stampImg.BringToFront();
            stampImg.Tag = stamp;

            //Events
            stampImg.MouseUp += Stamp_MouseUp;
            stampImg.MouseDown += Stamp_MouseDown;
            stampImg.MouseMove += IMapItem_MouseMove;
        }
        private void AddWaypointToMap(Waypoint wp)
        {
            // Draw the waypoint image over the map.
            var wpImg = new PictureBox();
            // Properties
            wpImg.SizeMode = PictureBoxSizeMode.AutoSize;
            wpImg.Parent = SeaChart;
            wpImg.BackColor = Color.Transparent;
            wpImg.Location = wp.GetLocation();
            wpImg.Image = wp.GetImage();
            wpImg.BringToFront();
            wpImg.Tag = wp;

            //Events
            wpImg.MouseUp += Waypoint_MouseUp;
            wpImg.MouseEnter += Waypoint_MouseEnter;
            wpImg.MouseLeave += Waypoint_MouseLeave;
            wpImg.MouseDown += Waypoint_MouseDown;
            wpImg.MouseMove += IMapItem_MouseMove;
        }

        private void Map_Load(object sender, EventArgs e)
        {
            this.Text = new StringBuilder("Salt Charts v").Append(Application.ProductVersion).Append(" - ").Append(SaltCharts.Properties.Settings.Default.LastMapFile).ToString();
            this.Size = SaltCharts.Properties.Settings.Default.FormSize;
            this.Location = SaltCharts.Properties.Settings.Default.FormLocation;
            this.WindowState = SaltCharts.Properties.Settings.Default.FormState;
            btnSave.Visible = !SaltCharts.Properties.Settings.Default.AutoSave;
            statusNotes.Text = String.Empty;
            statusName.Text = String.Empty;
            CenterMap();
            setDebug();
        }

        private void Map_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaltCharts.Properties.Settings.Default.FormState = this.WindowState;
            SaltCharts.Properties.Settings.Default.FormSize = this.Size;
            SaltCharts.Properties.Settings.Default.FormLocation = this.Location;
            SaltCharts.Properties.Settings.Default.Save();
        }

        private void SeaChart_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownLoc = e.Location;
            if (e.Button == MouseButtons.Left)
                this.Cursor = Cursors.NoMove2D;
            else if (e.Button == MouseButtons.Right)
                if (btnStamp.Checked)
                {
                    addStamp(e.Location);
                }
                else
                {
                    addWaypoint(e.Location);
                }
        }

        private void SeaChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DragMap(e.Location);
            }
            else if (e.Button == MouseButtons.None)
            {
                Coordinates coord = Coordinates.FromPoint(e.Location);
                statusCoord.Text = coord.ToString();

                // Debug information
                statusPoint.Text = string.Format("Point: {0},{1}", e.Location.X, e.Location.Y);
                statusRawCoord.Text = string.Format("Raw Coordinate: {0},{1}", coord.X, coord.Y);
                statusChartLocation.Text = string.Format("Chart Location: {0}, {1}", SeaChart.Location.X, SeaChart.Location.Y);
            }
        }

        private void DragMap(Point currentMousePos)
        {
                int distanceX = currentMousePos.X - mouseDownLoc.X;
                int distanceY = currentMousePos.Y - mouseDownLoc.Y;
                int newX = SeaChart.Location.X + distanceX;
                int newY = SeaChart.Location.Y + distanceY;

                if (newX + SeaChart.Image.Width < SeaChart.Image.Width && SeaChart.Image.Width + newX > panelChart.Width)
                    SeaChart.Location = new Point(newX, SeaChart.Location.Y);
                if (newY + SeaChart.Image.Height < SeaChart.Image.Height && SeaChart.Image.Height + newY > panelChart.Height)
                    SeaChart.Location = new Point(SeaChart.Location.X, newY);
        }

        private void addStamp(Point location)
        {
            MarkerType marker = getActiveMarker();
            
            if (marker == MarkerType.None)
                return;

            AddStampToMap(map.AddStamp(marker, (int)txtStampSize.Value, location));
            AutoSave();
        }

        /*
         * Event Handler for all context menu clicks (except delete)
         */
        private void addWaypoint(Point location)
        {
            IslandType island = getActiveIsland();
            MarkerType marker = getActiveMarker();

            if (island == IslandType.None && marker == MarkerType.None)
                return;

            AddWaypointToMap(map.AddWaypoint(island, marker, location));
            AutoSave();
        }

         private IslandType getActiveIsland()
         {
            Type eType = Type.GetType("SaltCharts.IslandType");
            IslandType island = IslandType.None;
            foreach (Control c in panelIsland.Controls)
                if (c.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)c).Checked)
                    {
                        island = (IslandType)Enum.Parse(eType, (String)c.Tag, true);
                        break;
                    }
                }
            return island;
         }

         private MarkerType getActiveMarker()
         {
             Type eType = Type.GetType("SaltCharts.MarkerType");
             MarkerType marker = MarkerType.None;
             foreach (Control c in markerPanel.Controls)
                 if (c.GetType() == typeof(RadioButton))
                 {
                     if (((RadioButton)c).Checked)
                     {
                         marker = (MarkerType)Enum.Parse(eType, (String)c.Tag, true);
                         break;
                     }
                 }
             return marker;
         }

        void Stamp_MouseDown(object sender, MouseEventArgs e)
         {
             mouseDownLoc = e.Location;
             if (e.Button == MouseButtons.Left)
                 this.Cursor = Cursors.NoMove2D;
         }

        void Waypoint_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownLoc = e.Location;
            mapBoxLoc = ((PictureBox)sender).Parent.Location;
            if (e.Button == MouseButtons.Left)
                this.Cursor = Cursors.Hand;
        }

        private void IMapItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Cursor = Cursors.NoMove2D;
                DragMap(e.Location);
            }
        }

        private void deleteStamp(PictureBox stamp)
        {
            if (btnNoIsland.Checked || btnStamp.Checked)
            {
                map.RemoveStamp(stamp.Location);
                stamp.Dispose();
                AutoSave();
            }
        }

        private void deleteWaypoint(PictureBox waypointPicBox)
        {
            if (map.HasWaypoint(waypointPicBox.Location))
            {
                Waypoint wp = map.GetWaypoint(waypointPicBox.Location);
                if (!String.IsNullOrEmpty(wp.Name) || !String.IsNullOrEmpty(wp.Notes))
                    if (MessageBox.Show("This waypoint has notes. Are you sure you want to delete it?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                        return;

                map.RemoveWaypoint(waypointPicBox.Location);
            }
            waypointPicBox.Dispose();
            AutoSave();
        }

        void Stamp_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            if (e.Button == MouseButtons.Right && btnNoMarker.Checked)
                deleteStamp((PictureBox)sender);
        }

        private void Waypoint_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            if (e.Button == MouseButtons.Left && mapBoxLoc.Equals(((PictureBox)sender).Parent.Location))
            {
                PictureBox pb = (PictureBox)sender;
                Waypoint mp = (Waypoint)pb.Tag;
                var frm = new WaypointDetails(mp);
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
                frm.ShowDialog(this);
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (btnNoIsland.Checked && btnNoMarker.Checked)
                    deleteWaypoint((PictureBox)sender);
                else if (btnStamp.Checked)
                {
                    Point wpLoc = ((PictureBox)sender).Location;
                    addStamp(new Point(wpLoc.X + e.Location.X, wpLoc.Y + e.Location.Y));
                }
                else
                {
                    IslandType island = getActiveIsland();
                    MarkerType marker = getActiveMarker();
                    PictureBox pb = (PictureBox)sender;
                    Waypoint wp = (Waypoint)pb.Tag;

                    if (wp.Island != island || wp.Marker != marker)
                    {
                        wp.Island = island;
                        wp.Marker = marker;
                        pb.Image = wp.GetImage();
                    }
                }
            }
        }

        private void Waypoint_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pix = ((PictureBox)sender);
            Waypoint mp = (Waypoint)pix.Tag;
            statusCoord.Text = mp.ToString();
            if(!String.IsNullOrEmpty(mp.Name))
                statusName.Text = "Name: " + mp.Name;
            if (!String.IsNullOrEmpty(mp.Notes))
            {
                statusNotes.Text = "Notes: " + mp.Notes;
                toolTip1.SetToolTip(pix, mp.Notes);
            }
        }

        private void Waypoint_MouseLeave(object sender, EventArgs e)
        {
            statusName.Text = string.Empty;
            statusNotes.Text = string.Empty;
        }
        #region Button Click Events
        private void btnNew_Click(object sender, EventArgs e)
        {
            string mapName = Interaction.InputBox("Map name? (This is usually the name of the seed you used when you created your world)", "New Map");
            if (string.IsNullOrEmpty(mapName))
            {
                MessageBox.Show("You must enter a name for the map.", "Map Name Required!", MessageBoxButtons.OK);
                return;
            }

            SaltCharts.Properties.Settings.Default.LastMapFile = mapName;

            this.Text = new StringBuilder("Salt Charts v").Append(Application.ProductVersion).Append(" - ").Append(mapName).ToString();
            btnSave.Enabled = false;
            map = new Map();

            //clear all the map points
            ClearMapPoints();
        }

        private void btnCenter_Click(object sender, EventArgs e)
        {
            CenterMap();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FileIO.Save(Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + SaltCharts.Properties.Settings.Default.LastMapFile + MAP_EXTENSION, map);
            btnSave.Enabled = false;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ClearMapPoints();
                SaltCharts.Properties.Settings.Default.LastMapFile = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                map = FileIO.Load(Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + SaltCharts.Properties.Settings.Default.LastMapFile + MAP_EXTENSION);
                PlotMapPoints();

                this.Text = new StringBuilder("Salt Charts v").Append(Application.ProductVersion).Append(" - ").Append(SaltCharts.Properties.Settings.Default.LastMapFile).ToString();
            }
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            SeaChart.Location = new Point(SeaChart.Location.X, Math.Min(SeaChart.Location.Y + (panelChart.Height / 2), -1));
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            SeaChart.Location = new Point(Math.Min(SeaChart.Location.X + (panelChart.Width / 2), -1), SeaChart.Location.Y);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            SeaChart.Location = new Point(Math.Max(SeaChart.Location.X - (panelChart.Width / 2), -(SeaChart.Image.Width - panelChart.Width)), SeaChart.Location.Y);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            SeaChart.Location = new Point(SeaChart.Location.X, Math.Max(SeaChart.Location.Y - (panelChart.Height / 2), -(SeaChart.Image.Height - panelChart.Height)));
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog(this);

            bool auto = SaltCharts.Properties.Settings.Default.AutoSave;
            // Trigger and immediate autosave if necessary.
            
            if (btnSave.Enabled)
                AutoSave();
            btnSave.Visible = !auto;
            setDebug();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            var about = new AboutBox();
            about.ShowDialog(this);
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            imageSaveDialog.FileName = SaltCharts.Properties.Settings.Default.LastMapFile + ".png";
            if (imageSaveDialog.ShowDialog() == DialogResult.OK)
                FileIO.SaveImage(imageSaveDialog.FileName, map);
        }
        #endregion

        private void SeaChart_LocationChanged(object sender, EventArgs e)
        {
            horizontalNavigation.Location = new Point(SeaChart.Location.X, horizontalNavigation.Location.Y);
            verticalNavigation.Location = new Point(verticalNavigation.Location.X, SeaChart.Location.Y);
        }

        private void horizontalNavigation_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownLoc = e.Location;
            if (e.Button == MouseButtons.Left)
                this.Cursor = Cursors.NoMoveHoriz;
        }

        private void horizontalNavigation_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DragMap(new Point(e.Location.X, mouseDownLoc.Y));
            }
        }

        private void verticalNavigation_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownLoc = e.Location;
            if (e.Button == MouseButtons.Left)
                this.Cursor = Cursors.NoMoveVert;
        }

        private void Navigation_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void verticalNavigation_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DragMap(new Point(mouseDownLoc.X, e.Location.Y));
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            new Help().Show(this);
        }
    }
}
