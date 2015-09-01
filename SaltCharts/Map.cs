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
     public partial class Map : Form
    {
        private const string MAP_EXTENSION = ".map";
        private const string DEFAULT_SEED = "Default";

        private List<Waypoint> waypoints;
        private string mapFilepath = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + DEFAULT_SEED + MAP_EXTENSION;
        private Point mouseDownLoc;
        private Point mapBoxLoc;
        //private Point rightMouseDownLoc;
        private List<PictureBox> pictureBoxes = new List<PictureBox>();

        public Map()
        {
            InitializeComponent();
            LoadMapFile();
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
            foreach (var p in pictureBoxes)
                p.Dispose();
        }

        /*
         * Plots all stored map points on the map. 
         */
        private void PlotMapPoints()
        {
            foreach (var mp in waypoints)
                AddWaypointToMap(mp);
        }

        /*
         * Place the center of the map image in the center of the map panel.
         */
        private void CenterMap()
        {
            SeaChart.Location = new Point(-(SeaChart.Image.Width / 2 - panelChart.Width / 2), -(SeaChart.Image.Height / 2 - panelChart.Height / 2));
        }

        private void LoadMapFile()
        {
            var serializer = new YamlSerializer();
            mapFilepath = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + SaltCharts.Properties.Settings.Default.LastMapFile + MAP_EXTENSION;

            if (File.Exists(mapFilepath))
            {
                var obj = serializer.DeserializeFromFile(mapFilepath);
                waypoints = (List<Waypoint>)obj[0];
            }
            else
                waypoints = new List<Waypoint>();


            this.Text = new StringBuilder("Salt Charts v").Append(Application.ProductVersion).Append(" - ").Append(SaltCharts.Properties.Settings.Default.LastMapFile).ToString();
        }

        private void SaveMapFile()
        {
            var serializer = new YamlSerializer();
            serializer.SerializeToFile(mapFilepath, waypoints);
        }

        public void EnableSaveButton()
        {
            if (SaltCharts.Properties.Settings.Default.AutoSave)
                SaveMapFile();
            else
                btnSave.Enabled = true;
        }

        private void AddWaypointToMap(Waypoint wp)
        {
            // Draw the waypoint image over the map.
            var wpImg = new PictureBox();
            // Properties
            wpImg.SizeMode = PictureBoxSizeMode.AutoSize;
            wpImg.Parent = SeaChart;
            wpImg.BackColor = Color.Transparent;
            wpImg.Location = wp.getLocation();
            wpImg.Image = wp.getImage();
            wpImg.BringToFront();
            wpImg.Tag = wp;

            //Events
            wpImg.MouseUp += Waypoint_MouseUp;
            wpImg.MouseEnter += Waypoint_MouseEnter;
            wpImg.MouseLeave += Waypoint_MouseLeave;
            wpImg.MouseDown += Waypoint_MouseDown;
            wpImg.MouseMove += Waypoint_MouseMove;

            //Add to storage
            pictureBoxes.Add(wpImg);
        }

        private void Map_Load(object sender, EventArgs e)
        {
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
                this.Cursor = Cursors.Hand;
            else if (e.Button == MouseButtons.Right)
                addWaypoint(e.Location);
        }

        private void SeaChart_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void SeaChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DragMap(e.Location);
            }
            else if (e.Button == MouseButtons.None)
            {
                statusCoord.Text = Waypoint.getFormatted(e.Location);

                // Debug information
                statusPoint.Text = string.Format("Point: {0},{1}", e.Location.X, e.Location.Y);
                statusRawCoord.Text = string.Format("Raw Coordinate: {0},{1}", Waypoint.getCoordinate(e.Location.X), Waypoint.getCoordinate(e.Location.Y));
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

        /*
         * Event Handler for all context menu clicks (except delete)
         */
        private void addWaypoint(Point location)
        {
            IslandType island = getActiveIsland();
            MarkerType marker = getActiveMarker();

            if (island == IslandType.None && marker == MarkerType.None)
                return;

            Waypoint wp = new Waypoint(location, marker, island);
            waypoints.Add(wp);
            AddWaypointToMap(wp);
            if (SaltCharts.Properties.Settings.Default.AutoSave)
                SaveMapFile();
            else
                btnSave.Enabled = true;
        }

         private IslandType getActiveIsland()
         {
            IslandType island = IslandType.None;
            foreach (RadioButton b in panelIsland.Controls)
                if (b.Checked)
                {
                    island = (IslandType)b.Tag;
                    break;
                }
            return island;
         }

         private MarkerType getActiveMarker()
         {
             MarkerType marker = MarkerType.None;
             foreach (RadioButton b in markerPanel.Controls)
                 if (b.Checked)
                 {
                     marker = (MarkerType)b.Tag;
                     break;
                 }
             return marker;
         }

         private void deleteWaypoint(PictureBox waypointPicBox)
        {
            Waypoint wp = (Waypoint)waypointPicBox.Tag;
            if(!String.IsNullOrEmpty(wp.Name) || !String.IsNullOrEmpty(wp.Notes))
                if(MessageBox.Show("This waypoint has notes. Are you sure you want to delete it?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                    return;

            waypoints.Remove(wp);
            waypointPicBox.Dispose();
            
            if (SaltCharts.Properties.Settings.Default.AutoSave)
                SaveMapFile();
            else
                btnSave.Enabled = true;
        }

        void Waypoint_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownLoc = e.Location;
            mapBoxLoc = ((PictureBox)sender).Parent.Location;
            if (e.Button == MouseButtons.Left)
                this.Cursor = Cursors.Hand;
        }

        private void Waypoint_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DragMap(e.Location);
            }
        }

        private void Waypoint_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mapBoxLoc.Equals(((PictureBox)sender).Parent.Location))
            {
                PictureBox pb = (PictureBox)sender;
                Waypoint mp = (Waypoint)pb.Tag;
                var frm = new PoiDetails(mp);
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
                frm.ShowDialog(this);
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (btnNoIsland.Checked && btnNoMarker.Checked)
                    deleteWaypoint((PictureBox)sender);
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
                        pb.Image = wp.getImage();
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

            mapFilepath = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + mapName + MAP_EXTENSION;
            SaltCharts.Properties.Settings.Default.LastMapFile = mapName;

            this.Text = new StringBuilder("Salt Charts v").Append(Application.ProductVersion).Append(" - ").Append(mapName).ToString();
            btnSave.Enabled = false;
            waypoints = new List<Waypoint>();

            //clear all the map points
            ClearMapPoints();
        }

        private void btnCenter_Click(object sender, EventArgs e)
        {
            CenterMap();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveMapFile();
            btnSave.Enabled = false;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ClearMapPoints();
                SaltCharts.Properties.Settings.Default.LastMapFile = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                LoadMapFile();
                PlotMapPoints();

                this.Text = "Salt Charts - " + SaltCharts.Properties.Settings.Default.LastMapFile;
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
            if (btnSave.Enabled && auto)
            {
                // Trigger and immediate autosave if necessary.
                SaveMapFile();
                btnSave.Enabled = false;
            }
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
            {
                // Build the Image
                Bitmap bmp = new Bitmap(SaltCharts.Properties.Resources.Grid);
                Graphics g = Graphics.FromImage(bmp);
                //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                foreach (Waypoint mp in waypoints)
                    g.DrawImageUnscaled(mp.getImage(), mp.getLocation().X, mp.getLocation().Y);

                // Find the format
                ImageFormat format;
                string[] split = imageSaveDialog.FileName.Split('.');
                string extension;
                if (split.GetLength(0) < 2)
                    extension = ".err";
                else
                    extension = split[1];

                switch (extension.ToLower())
                {
                    case "bmp":
                        format = ImageFormat.Bmp;
                        break;
                    case "png":
                        format = ImageFormat.Png;
                        break;
                    case "jpg":
                    case "jpeg":
                        format = ImageFormat.Jpeg;
                        break;
                    case "gif":
                        format = ImageFormat.Gif;
                        break;
                    case "tiff":
                    case "tif":
                        format = ImageFormat.Tiff;
                        break;
                    default:
                        MessageBox.Show("Unable to determine a valid image type.  Please use a proper file extension.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                bmp.Save(imageSaveDialog.FileName, format);
            }

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

        private void horizontalNavigation_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
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

        private void verticalNavigation_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void verticalNavigation_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DragMap(new Point(mouseDownLoc.X, e.Location.Y));
            }
        }
    }
}
