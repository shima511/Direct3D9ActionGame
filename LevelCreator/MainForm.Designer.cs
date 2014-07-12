namespace LevelCreator
{
    partial class LevelCreator
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.OverSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.編集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.元に戻すToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.やり直しRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.地形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.アイテムToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.敵ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.装飾ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.編集ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewProjectToolStripMenuItem,
            this.OpenFileStripMenuItem1,
            this.OverSaveToolStripMenuItem,
            this.NewSaveToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            this.ファイルFToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.ファイルFToolStripMenuItem.Text = "ファイル(F)";
            // 
            // NewProjectToolStripMenuItem
            // 
            this.NewProjectToolStripMenuItem.Name = "NewProjectToolStripMenuItem";
            this.NewProjectToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.NewProjectToolStripMenuItem.Text = "新規作成";
            // 
            // OpenFileStripMenuItem1
            // 
            this.OpenFileStripMenuItem1.Name = "OpenFileStripMenuItem1";
            this.OpenFileStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenFileStripMenuItem1.Size = new System.Drawing.Size(227, 22);
            this.OpenFileStripMenuItem1.Text = "ファイルを開く(O)";
            this.OpenFileStripMenuItem1.Click += new System.EventHandler(this.OpenFileStripMenuItem1_Click);
            // 
            // OverSaveToolStripMenuItem
            // 
            this.OverSaveToolStripMenuItem.Name = "OverSaveToolStripMenuItem";
            this.OverSaveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.OverSaveToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.OverSaveToolStripMenuItem.Text = "上書き保存";
            this.OverSaveToolStripMenuItem.Click += new System.EventHandler(this.OverSaveToolStripMenuItem_Click);
            // 
            // NewSaveToolStripMenuItem
            // 
            this.NewSaveToolStripMenuItem.Name = "NewSaveToolStripMenuItem";
            this.NewSaveToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.NewSaveToolStripMenuItem.Text = "名前を付けて保存";
            this.NewSaveToolStripMenuItem.Click += new System.EventHandler(this.NewSaveToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.ExitToolStripMenuItem.Text = "終了(X)";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // 編集ToolStripMenuItem
            // 
            this.編集ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.元に戻すToolStripMenuItem,
            this.やり直しRToolStripMenuItem});
            this.編集ToolStripMenuItem.Name = "編集ToolStripMenuItem";
            this.編集ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
            this.編集ToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
            this.編集ToolStripMenuItem.Text = "編集(E)";
            this.編集ToolStripMenuItem.Click += new System.EventHandler(this.EditToolStripMenuItem_Click);
            // 
            // 元に戻すToolStripMenuItem
            // 
            this.元に戻すToolStripMenuItem.Name = "元に戻すToolStripMenuItem";
            this.元に戻すToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.元に戻すToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.元に戻すToolStripMenuItem.Text = "元に戻す(U)";
            // 
            // やり直しRToolStripMenuItem
            // 
            this.やり直しRToolStripMenuItem.Name = "やり直しRToolStripMenuItem";
            this.やり直しRToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.やり直しRToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.やり直しRToolStripMenuItem.Text = "やり直し(R)";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.地形ToolStripMenuItem,
            this.アイテムToolStripMenuItem,
            this.敵ToolStripMenuItem,
            this.装飾ToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.testToolStripMenuItem.Text = "部品追加";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // 地形ToolStripMenuItem
            // 
            this.地形ToolStripMenuItem.Name = "地形ToolStripMenuItem";
            this.地形ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.地形ToolStripMenuItem.Text = "地形";
            // 
            // アイテムToolStripMenuItem
            // 
            this.アイテムToolStripMenuItem.Name = "アイテムToolStripMenuItem";
            this.アイテムToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.アイテムToolStripMenuItem.Text = "アイテム";
            // 
            // 敵ToolStripMenuItem
            // 
            this.敵ToolStripMenuItem.Name = "敵ToolStripMenuItem";
            this.敵ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.敵ToolStripMenuItem.Text = "敵";
            // 
            // 装飾ToolStripMenuItem
            // 
            this.装飾ToolStripMenuItem.Name = "装飾ToolStripMenuItem";
            this.装飾ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.装飾ToolStripMenuItem.Text = "装飾";
            // 
            // LevelCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "LevelCreator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LevelCreator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OverSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 編集ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 元に戻すToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem やり直しRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenFileStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 地形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem アイテムToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 敵ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 装飾ToolStripMenuItem;

    }
}

