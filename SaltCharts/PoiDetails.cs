﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaltCharts
{
    public partial class PoiDetails : Form
    {
        private Waypoint _mapPoint;
        public PoiDetails(Waypoint mp)
        {
            InitializeComponent();

            _mapPoint = mp;
            propertyGrid1.SelectedObject = _mapPoint;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Map frm = (Map)this.Owner;
            frm.EnableSaveButton();
        }

        private void PoiDetails_Load(object sender, EventArgs e)
        {

        }
    }
}
