namespace LevelCreator
{
    partial class PropertyForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.GroundTab = new System.Windows.Forms.TabPage();
            this.ItemTab = new System.Windows.Forms.TabPage();
            this.DecolationTab = new System.Windows.Forms.TabPage();
            this.EnemyTab = new System.Windows.Forms.TabPage();
            this.PlayerTab = new System.Windows.Forms.TabPage();
            this.StageTab = new System.Windows.Forms.TabPage();
            this.StartPoint = new System.Windows.Forms.Label();
            this.StartXAxis = new System.Windows.Forms.Label();
            this.StartYAxis = new System.Windows.Forms.Label();
            this.StartXAxisTextBox = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox2 = new System.Windows.Forms.MaskedTextBox();
            this.TerminatePoint = new System.Windows.Forms.Label();
            this.TerminatePointXAxis = new System.Windows.Forms.Label();
            this.TerminatePointYAxis = new System.Windows.Forms.Label();
            this.TerminatePointXAxisTextBox = new System.Windows.Forms.MaskedTextBox();
            this.TerminatePointYAxitTextBox = new System.Windows.Forms.MaskedTextBox();
            this.TypeId = new System.Windows.Forms.Label();
            this.CollisionType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ItemPositionXAxis = new System.Windows.Forms.MaskedTextBox();
            this.ItemPositionYAxis = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ItemType = new System.Windows.Forms.ComboBox();
            this.tabControl.SuspendLayout();
            this.GroundTab.SuspendLayout();
            this.ItemTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.GroundTab);
            this.tabControl.Controls.Add(this.ItemTab);
            this.tabControl.Controls.Add(this.DecolationTab);
            this.tabControl.Controls.Add(this.EnemyTab);
            this.tabControl.Controls.Add(this.PlayerTab);
            this.tabControl.Controls.Add(this.StageTab);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(360, 238);
            this.tabControl.TabIndex = 1;
            // 
            // GroundTab
            // 
            this.GroundTab.Controls.Add(this.CollisionType);
            this.GroundTab.Controls.Add(this.TypeId);
            this.GroundTab.Controls.Add(this.TerminatePointYAxitTextBox);
            this.GroundTab.Controls.Add(this.maskedTextBox2);
            this.GroundTab.Controls.Add(this.TerminatePointXAxisTextBox);
            this.GroundTab.Controls.Add(this.TerminatePointYAxis);
            this.GroundTab.Controls.Add(this.StartXAxisTextBox);
            this.GroundTab.Controls.Add(this.TerminatePointXAxis);
            this.GroundTab.Controls.Add(this.StartYAxis);
            this.GroundTab.Controls.Add(this.StartXAxis);
            this.GroundTab.Controls.Add(this.TerminatePoint);
            this.GroundTab.Controls.Add(this.StartPoint);
            this.GroundTab.Location = new System.Drawing.Point(4, 22);
            this.GroundTab.Name = "GroundTab";
            this.GroundTab.Padding = new System.Windows.Forms.Padding(3);
            this.GroundTab.Size = new System.Drawing.Size(352, 212);
            this.GroundTab.TabIndex = 0;
            this.GroundTab.Text = "地形";
            this.GroundTab.UseVisualStyleBackColor = true;
            // 
            // ItemTab
            // 
            this.ItemTab.Controls.Add(this.ItemType);
            this.ItemTab.Controls.Add(this.label4);
            this.ItemTab.Controls.Add(this.ItemPositionYAxis);
            this.ItemTab.Controls.Add(this.ItemPositionXAxis);
            this.ItemTab.Controls.Add(this.label3);
            this.ItemTab.Controls.Add(this.label2);
            this.ItemTab.Controls.Add(this.label1);
            this.ItemTab.Location = new System.Drawing.Point(4, 22);
            this.ItemTab.Name = "ItemTab";
            this.ItemTab.Padding = new System.Windows.Forms.Padding(3);
            this.ItemTab.Size = new System.Drawing.Size(352, 212);
            this.ItemTab.TabIndex = 1;
            this.ItemTab.Text = "アイテム";
            this.ItemTab.UseVisualStyleBackColor = true;
            // 
            // DecolationTab
            // 
            this.DecolationTab.Location = new System.Drawing.Point(4, 22);
            this.DecolationTab.Name = "DecolationTab";
            this.DecolationTab.Padding = new System.Windows.Forms.Padding(3);
            this.DecolationTab.Size = new System.Drawing.Size(252, 212);
            this.DecolationTab.TabIndex = 2;
            this.DecolationTab.Text = "装飾";
            this.DecolationTab.UseVisualStyleBackColor = true;
            // 
            // EnemyTab
            // 
            this.EnemyTab.Location = new System.Drawing.Point(4, 22);
            this.EnemyTab.Name = "EnemyTab";
            this.EnemyTab.Padding = new System.Windows.Forms.Padding(3);
            this.EnemyTab.Size = new System.Drawing.Size(352, 212);
            this.EnemyTab.TabIndex = 3;
            this.EnemyTab.Text = "敵";
            this.EnemyTab.UseVisualStyleBackColor = true;
            // 
            // PlayerTab
            // 
            this.PlayerTab.Location = new System.Drawing.Point(4, 22);
            this.PlayerTab.Name = "PlayerTab";
            this.PlayerTab.Padding = new System.Windows.Forms.Padding(3);
            this.PlayerTab.Size = new System.Drawing.Size(352, 212);
            this.PlayerTab.TabIndex = 4;
            this.PlayerTab.Text = "プレイヤー";
            this.PlayerTab.UseVisualStyleBackColor = true;
            this.PlayerTab.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // StageTab
            // 
            this.StageTab.Location = new System.Drawing.Point(4, 22);
            this.StageTab.Name = "StageTab";
            this.StageTab.Padding = new System.Windows.Forms.Padding(3);
            this.StageTab.Size = new System.Drawing.Size(352, 212);
            this.StageTab.TabIndex = 5;
            this.StageTab.Text = "ステージ";
            this.StageTab.UseVisualStyleBackColor = true;
            // 
            // StartPoint
            // 
            this.StartPoint.AutoSize = true;
            this.StartPoint.Location = new System.Drawing.Point(7, 4);
            this.StartPoint.Name = "StartPoint";
            this.StartPoint.Size = new System.Drawing.Size(29, 12);
            this.StartPoint.TabIndex = 0;
            this.StartPoint.Text = "始点";
            // 
            // StartXAxis
            // 
            this.StartXAxis.AutoSize = true;
            this.StartXAxis.Location = new System.Drawing.Point(9, 20);
            this.StartXAxis.Name = "StartXAxis";
            this.StartXAxis.Size = new System.Drawing.Size(36, 12);
            this.StartXAxis.TabIndex = 1;
            this.StartXAxis.Text = "X座標";
            // 
            // StartYAxis
            // 
            this.StartYAxis.AutoSize = true;
            this.StartYAxis.Location = new System.Drawing.Point(120, 20);
            this.StartYAxis.Name = "StartYAxis";
            this.StartYAxis.Size = new System.Drawing.Size(36, 12);
            this.StartYAxis.TabIndex = 2;
            this.StartYAxis.Text = "Y座標";
            // 
            // StartXAxisTextBox
            // 
            this.StartXAxisTextBox.Location = new System.Drawing.Point(9, 36);
            this.StartXAxisTextBox.Mask = "000.000";
            this.StartXAxisTextBox.Name = "StartXAxisTextBox";
            this.StartXAxisTextBox.Size = new System.Drawing.Size(100, 19);
            this.StartXAxisTextBox.TabIndex = 0;
            // 
            // maskedTextBox2
            // 
            this.maskedTextBox2.Location = new System.Drawing.Point(122, 36);
            this.maskedTextBox2.Mask = "000.000";
            this.maskedTextBox2.Name = "maskedTextBox2";
            this.maskedTextBox2.Size = new System.Drawing.Size(100, 19);
            this.maskedTextBox2.TabIndex = 1;
            // 
            // TerminatePoint
            // 
            this.TerminatePoint.AutoSize = true;
            this.TerminatePoint.Location = new System.Drawing.Point(7, 71);
            this.TerminatePoint.Name = "TerminatePoint";
            this.TerminatePoint.Size = new System.Drawing.Size(29, 12);
            this.TerminatePoint.TabIndex = 0;
            this.TerminatePoint.Text = "終点";
            // 
            // TerminatePointXAxis
            // 
            this.TerminatePointXAxis.AutoSize = true;
            this.TerminatePointXAxis.Location = new System.Drawing.Point(9, 87);
            this.TerminatePointXAxis.Name = "TerminatePointXAxis";
            this.TerminatePointXAxis.Size = new System.Drawing.Size(36, 12);
            this.TerminatePointXAxis.TabIndex = 1;
            this.TerminatePointXAxis.Text = "X座標";
            // 
            // TerminatePointYAxis
            // 
            this.TerminatePointYAxis.AutoSize = true;
            this.TerminatePointYAxis.Location = new System.Drawing.Point(120, 87);
            this.TerminatePointYAxis.Name = "TerminatePointYAxis";
            this.TerminatePointYAxis.Size = new System.Drawing.Size(36, 12);
            this.TerminatePointYAxis.TabIndex = 2;
            this.TerminatePointYAxis.Text = "Y座標";
            // 
            // TerminatePointXAxisTextBox
            // 
            this.TerminatePointXAxisTextBox.Location = new System.Drawing.Point(9, 103);
            this.TerminatePointXAxisTextBox.Mask = "000.000";
            this.TerminatePointXAxisTextBox.Name = "TerminatePointXAxisTextBox";
            this.TerminatePointXAxisTextBox.Size = new System.Drawing.Size(100, 19);
            this.TerminatePointXAxisTextBox.TabIndex = 2;
            // 
            // TerminatePointYAxitTextBox
            // 
            this.TerminatePointYAxitTextBox.Location = new System.Drawing.Point(122, 103);
            this.TerminatePointYAxitTextBox.Mask = "000.000";
            this.TerminatePointYAxitTextBox.Name = "TerminatePointYAxitTextBox";
            this.TerminatePointYAxitTextBox.Size = new System.Drawing.Size(100, 19);
            this.TerminatePointYAxitTextBox.TabIndex = 3;
            // 
            // TypeId
            // 
            this.TypeId.AutoSize = true;
            this.TypeId.Location = new System.Drawing.Point(9, 129);
            this.TypeId.Name = "TypeId";
            this.TypeId.Size = new System.Drawing.Size(29, 12);
            this.TypeId.TabIndex = 5;
            this.TypeId.Text = "種類";
            // 
            // CollisionType
            // 
            this.CollisionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CollisionType.FormattingEnabled = true;
            this.CollisionType.Items.AddRange(new object[] {
            "Floor",
            "RightWall",
            "LeftWall",
            "Ceiling"});
            this.CollisionType.Location = new System.Drawing.Point(9, 145);
            this.CollisionType.Name = "CollisionType";
            this.CollisionType.Size = new System.Drawing.Size(121, 20);
            this.CollisionType.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "位置";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "X座標";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Y座標";
            // 
            // ItemPositionXAxis
            // 
            this.ItemPositionXAxis.Location = new System.Drawing.Point(9, 36);
            this.ItemPositionXAxis.Mask = "000.000";
            this.ItemPositionXAxis.Name = "ItemPositionXAxis";
            this.ItemPositionXAxis.Size = new System.Drawing.Size(100, 19);
            this.ItemPositionXAxis.TabIndex = 0;
            // 
            // ItemPositionYAxis
            // 
            this.ItemPositionYAxis.Location = new System.Drawing.Point(122, 36);
            this.ItemPositionYAxis.Mask = "000.000";
            this.ItemPositionYAxis.Name = "ItemPositionYAxis";
            this.ItemPositionYAxis.Size = new System.Drawing.Size(100, 19);
            this.ItemPositionYAxis.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "種類";
            // 
            // ItemType
            // 
            this.ItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ItemType.FormattingEnabled = true;
            this.ItemType.Items.AddRange(new object[] {
            "Coin",
            "Portion"});
            this.ItemType.Location = new System.Drawing.Point(11, 78);
            this.ItemType.Name = "ItemType";
            this.ItemType.Size = new System.Drawing.Size(121, 20);
            this.ItemType.TabIndex = 2;
            // 
            // PropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 262);
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.Name = "PropertyForm";
            this.Text = "PropertyForm";
            this.tabControl.ResumeLayout(false);
            this.GroundTab.ResumeLayout(false);
            this.GroundTab.PerformLayout();
            this.ItemTab.ResumeLayout(false);
            this.ItemTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage GroundTab;
        private System.Windows.Forms.TabPage ItemTab;
        private System.Windows.Forms.TabPage DecolationTab;
        private System.Windows.Forms.TabPage EnemyTab;
        private System.Windows.Forms.TabPage PlayerTab;
        private System.Windows.Forms.TabPage StageTab;
        private System.Windows.Forms.MaskedTextBox StartXAxisTextBox;
        private System.Windows.Forms.Label StartYAxis;
        private System.Windows.Forms.Label StartXAxis;
        private System.Windows.Forms.Label StartPoint;
        private System.Windows.Forms.MaskedTextBox maskedTextBox2;
        private System.Windows.Forms.MaskedTextBox TerminatePointYAxitTextBox;
        private System.Windows.Forms.MaskedTextBox TerminatePointXAxisTextBox;
        private System.Windows.Forms.Label TerminatePointYAxis;
        private System.Windows.Forms.Label TerminatePointXAxis;
        private System.Windows.Forms.Label TerminatePoint;
        private System.Windows.Forms.ComboBox CollisionType;
        private System.Windows.Forms.Label TypeId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox ItemPositionXAxis;
        private System.Windows.Forms.MaskedTextBox ItemPositionYAxis;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ItemType;
    }
}