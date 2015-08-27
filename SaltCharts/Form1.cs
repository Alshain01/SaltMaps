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
                picY = 2066 - 20 - (40 * mp.Y);
            }
            else
            {
                picY = 2066 - 20 + (40 * mp.Y);
            }

            newPic.Location = new Point(picX, picY + 3);
            newPic.Image = imgListPOI.Images[(int)mp.PoiType];
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


        private void AddPOI(Point p, POIType type)
        {
            var mp = CalulateMapPosition(p.X, p.Y);
            mp.PoiType = type;
            mapPoints.Add(mp);
            AddPOIToMap(mp);
            btnSave.Enabled = true;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.Island);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.Pirate);
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

        private void pirateShipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.PirateShip);
        }

        private void merchantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.Merchant);
        }

        private void battleMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.BattleMaster);
        }

        private void ancientRuinsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.AncientRuins);
        }

        private void highMountainsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.HighMountains);
        }

        private void moonstonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.MoonStones);
        }

        private void goodResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.GoodResources);
        }

        private void bronzeChestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.BronzeChest);
        }

        private void silverChestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPOI(mouseDownLoc, POIType.SilverChest);
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
        
    }

    public enum POIType
    {
        Island,
        Pirate,
        PirateShip,
        Merchant,
        BattleMaster,
        AncientRuins,
        HighMountains,
        MoonStones,
        GoodResources,
        BronzeChest,
        SilverChest
    }
}
