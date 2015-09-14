namespace SaltCharts
{
    partial class WaypointConflict
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
            this.btnOK = new System.Windows.Forms.Button();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.pgOriginal = new System.Windows.Forms.PropertyGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpNew = new System.Windows.Forms.GroupBox();
            this.pgNew = new System.Windows.Forms.PropertyGrid();
            this.grpMerged = new System.Windows.Forms.GroupBox();
            this.pgMerged = new System.Windows.Forms.PropertyGrid();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.grpNew.SuspendLayout();
            this.grpMerged.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(496, 319);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pgOriginal
            // 
            this.pgOriginal.CanShowVisualStyleGlyphs = false;
            this.pgOriginal.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgOriginal.Enabled = false;
            this.pgOriginal.HelpVisible = false;
            this.pgOriginal.Location = new System.Drawing.Point(6, 19);
            this.pgOriginal.Name = "pgOriginal";
            this.pgOriginal.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgOriginal.Size = new System.Drawing.Size(260, 107);
            this.pgOriginal.TabIndex = 5;
            this.pgOriginal.ToolbarVisible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pgOriginal);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 132);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Original";
            // 
            // grpNew
            // 
            this.grpNew.Controls.Add(this.pgNew);
            this.grpNew.Location = new System.Drawing.Point(299, 12);
            this.grpNew.Name = "grpNew";
            this.grpNew.Size = new System.Drawing.Size(272, 132);
            this.grpNew.TabIndex = 7;
            this.grpNew.TabStop = false;
            this.grpNew.Text = "New";
            // 
            // pgNew
            // 
            this.pgNew.CanShowVisualStyleGlyphs = false;
            this.pgNew.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgNew.Enabled = false;
            this.pgNew.HelpVisible = false;
            this.pgNew.Location = new System.Drawing.Point(6, 19);
            this.pgNew.Name = "pgNew";
            this.pgNew.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgNew.Size = new System.Drawing.Size(260, 107);
            this.pgNew.TabIndex = 5;
            this.pgNew.ToolbarVisible = false;
            // 
            // grpMerged
            // 
            this.grpMerged.Controls.Add(this.pgMerged);
            this.grpMerged.Location = new System.Drawing.Point(12, 179);
            this.grpMerged.Name = "grpMerged";
            this.grpMerged.Size = new System.Drawing.Size(559, 132);
            this.grpMerged.TabIndex = 8;
            this.grpMerged.TabStop = false;
            this.grpMerged.Text = "Merged";
            // 
            // pgMerged
            // 
            this.pgMerged.CanShowVisualStyleGlyphs = false;
            this.pgMerged.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgMerged.HelpVisible = false;
            this.pgMerged.Location = new System.Drawing.Point(6, 19);
            this.pgMerged.Name = "pgMerged";
            this.pgMerged.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgMerged.Size = new System.Drawing.Size(547, 107);
            this.pgMerged.TabIndex = 5;
            this.pgMerged.ToolbarVisible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SaltCharts.Properties.Resources.down;
            this.pictureBox1.Location = new System.Drawing.Point(280, 150);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 354);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.grpMerged);
            this.Controls.Add(this.grpNew);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Waypoint Conflict";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.grpNew.ResumeLayout(false);
            this.grpMerged.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.PropertyGrid pgOriginal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpNew;
        private System.Windows.Forms.PropertyGrid pgNew;
        private System.Windows.Forms.GroupBox grpMerged;
        private System.Windows.Forms.PropertyGrid pgMerged;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}