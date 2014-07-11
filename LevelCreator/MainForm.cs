﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator
{
    public partial class LevelCreator : Form
    {
        PropertyForm propertyForm = new PropertyForm();

        public LevelCreator()
        {
            InitializeComponent();
            propertyForm.Owner = this;
            propertyForm.Show();
        }

        private void 編集ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ファイルを開くStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                }
            }
        }
    }
}
