namespace SaltCharts
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.mnuRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pirateShipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.merchantToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.battleMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ancientRuinsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highMountainsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moonstonesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goodResourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bronzeChestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.silverChestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusCoord = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnCenterMap = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.imgListPOI = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSeedMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.mnuWaypointRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteWaypointToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.mnuRightClick.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.mnuWaypointRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 647);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ContextMenuStrip = this.mnuRightClick;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(4125, 4135);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // mnuRightClick
            // 
            this.mnuRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.pirateShipToolStripMenuItem,
            this.merchantToolStripMenuItem,
            this.battleMasterToolStripMenuItem,
            this.ancientRuinsToolStripMenuItem,
            this.highMountainsToolStripMenuItem,
            this.moonstonesToolStripMenuItem,
            this.goodResourcesToolStripMenuItem,
            this.bronzeChestToolStripMenuItem,
            this.silverChestToolStripMenuItem});
            this.mnuRightClick.Name = "mnuRightClick";
            this.mnuRightClick.Size = new System.Drawing.Size(162, 246);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem1.Text = "Island";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem2.Text = "Pirate Encounter";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // pirateShipToolStripMenuItem
            // 
            this.pirateShipToolStripMenuItem.Name = "pirateShipToolStripMenuItem";
            this.pirateShipToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.pirateShipToolStripMenuItem.Text = "Pirate Ship";
            this.pirateShipToolStripMenuItem.Click += new System.EventHandler(this.pirateShipToolStripMenuItem_Click);
            // 
            // merchantToolStripMenuItem
            // 
            this.merchantToolStripMenuItem.Name = "merchantToolStripMenuItem";
            this.merchantToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.merchantToolStripMenuItem.Text = "Merchant";
            this.merchantToolStripMenuItem.Click += new System.EventHandler(this.merchantToolStripMenuItem_Click);
            // 
            // battleMasterToolStripMenuItem
            // 
            this.battleMasterToolStripMenuItem.Name = "battleMasterToolStripMenuItem";
            this.battleMasterToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.battleMasterToolStripMenuItem.Text = "Battle Master";
            this.battleMasterToolStripMenuItem.Click += new System.EventHandler(this.battleMasterToolStripMenuItem_Click);
            // 
            // ancientRuinsToolStripMenuItem
            // 
            this.ancientRuinsToolStripMenuItem.Name = "ancientRuinsToolStripMenuItem";
            this.ancientRuinsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.ancientRuinsToolStripMenuItem.Text = "Ancient Ruins";
            this.ancientRuinsToolStripMenuItem.Click += new System.EventHandler(this.ancientRuinsToolStripMenuItem_Click);
            // 
            // highMountainsToolStripMenuItem
            // 
            this.highMountainsToolStripMenuItem.Name = "highMountainsToolStripMenuItem";
            this.highMountainsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.highMountainsToolStripMenuItem.Text = "High Mountains";
            this.highMountainsToolStripMenuItem.Click += new System.EventHandler(this.highMountainsToolStripMenuItem_Click);
            // 
            // moonstonesToolStripMenuItem
            // 
            this.moonstonesToolStripMenuItem.Name = "moonstonesToolStripMenuItem";
            this.moonstonesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.moonstonesToolStripMenuItem.Text = "Moonstones";
            this.moonstonesToolStripMenuItem.Click += new System.EventHandler(this.moonstonesToolStripMenuItem_Click);
            // 
            // goodResourcesToolStripMenuItem
            // 
            this.goodResourcesToolStripMenuItem.Name = "goodResourcesToolStripMenuItem";
            this.goodResourcesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.goodResourcesToolStripMenuItem.Text = "Good Resources";
            this.goodResourcesToolStripMenuItem.Click += new System.EventHandler(this.goodResourcesToolStripMenuItem_Click);
            // 
            // bronzeChestToolStripMenuItem
            // 
            this.bronzeChestToolStripMenuItem.Name = "bronzeChestToolStripMenuItem";
            this.bronzeChestToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.bronzeChestToolStripMenuItem.Text = "Bronze Chest";
            this.bronzeChestToolStripMenuItem.Click += new System.EventHandler(this.bronzeChestToolStripMenuItem_Click);
            // 
            // silverChestToolStripMenuItem
            // 
            this.silverChestToolStripMenuItem.Name = "silverChestToolStripMenuItem";
            this.silverChestToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.silverChestToolStripMenuItem.Text = "Silver Chest";
            this.silverChestToolStripMenuItem.Click += new System.EventHandler(this.silverChestToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusCoord});
            this.statusStrip1.Location = new System.Drawing.Point(0, 625);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1280, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusCoord
            // 
            this.statusCoord.Name = "statusCoord";
            this.statusCoord.Size = new System.Drawing.Size(88, 17);
            this.statusCoord.Text = "0 West, 0 South";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCenterMap,
            this.btnSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1280, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnCenterMap
            // 
            this.btnCenterMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCenterMap.Image = ((System.Drawing.Image)(resources.GetObject("btnCenterMap.Image")));
            this.btnCenterMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCenterMap.Name = "btnCenterMap";
            this.btnCenterMap.Size = new System.Drawing.Size(23, 22);
            this.btnCenterMap.Text = "Center Map";
            this.btnCenterMap.Click += new System.EventHandler(this.btnCenterMap_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "toolStripButton1";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // imgListPOI
            // 
            this.imgListPOI.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListPOI.ImageStream")));
            this.imgListPOI.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListPOI.Images.SetKeyName(0, "island.png");
            this.imgListPOI.Images.SetKeyName(1, "Pirate.png");
            this.imgListPOI.Images.SetKeyName(2, "PirateShip.png");
            this.imgListPOI.Images.SetKeyName(3, "merchant.png");
            this.imgListPOI.Images.SetKeyName(4, "Battlemasters.png");
            this.imgListPOI.Images.SetKeyName(5, "AncientRuins.png");
            this.imgListPOI.Images.SetKeyName(6, "HighMountains.png");
            this.imgListPOI.Images.SetKeyName(7, "Moonstones.png");
            this.imgListPOI.Images.SetKeyName(8, "GoodResources.png");
            this.imgListPOI.Images.SetKeyName(9, "BronzeChest.png");
            this.imgListPOI.Images.SetKeyName(10, "SilverChest.png");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1280, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSeedMapToolStripMenuItem,
            this.openMapToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newSeedMapToolStripMenuItem
            // 
            this.newSeedMapToolStripMenuItem.Name = "newSeedMapToolStripMenuItem";
            this.newSeedMapToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.newSeedMapToolStripMenuItem.Text = "New Map";
            this.newSeedMapToolStripMenuItem.Click += new System.EventHandler(this.newSeedMapToolStripMenuItem_Click);
            // 
            // openMapToolStripMenuItem
            // 
            this.openMapToolStripMenuItem.Name = "openMapToolStripMenuItem";
            this.openMapToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.openMapToolStripMenuItem.Text = "Open Map";
            this.openMapToolStripMenuItem.Click += new System.EventHandler(this.openMapToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "cfg";
            this.openFileDialog1.Filter = "Map File|*.cfg";
            this.openFileDialog1.Title = "Select Map File";
            // 
            // mnuWaypointRightClick
            // 
            this.mnuWaypointRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteWaypointToolStripMenuItem1});
            this.mnuWaypointRightClick.Name = "mnuWaypointRightClick";
            this.mnuWaypointRightClick.Size = new System.Drawing.Size(162, 26);
            // 
            // deleteWaypointToolStripMenuItem1
            // 
            this.deleteWaypointToolStripMenuItem1.Name = "deleteWaypointToolStripMenuItem1";
            this.deleteWaypointToolStripMenuItem1.Size = new System.Drawing.Size(161, 22);
            this.deleteWaypointToolStripMenuItem1.Text = "Delete Waypoint";
            this.deleteWaypointToolStripMenuItem1.Click += new System.EventHandler(this.deleteWaypointToolStripMenuItem1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 647);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Salt Charts";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.mnuRightClick.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mnuWaypointRightClick.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusCoord;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnCenterMap;
        private System.Windows.Forms.ImageList imgListPOI;
        private System.Windows.Forms.ContextMenuStrip mnuRightClick;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripMenuItem pirateShipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem merchantToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem battleMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ancientRuinsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highMountainsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moonstonesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goodResourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bronzeChestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem silverChestToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSeedMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMapToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip mnuWaypointRightClick;
        private System.Windows.Forms.ToolStripMenuItem deleteWaypointToolStripMenuItem1;

    }
}

