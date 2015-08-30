using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Yaml.Serialization;
using Microsoft.VisualBasic;

namespace SaltCharts
{
    public enum POIType
    {
        UninhabitedIsland,
        DesertIsland,
        HighMountainIsland,
        PirateCampIsland,
        PirateTownshipIsland,
        AncientRuinsIsland,
        AncientAltarIsland,
        MerchantIsland,
        HuntingCampIsland,
        InnkeeperIsland,
        MarkerPirateShip,
        MarkerMoonstones,
        MarkerGoodResources,
        MarkerBronzeChest,
        MarkerSilverChest,
        MarkerCompass,
        MarkerX,
        MarkerSpiderQueen,
        MarkerQuestion
    }

    public enum POISubType
    {
        None,
        Single,
        NorthWest,
        NorthEast,
        SouthWest,
        SouthEast
    }

     public partial class Map : Form
    {
        private const string MAP_EXTENSION = ".map";
        private const string SETTINGS_FILE = "Settings.cfg";
        private const string DEFAULT_SEED = "Default";

        private List<MapPoint> mapPoints;
        private Config _config;
        private string configFilepath = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + DEFAULT_SEED + MAP_EXTENSION;
        private string _settingsFile = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + SETTINGS_FILE;
        private Point mouseDownLoc;
        private Point rightMouseDownLoc;
        private List<PictureBox> pictureBoxes;

        public Map()
        {
            InitializeComponent();
            CenterMap();
            LoadConfigFile();
            PlotMapPoints();
            DelegateEvents();
            btnSave.Enabled = false;
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
            foreach (var mp in mapPoints)
                AddPOIToMap(mp);
        }

        private void SeaChart_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownLoc = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                this.Cursor = Cursors.Hand;
            }
            else if(e.Button == MouseButtons.Right)
            {
                rightMouseDownLoc = e.Location;
            }

        }

        private void SeaChart_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void SeaChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point currentMousePos = e.Location;
                int distanceX = currentMousePos.X - mouseDownLoc.X;
                int distanceY = currentMousePos.Y - mouseDownLoc.Y;
                int newX = SeaChart.Location.X + distanceX;
                int newY = SeaChart.Location.Y + distanceY;

                if (newX + SeaChart.Image.Width < SeaChart.Image.Width && SeaChart.Image.Width + newX > MapPanel.Width)
                    SeaChart.Location = new Point(newX, SeaChart.Location.Y);
                if (newY + SeaChart.Image.Height < SeaChart.Image.Height && SeaChart.Image.Height + newY > MapPanel.Height)
                    SeaChart.Location = new Point(SeaChart.Location.X, newY);
            }
            else if (e.Button == MouseButtons.None)
            {
                statusCoord.Text = MapPoint.getFormatted(e.Location);
                statusPoint.Text = string.Format("Point: {0},{1}", e.Location.X, e.Location.Y);
                statusRawCoord.Text = string.Format("Raw Coordinate: {0},{1}", MapPoint.getCoordinate(e.Location.X), MapPoint.getCoordinate(e.Location.Y));
                statusChartLocation.Text = string.Format("Chart Location: {0}, {1}", SeaChart.Location.X, SeaChart.Location.Y);
            }
        }


        private void btnCenterMap_Click(object sender, EventArgs e)
        {
            CenterMap();
        }

        private void CenterMap()
        {
            SeaChart.Location = new Point(-(SeaChart.Image.Width / 2 - MapPanel.Width / 2), -(SeaChart.Image.Height / 2 - MapPanel.Height / 2));
            //SeaChart.Location = new Point(-1434, -1769);
        }

        private void AddPOIToMap(MapPoint mp)
        {
            
            var newPic = new PictureBox();
            newPic.SizeMode = PictureBoxSizeMode.AutoSize;
            newPic.Parent = SeaChart;
            newPic.BackColor = Color.Transparent;
            
            newPic.Location = mp.getPosition();

            string imageName = (mp.PoiSubType != POISubType.None && mp.PoiSubType != POISubType.Single) 
                ? mp.PoiType.ToString() + mp.PoiSubType.ToString() : mp.PoiType.ToString();

            newPic.Image = (Image)SaltCharts.Properties.Resources.ResourceManager.GetObject(imageName);
            newPic.BringToFront();
            newPic.Tag = mp;
            newPic.ContextMenuStrip = mnuWaypointRightClick;
            newPic.Click += newPic_Click;
            newPic.MouseEnter += newPic_MouseEnter;
            newPic.MouseDown += newPic_MouseDown;
            if (pictureBoxes == null)
                pictureBoxes = new List<PictureBox>();

            pictureBoxes.Add(newPic);
        }

        void newPic_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownLoc = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                this.Cursor = Cursors.Hand;
            }
            else if (e.Button == MouseButtons.Right)
            {
                rightMouseDownLoc = e.Location;
            }
        }

        private void newPic_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            MapPoint mp = (MapPoint)pb.Tag;
            var frm = new PoiDetails(mp);
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
            frm.ShowDialog(this);
        }

        private void newPic_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pix = ((PictureBox)sender);
            MapPoint mp = (MapPoint)pix.Tag;
            statusCoord.Text = mp.ToString();
            toolTip1.SetToolTip(pix, mp.Notes);
        }

        private void LoadConfigFile()
        {
            
            var serializer = new YamlSerializer();
            if (File.Exists(_settingsFile))
            {
                var obj = serializer.DeserializeFromFile(Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + SETTINGS_FILE);
                _config = (Config)obj[0];
                configFilepath = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + _config.LastMapFile + MAP_EXTENSION;
            }
            else
            {
                _config = new Config();
                _config.LastMapFile = DEFAULT_SEED;
            }

            
            if (File.Exists(configFilepath))
            {
                var obj = serializer.DeserializeFromFile(configFilepath);
                mapPoints = (List<MapPoint>)obj[0];
            }
            else
                mapPoints = new List<MapPoint>();


            this.Text = new StringBuilder("Salt Charts v").Append(Application.ProductVersion).Append(" - ").Append(_config.LastMapFile).ToString();
        }

        private void SaveConfigfile()
        {
            var serializer = new YamlSerializer();
            serializer.SerializeToFile(configFilepath, mapPoints);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfigfile();
            btnSave.Enabled = false;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            string mapName = Interaction.InputBox("Map name? (This is usually the name of the seed you used when you created your world)", "New Map");
            if (string.IsNullOrEmpty(mapName))
            {
                MessageBox.Show("You must enter a name for the map.", "Map Name Required!", MessageBoxButtons.OK);
                return;
            }

            configFilepath = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + mapName + MAP_EXTENSION;
            _config.LastMapFile = mapName;

            this.Text = new StringBuilder("Salt Charts v").Append(Application.ProductVersion).Append(" - ").Append(_config.LastMapFile).ToString();
            btnSave.Enabled = false;
            mapPoints = new List<MapPoint>();

            //clear all the map points
            if (pictureBoxes != null)
            {
                foreach (var p in pictureBoxes)
                {
                    p.Dispose();
                }
            }
        }

        public void EnableSaveButton()
        {
            if (SaltCharts.Properties.Settings.Default.AutoSave)
                SaveConfigfile();
            else
                btnSave.Enabled = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            var serializer = new YamlSerializer();
            serializer.SerializeToFile(_settingsFile, _config);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ClearMapPoints();
                _config.LastMapFile = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                var serializer = new YamlSerializer();
                serializer.SerializeToFile(_settingsFile, _config);
                LoadConfigFile();
                PlotMapPoints();

                this.Text = "Salt Charts - " + _config.LastMapFile;
            }
        }

        private void deleteWaypointToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PictureBox waypointPB = null;
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    waypointPB = owner.SourceControl as PictureBox;
                }
            }

                        
            if (waypointPB != null)
            {
                var mp = (MapPoint)waypointPB.Tag;
                mapPoints.Remove(mp);
                waypointPB.Dispose();
                if (SaltCharts.Properties.Settings.Default.AutoSave)
                    SaveConfigfile();
                else
                    btnSave.Enabled = true;
            }
        }

        /*
         * Event Handler for all context menu clicks (except delete)
         */
        private void addPOI(object sender, EventArgs e, POIType type, POISubType subType)
        {
            MapPoint mp = new MapPoint(mouseDownLoc, type, subType);
            mapPoints.Add(mp);
            AddPOIToMap(mp);
            if (SaltCharts.Properties.Settings.Default.AutoSave)
                SaveConfigfile();
            else
                btnSave.Enabled = true;
        }
         
        /*
         * Define the click events with custom arguments for the context menu
         */
        private void DelegateEvents()
        {
            // Uninhabited Island
            this.uninhabitedIslandSingle.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.UninhabitedIsland, POISubType.Single); };
            this.uninhabitedIslandNorthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.UninhabitedIsland, POISubType.NorthWest); };
            this.uninhabitedIslandNorthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.UninhabitedIsland, POISubType.NorthEast); };
            this.uninhabitedIslandSouthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.UninhabitedIsland, POISubType.SouthWest); };
            this.uninhabitedIslandSouthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.UninhabitedIsland, POISubType.SouthEast); };

            // Desert Island
            this.desertIslandSingle.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.DesertIsland, POISubType.Single); };
            this.desertIslandNorthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.DesertIsland, POISubType.NorthWest); };
            this.desertIslandNorthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.DesertIsland, POISubType.NorthEast); };
            this.desertIslandSouthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.DesertIsland, POISubType.SouthWest); };
            this.desertIslandSouthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.DesertIsland, POISubType.SouthEast); };

            //High Mountain Island
            this.highMountainIslandSingle.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.HighMountainIsland, POISubType.Single); };
            this.highMountainIslandNorthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.HighMountainIsland, POISubType.NorthWest); };
            this.highMountainIslandNorthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.HighMountainIsland, POISubType.NorthEast); };
            this.highMountainIslandSouthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.HighMountainIsland, POISubType.SouthWest); };
            this.highMountainIslandSouthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.HighMountainIsland, POISubType.SouthEast); };

            // Pirate Camp Island
            this.pirateCampIslandSingle.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.PirateCampIsland, POISubType.Single); };
            this.pirateCampIslandNorthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.PirateCampIsland, POISubType.NorthWest); };
            this.pirateCampIslandNorthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.PirateCampIsland, POISubType.NorthEast); };
            this.pirateCampIslandSouthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.PirateCampIsland, POISubType.SouthWest); };
            this.pirateCampIslandSouthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.PirateCampIsland, POISubType.SouthEast); };

            // Pirate Township Island
            this.pirateTownshipIslandSingle.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.PirateTownshipIsland, POISubType.Single); };
            this.pirateTownshipIslandNorthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.PirateTownshipIsland, POISubType.NorthWest); };
            this.pirateTownshipIslandNorthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.PirateTownshipIsland, POISubType.NorthEast); };
            this.pirateTownshipIslandSouthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.PirateTownshipIsland, POISubType.SouthWest); };
            this.pirateTownshipIslandSouthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.PirateTownshipIsland, POISubType.SouthEast); };

            // Ancient Ruins Island
            this.ancientRuinsIslandSingle.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.AncientRuinsIsland, POISubType.Single); };
            this.ancientRuinsIslandNorthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.AncientRuinsIsland, POISubType.NorthWest); };
            this.ancientRuinsIslandNorthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.AncientRuinsIsland, POISubType.NorthEast); };
            this.ancientRuinsIslandSouthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.AncientRuinsIsland, POISubType.SouthWest); };
            this.ancientRuinsIslandSouthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.AncientRuinsIsland, POISubType.SouthEast); };

            // Ancient Ruins Island
            this.ancientAltarIslandSingle.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.AncientAltarIsland, POISubType.Single); };
            this.ancientAltarIslandNorthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.AncientAltarIsland, POISubType.NorthWest); };
            this.ancientAltarIslandNorthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.AncientAltarIsland, POISubType.NorthEast); };
            this.ancientAltarIslandSouthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.AncientAltarIsland, POISubType.SouthWest); };
            this.ancientAltarIslandSouthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.AncientAltarIsland, POISubType.SouthEast); };

            // Merchant Island
            this.merchantIslandSingle.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MerchantIsland, POISubType.Single); };
            this.merchantIslandNorthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MerchantIsland, POISubType.NorthWest); };
            this.merchantIslandNorthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MerchantIsland, POISubType.NorthEast); };
            this.merchantIslandSouthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MerchantIsland, POISubType.SouthWest); };
            this.merchantIslandSouthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MerchantIsland, POISubType.SouthEast); };

            // Hunting Camp
            this.huntingCampIslandSingle.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.HuntingCampIsland, POISubType.Single); };
            this.huntingCampIslandNorthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.HuntingCampIsland, POISubType.NorthWest); };
            this.huntingCampIslandNorthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.HuntingCampIsland, POISubType.NorthEast); };
            this.huntingCampIslandSouthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.HuntingCampIsland, POISubType.SouthWest); };
            this.huntingCampIslandSouthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.HuntingCampIsland, POISubType.SouthEast); };

            //Innkeeper
            this.innkeeperIslandSingle.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.InnkeeperIsland, POISubType.Single); };
            this.innkeeperIslandNorthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.InnkeeperIsland, POISubType.NorthWest); };
            this.innkeeperIslandNorthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.InnkeeperIsland, POISubType.NorthEast); };
            this.innkeeperIslandSouthWest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.InnkeeperIsland, POISubType.SouthWest); };
            this.innkeeperIslandSouthEast.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.InnkeeperIsland, POISubType.SouthEast); };

            //Markers
            this.markerPirateShip.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MarkerPirateShip, POISubType.None); };
            this.markerMoonstones.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MarkerMoonstones, POISubType.None); };
            this.markerGoodResources.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MarkerGoodResources, POISubType.None); };
            this.markerBronzeChest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MarkerBronzeChest, POISubType.None); };
            this.markerSilverChest.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MarkerSilverChest, POISubType.None); };
            this.markerCompass.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MarkerCompass, POISubType.None); };
            this.markerSpiderQueen.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MarkerSpiderQueen, POISubType.None); };
            this.markerX.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MarkerX, POISubType.None); };
            this.markerQuestion.Click += delegate(object sender, System.EventArgs e) { this.addPOI(sender, e, POIType.MarkerQuestion, POISubType.None); };
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            var about = new AboutBox();
            about.ShowDialog(this);
        }

        private void Map_Load(object sender, EventArgs e)
        {
            btnSave.Visible = !SaltCharts.Properties.Settings.Default.AutoSave;
            setDebug();
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            SeaChart.Location = new Point(Math.Min(SeaChart.Location.X + (MapPanel.Width / 2), -1), SeaChart.Location.Y);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            SeaChart.Location = new Point(Math.Max(SeaChart.Location.X - (MapPanel.Width / 2), -(SeaChart.Image.Width - MapPanel.Width)), SeaChart.Location.Y);
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            SeaChart.Location = new Point(SeaChart.Location.X, Math.Min(SeaChart.Location.Y + (MapPanel.Height / 2), -1));
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            SeaChart.Location = new Point(SeaChart.Location.X, Math.Max(SeaChart.Location.Y - (MapPanel.Height / 2), -(SeaChart.Image.Height - MapPanel.Height)));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog(this);
            btnSave.Visible = !SaltCharts.Properties.Settings.Default.AutoSave;
            setDebug();
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
    }
}
