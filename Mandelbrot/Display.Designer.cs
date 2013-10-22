namespace Mandelbrot
{
    partial class Display
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Display));
            this.picture = new System.Windows.Forms.PictureBox();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.slotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slot1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slot2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quicksaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quickloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cycleColoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.colourCycleTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // picture
            // 
            this.picture.BackColor = System.Drawing.SystemColors.Control;
            this.picture.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picture.Location = new System.Drawing.Point(0, 24);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(800, 600);
            this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            this.picture.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxPaint);
            this.picture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousePressed);
            this.picture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseDragged);
            this.picture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseReleased);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.mainMenuStrip.TabIndex = 1;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToClipboardToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.slotToolStripMenuItem,
            this.quicksaveToolStripMenuItem,
            this.quickloadToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy Image to Clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.saveToolStripMenuItem.Text = "Save Image As...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(204, 6);
            // 
            // slotToolStripMenuItem
            // 
            this.slotToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slot1MenuItem,
            this.slot2MenuItem});
            this.slotToolStripMenuItem.Name = "slotToolStripMenuItem";
            this.slotToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.slotToolStripMenuItem.Text = "Slot";
            // 
            // slot1MenuItem
            // 
            this.slot1MenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.slot1MenuItem.Checked = true;
            this.slot1MenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.slot1MenuItem.Name = "slot1MenuItem";
            this.slot1MenuItem.Size = new System.Drawing.Size(152, 22);
            this.slot1MenuItem.Text = "1";
            this.slot1MenuItem.Click += new System.EventHandler(this.slot1Click);
            // 
            // slot2MenuItem
            // 
            this.slot2MenuItem.Name = "slot2MenuItem";
            this.slot2MenuItem.Size = new System.Drawing.Size(152, 22);
            this.slot2MenuItem.Text = "2";
            this.slot2MenuItem.Click += new System.EventHandler(this.slot2MenuItem_Click);
            // 
            // quicksaveToolStripMenuItem
            // 
            this.quicksaveToolStripMenuItem.Name = "quicksaveToolStripMenuItem";
            this.quicksaveToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.quicksaveToolStripMenuItem.Text = "Quicksave";
            this.quicksaveToolStripMenuItem.Click += new System.EventHandler(this.quicksaveToolStripMenuItem_Click);
            // 
            // quickloadToolStripMenuItem
            // 
            this.quickloadToolStripMenuItem.Name = "quickloadToolStripMenuItem";
            this.quickloadToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.quickloadToolStripMenuItem.Text = "Quickload";
            this.quickloadToolStripMenuItem.Click += new System.EventHandler(this.quickloadToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cycleColoursToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // cycleColoursToolStripMenuItem
            // 
            this.cycleColoursToolStripMenuItem.Name = "cycleColoursToolStripMenuItem";
            this.cycleColoursToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.cycleColoursToolStripMenuItem.Text = "Cycle Colours";
            this.cycleColoursToolStripMenuItem.Click += new System.EventHandler(this.cycleColoursMainMenu_Click);
            // 
            // colourCycleTimer
            // 
            this.colourCycleTimer.Tick += new System.EventHandler(this.colourCycleTimer_Tick);
            // 
            // Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 624);
            this.Controls.Add(this.picture);
            this.Controls.Add(this.mainMenuStrip);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "Display";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Display";
            this.Resize += new System.EventHandler(this.Display_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picture;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quicksaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quickloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cycleColoursToolStripMenuItem;
        private System.Windows.Forms.Timer colourCycleTimer;
        private System.Windows.Forms.ToolStripMenuItem slotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem slot1MenuItem;
        private System.Windows.Forms.ToolStripMenuItem slot2MenuItem;
    }
}

