using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Yaml.Serialization;
using Microsoft.VisualBasic;
using SaltCharts.Properties;

namespace SaltCharts
{
    public partial class Form1 : Form
    {
        private List<MapPoint> mapPoints;
        private Config _config;
        private string configFilepath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Default.cfg";
        private string _settingsFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\Settings.cfg";
        public Form1()
        {
            InitializeComponent();

            CenterMap();

            LoadConfigFile();

            PlotMapPoints();

            DelegateEvents();

            btnSave.Enabled = false;
        }

        private void ClearMapPoints()
        {
            //clear all the map points
            foreach (var p in pictureBoxes)
            {
                p.Dispose();
            }
        }
        private void PlotMapPoints()
        {
            foreach (var mp in mapPoints)
            {
                AddPOIToMap(mp);
            }
        }

        Point mouseDownLoc;
        Point rightMouseDownLoc;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
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

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point currentMousePos = e.Location;
                int distanceX = currentMousePos.X - mouseDownLoc.X;
                int distanceY = currentMousePos.Y - mouseDownLoc.Y;
                int newX = pictureBox1.Location.X + distanceX;
                int newY = pictureBox1.Location.Y + distanceY;

                if (newX + pictureBox1.Image.Width < pictureBox1.Image.Width && pictureBox1.Image.Width + newX > panel1.Width)
                    pictureBox1.Location = new Point(newX, pictureBox1.Location.Y);
                if (newY + pictureBox1.Image.Height < pictureBox1.Image.Height && pictureBox1.Image.Height + newY > panel1.Height)
                    pictureBox1.Location = new Point(pictureBox1.Location.X, newY);
            }
            else if (e.Button == MouseButtons.None)
            {
                statusCoord.Text = string.Format("{0}, {1}", e.Location.X.ToString(), e.Location.Y.ToString());
                statusCoord.Text = CalulateMapPosition(e.Location.X, e.Location.Y).ToString();

                
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }


        private MapPoint CalulateMapPosition(int x, int y)
        {
            var mp = new MapPoint();

            //calc east/west
            int xVal = 0;
            if (x < 2066)
            {
                mp.IsX_East = false;
                int val1 = 2066 - x - 20;
                if (val1 < 0)
                    val1 = 0;
                else
                    xVal = val1 / 40 + 1;
            }
            else
            {
                int val1 = x - 2066 - 20;
                if (val1 < 0)
                    val1 = 0;
                else
                    xVal = val1 / 40 + 1;
            }


            //calc north south
            int yVal = 0;
            if (y < 2066)
            {
                mp.IsY_South = false;
                int val1 = 2066 - y - 20;
                if (val1 < 0)
                    val1 = 0;
                else
                    yVal = val1 / 40 + 1;
            }
            else
            {
                int val1 = y - 2066 - 20;
                if (val1 < 0)
                    val1 = 0;
                else
                    yVal = val1 / 40 + 1;
            }
            mp.X = xVal;
            mp.Y = yVal;
            return mp;
        }

        private void btnCenterMap_Click(object sender, EventArgs e)
        {
            CenterMap();
        }

        private void CenterMap()
        {
            pictureBox1.Location = new Point(-1434, -1769);
        }

        private List<PictureBox> pictureBoxes;
        private void AddPOIToMap(MapPoint mp)
        {
            
            var newPic = new PictureBox();
            newPic.SizeMode = PictureBoxSizeMode.AutoSize;
            newPic.Parent = pictureBox1;
            newPic.BackColor = Color.Transparent;
            
            //calc the upper left corner of the map square
            int picX = 0;
            int picY = 0;

            if (!mp.IsX_East)
            {
                picX = 2066 - 20 - (40 * mp.X);
            }
            else
            {
                picX = 2066 - 20 + (40 * mp.X);
            }

            if (!mp.IsY_South)
            {
                picY = 2065 - 20 - (40 * mp.Y);
            }
            else
            {
                picY = 2066 - 20 + (40 * mp.Y);
            }

            newPic.Location = new Point(picX, picY + 3);

            string imageName = (mp.PoiSubType != POISubType.None && mp.PoiSubType != POISubType.Single) 
                ? mp.PoiType.ToString() + mp.PoiSubType.ToString() : mp.PoiType.ToString();

            newPic.Image = (Image)SaltCharts.Properties.Resources.ResourceManager.GetObject(imageName);
            newPic.BringToFront();
            newPic.Tag = mp;
            newPic.ContextMenuStrip = mnuWaypointRightClick;
            newPic.Click += newPic_Click;
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
            var frm = new frmPOIDetails(mp);
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
            frm.ShowDialog(this);
        }

        // Event Handler for all context menu clicks (except delete)
        private void addPOI(object sender, EventArgs e, POIType type, POISubType subType)
        {
            var mp = CalulateMapPosition(mouseDownLoc.X, mouseDownLoc.Y);
            mp.PoiType = type;
            mp.PoiSubType = subType;
            mapPoints.Add(mp);
            AddPOIToMap(mp);
            btnSave.Enabled = true;
        }

        private void LoadConfigFile()
        {
            
            var serializer = new YamlSerializer();
            if (File.Exists(_settingsFile))
            {
                var obj = serializer.DeserializeFromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\Settings.cfg");
                _config = (Config)obj[0];
                configFilepath = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + _config.LastMapFile + ".cfg";
            }
            else
            {
                _config = new Config();
                _config.LastMapFile = "Default";
            }

            
            if (File.Exists(configFilepath))
            {
                var obj = serializer.DeserializeFromFile(configFilepath);
                mapPoints = (List<MapPoint>)obj[0];
            }
            else
                mapPoints = new List<MapPoint>();


            this.Text = "Salt Charts - " + _config.LastMapFile;
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

        private void newSeedMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mapName = Interaction.InputBox("Map name? (This is usually the name of the seed you used when you created your world)", "Default");
            if(string.IsNullOrEmpty(mapName))
            {
                MessageBox.Show("You must enter a name for the map.", "Map Name Required!", MessageBoxButtons.OK);
                return;
            }

            configFilepath = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + mapName + ".cfg";
            _config.LastMapFile = mapName;

            this.Text = "Salt Charts - " + _config.LastMapFile;
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
            btnSave.Enabled = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            var serializer = new YamlSerializer();
            serializer.SerializeToFile(_settingsFile, _config);
        }

        private void openMapToolStripMenuItem_Click(object sender, EventArgs e)
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
                btnSave.Enabled = true;
            }
        }

        // Define the click events with custom arguments for the context menu
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
}
