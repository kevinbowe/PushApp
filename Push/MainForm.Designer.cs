namespace Push
{
    partial class MainForm
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
			this.pnlDetails = new System.Windows.Forms.Panel();
			this.splitContainerDetails = new System.Windows.Forms.SplitContainer();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblSourcePath = new System.Windows.Forms.Label();
			this.lblSource = new System.Windows.Forms.Label();
			this.lvSource = new System.Windows.Forms.ListView();
			this.FName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.FSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.lblTargetPath = new System.Windows.Forms.Label();
			this.lblTarget = new System.Windows.Forms.Label();
			this.lvTarget = new System.Windows.Forms.ListView();
			this.FNameTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.TypeTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SizeTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.DateTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblShowHide = new System.Windows.Forms.Label();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripBtnPush = new System.Windows.Forms.ToolStripButton();
			this.toolStripBtnRefresh = new System.Windows.Forms.ToolStripButton();
			this.toolStripBtnConfig = new System.Windows.Forms.ToolStripButton();
			this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.toolStripLblProgress = new System.Windows.Forms.ToolStripLabel();
			this.lblFileFilter = new System.Windows.Forms.Label();
			this.lblFileExtensionFilterString = new System.Windows.Forms.Label();
			this.ttDetails = new System.Windows.Forms.ToolTip(this.components);
			this.lblDuplicateFileAction = new System.Windows.Forms.Label();
			this.lblDupeFileActionText = new System.Windows.Forms.Label();
			this.lblStatus1_1 = new System.Windows.Forms.Label();
			this.lblStatus1_2 = new System.Windows.Forms.Label();
			this.lblStatus2_2 = new System.Windows.Forms.Label();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.picBxPush = new System.Windows.Forms.PictureBox();
			this.picBxShowHide = new System.Windows.Forms.PictureBox();
			this.pnlDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerDetails)).BeginInit();
			this.splitContainerDetails.Panel1.SuspendLayout();
			this.splitContainerDetails.Panel2.SuspendLayout();
			this.splitContainerDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBxPush)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picBxShowHide)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlDetails
			// 
			this.pnlDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlDetails.Controls.Add(this.splitContainerDetails);
			this.pnlDetails.Location = new System.Drawing.Point(17, 144);
			this.pnlDetails.Name = "pnlDetails";
			this.pnlDetails.Size = new System.Drawing.Size(507, 102);
			this.pnlDetails.TabIndex = 16;
			// 
			// splitContainerDetails
			// 
			this.splitContainerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerDetails.Location = new System.Drawing.Point(0, 0);
			this.splitContainerDetails.Name = "splitContainerDetails";
			// 
			// splitContainerDetails.Panel1
			// 
			this.splitContainerDetails.Panel1.Controls.Add(this.pictureBox1);
			this.splitContainerDetails.Panel1.Controls.Add(this.lblSourcePath);
			this.splitContainerDetails.Panel1.Controls.Add(this.lblSource);
			this.splitContainerDetails.Panel1.Controls.Add(this.lvSource);
			// 
			// splitContainerDetails.Panel2
			// 
			this.splitContainerDetails.Panel2.Controls.Add(this.pictureBox2);
			this.splitContainerDetails.Panel2.Controls.Add(this.lblTargetPath);
			this.splitContainerDetails.Panel2.Controls.Add(this.lblTarget);
			this.splitContainerDetails.Panel2.Controls.Add(this.lvTarget);
			this.splitContainerDetails.Size = new System.Drawing.Size(507, 102);
			this.splitContainerDetails.SplitterDistance = 254;
			this.splitContainerDetails.TabIndex = 0;
			this.splitContainerDetails.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerDetails_SplitterMoved);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Push.Properties.Resources.Fit;
			this.pictureBox1.Location = new System.Drawing.Point(4, 4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(18, 15);
			this.pictureBox1.TabIndex = 10;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			// 
			// lblSourcePath
			// 
			this.lblSourcePath.AutoSize = true;
			this.lblSourcePath.BackColor = System.Drawing.SystemColors.Control;
			this.lblSourcePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSourcePath.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lblSourcePath.Location = new System.Drawing.Point(99, 6);
			this.lblSourcePath.Name = "lblSourcePath";
			this.lblSourcePath.Size = new System.Drawing.Size(73, 13);
			this.lblSourcePath.TabIndex = 9;
			this.lblSourcePath.Text = "SourcePath";
			// 
			// lblSource
			// 
			this.lblSource.AutoSize = true;
			this.lblSource.Location = new System.Drawing.Point(24, 6);
			this.lblSource.Name = "lblSource";
			this.lblSource.Size = new System.Drawing.Size(76, 13);
			this.lblSource.TabIndex = 8;
			this.lblSource.Text = "Source Folder:";
			// 
			// lvSource
			// 
			this.lvSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvSource.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FName,
            this.Type,
            this.FSize,
            this.Date});
			this.lvSource.FullRowSelect = true;
			this.lvSource.Location = new System.Drawing.Point(0, 24);
			this.lvSource.Name = "lvSource";
			this.lvSource.Size = new System.Drawing.Size(251, 78);
			this.lvSource.TabIndex = 7;
			this.lvSource.UseCompatibleStateImageBehavior = false;
			this.lvSource.View = System.Windows.Forms.View.Details;
			this.lvSource.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView_ColumnClick);
			this.lvSource.SizeChanged += new System.EventHandler(this.lvSource_SizeChanged);
			// 
			// FName
			// 
			this.FName.Text = "Name";
			this.FName.Width = 73;
			// 
			// Type
			// 
			this.Type.Text = "Type";
			this.Type.Width = 74;
			// 
			// FSize
			// 
			this.FSize.Text = "Size";
			// 
			// Date
			// 
			this.Date.Text = "Date";
			this.Date.Width = 118;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::Push.Properties.Resources.Fit;
			this.pictureBox2.Location = new System.Drawing.Point(3, 4);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(18, 15);
			this.pictureBox2.TabIndex = 11;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
			// 
			// lblTargetPath
			// 
			this.lblTargetPath.AutoSize = true;
			this.lblTargetPath.BackColor = System.Drawing.SystemColors.Control;
			this.lblTargetPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTargetPath.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lblTargetPath.Location = new System.Drawing.Point(103, 6);
			this.lblTargetPath.Name = "lblTargetPath";
			this.lblTargetPath.Size = new System.Drawing.Size(70, 13);
			this.lblTargetPath.TabIndex = 10;
			this.lblTargetPath.Text = "TargetPath";
			// 
			// lblTarget
			// 
			this.lblTarget.AutoSize = true;
			this.lblTarget.Location = new System.Drawing.Point(23, 6);
			this.lblTarget.Name = "lblTarget";
			this.lblTarget.Size = new System.Drawing.Size(73, 13);
			this.lblTarget.TabIndex = 9;
			this.lblTarget.Text = "Target Folder:";
			// 
			// lvTarget
			// 
			this.lvTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvTarget.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FNameTarget,
            this.TypeTarget,
            this.SizeTarget,
            this.DateTarget});
			this.lvTarget.Location = new System.Drawing.Point(0, 24);
			this.lvTarget.Name = "lvTarget";
			this.lvTarget.Size = new System.Drawing.Size(242, 78);
			this.lvTarget.TabIndex = 8;
			this.lvTarget.UseCompatibleStateImageBehavior = false;
			this.lvTarget.View = System.Windows.Forms.View.Details;
			this.lvTarget.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView_ColumnClick);
			// 
			// FNameTarget
			// 
			this.FNameTarget.Text = "Name";
			this.FNameTarget.Width = 73;
			// 
			// TypeTarget
			// 
			this.TypeTarget.Text = "Type";
			this.TypeTarget.Width = 74;
			// 
			// SizeTarget
			// 
			this.SizeTarget.Text = "Size";
			// 
			// DateTarget
			// 
			this.DateTarget.Text = "Date";
			this.DateTarget.Width = 118;
			// 
			// lblShowHide
			// 
			this.lblShowHide.AutoSize = true;
			this.lblShowHide.Location = new System.Drawing.Point(40, 103);
			this.lblShowHide.Name = "lblShowHide";
			this.lblShowHide.Size = new System.Drawing.Size(64, 13);
			this.lblShowHide.TabIndex = 20;
			this.lblShowHide.Text = "Hide Details";
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnPush,
            this.toolStripBtnRefresh,
            this.toolStripBtnConfig,
            this.toolStripProgressBar,
            this.toolStripLblProgress});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(529, 25);
			this.toolStrip.TabIndex = 22;
			this.toolStrip.Text = "toolStrip1";
			// 
			// toolStripBtnPush
			// 
			this.toolStripBtnPush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripBtnPush.Image = global::Push.Properties.Resources.Run;
			this.toolStripBtnPush.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripBtnPush.Name = "toolStripBtnPush";
			this.toolStripBtnPush.Size = new System.Drawing.Size(23, 22);
			this.toolStripBtnPush.Text = "Push";
			this.toolStripBtnPush.Click += new System.EventHandler(this.toolStripBtnPush_Click);
			// 
			// toolStripBtnRefresh
			// 
			this.toolStripBtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripBtnRefresh.Image = global::Push.Properties.Resources.Refresh;
			this.toolStripBtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripBtnRefresh.Name = "toolStripBtnRefresh";
			this.toolStripBtnRefresh.Size = new System.Drawing.Size(23, 22);
			this.toolStripBtnRefresh.Text = "Refresh";
			this.toolStripBtnRefresh.Click += new System.EventHandler(this.toolStripBtnRefresh_Click);
			// 
			// toolStripBtnConfig
			// 
			this.toolStripBtnConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripBtnConfig.Image = global::Push.Properties.Resources.gear;
			this.toolStripBtnConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripBtnConfig.Name = "toolStripBtnConfig";
			this.toolStripBtnConfig.Size = new System.Drawing.Size(23, 22);
			this.toolStripBtnConfig.Text = "Configuration";
			this.toolStripBtnConfig.Click += new System.EventHandler(this.tooStripBtnConfig_Click);
			// 
			// toolStripProgressBar
			// 
			this.toolStripProgressBar.Name = "toolStripProgressBar";
			this.toolStripProgressBar.Size = new System.Drawing.Size(50, 22);
			// 
			// toolStripLblProgress
			// 
			this.toolStripLblProgress.ForeColor = System.Drawing.Color.RoyalBlue;
			this.toolStripLblProgress.Name = "toolStripLblProgress";
			this.toolStripLblProgress.Size = new System.Drawing.Size(113, 22);
			this.toolStripLblProgress.Text = "toolStripLblProgress";
			// 
			// lblFileFilter
			// 
			this.lblFileFilter.AutoSize = true;
			this.lblFileFilter.Location = new System.Drawing.Point(178, 128);
			this.lblFileFilter.Name = "lblFileFilter";
			this.lblFileFilter.Size = new System.Drawing.Size(51, 13);
			this.lblFileFilter.TabIndex = 23;
			this.lblFileFilter.Text = "File Filter:";
			// 
			// lblFileExtensionFilterString
			// 
			this.lblFileExtensionFilterString.AutoSize = true;
			this.lblFileExtensionFilterString.BackColor = System.Drawing.SystemColors.Control;
			this.lblFileExtensionFilterString.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFileExtensionFilterString.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lblFileExtensionFilterString.Location = new System.Drawing.Point(229, 128);
			this.lblFileExtensionFilterString.Name = "lblFileExtensionFilterString";
			this.lblFileExtensionFilterString.Size = new System.Drawing.Size(143, 13);
			this.lblFileExtensionFilterString.TabIndex = 24;
			this.lblFileExtensionFilterString.Text = "FileExtensionFilterString";
			// 
			// lblDuplicateFileAction
			// 
			this.lblDuplicateFileAction.AutoSize = true;
			this.lblDuplicateFileAction.Location = new System.Drawing.Point(21, 128);
			this.lblDuplicateFileAction.Name = "lblDuplicateFileAction";
			this.lblDuplicateFileAction.Size = new System.Drawing.Size(69, 13);
			this.lblDuplicateFileAction.TabIndex = 25;
			this.lblDuplicateFileAction.Text = "Dupe Action:";
			// 
			// lblDupeFileActionText
			// 
			this.lblDupeFileActionText.AutoSize = true;
			this.lblDupeFileActionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDupeFileActionText.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lblDupeFileActionText.Location = new System.Drawing.Point(90, 128);
			this.lblDupeFileActionText.Name = "lblDupeFileActionText";
			this.lblDupeFileActionText.Size = new System.Drawing.Size(68, 13);
			this.lblDupeFileActionText.TabIndex = 26;
			this.lblDupeFileActionText.Text = "ActionText";
			// 
			// lblStatus1_1
			// 
			this.lblStatus1_1.AutoSize = true;
			this.lblStatus1_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStatus1_1.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lblStatus1_1.Location = new System.Drawing.Point(94, 53);
			this.lblStatus1_1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblStatus1_1.Name = "lblStatus1_1";
			this.lblStatus1_1.Size = new System.Drawing.Size(0, 20);
			this.lblStatus1_1.TabIndex = 27;
			// 
			// lblStatus1_2
			// 
			this.lblStatus1_2.AutoSize = true;
			this.lblStatus1_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStatus1_2.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lblStatus1_2.Location = new System.Drawing.Point(94, 40);
			this.lblStatus1_2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblStatus1_2.Name = "lblStatus1_2";
			this.lblStatus1_2.Size = new System.Drawing.Size(39, 20);
			this.lblStatus1_2.TabIndex = 28;
			this.lblStatus1_2.Text = "1_2";
			// 
			// lblStatus2_2
			// 
			this.lblStatus2_2.AutoSize = true;
			this.lblStatus2_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStatus2_2.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lblStatus2_2.Location = new System.Drawing.Point(94, 63);
			this.lblStatus2_2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblStatus2_2.Name = "lblStatus2_2";
			this.lblStatus2_2.Size = new System.Drawing.Size(39, 20);
			this.lblStatus2_2.TabIndex = 29;
			this.lblStatus2_2.Text = "2_2";
			// 
			// picBxPush
			// 
			this.picBxPush.Image = global::Push.Properties.Resources.Green_Button1;
			this.picBxPush.Location = new System.Drawing.Point(16, 28);
			this.picBxPush.Name = "picBxPush";
			this.picBxPush.Size = new System.Drawing.Size(73, 67);
			this.picBxPush.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picBxPush.TabIndex = 21;
			this.picBxPush.TabStop = false;
			this.picBxPush.Click += new System.EventHandler(this.picBoxPush_Click);
			// 
			// picBxShowHide
			// 
			this.picBxShowHide.Image = global::Push.Properties.Resources.Control_Collapser1;
			this.picBxShowHide.Location = new System.Drawing.Point(21, 101);
			this.picBxShowHide.Name = "picBxShowHide";
			this.picBxShowHide.Size = new System.Drawing.Size(17, 19);
			this.picBxShowHide.TabIndex = 19;
			this.picBxShowHide.TabStop = false;
			this.picBxShowHide.Click += new System.EventHandler(this.picBoxShowHide_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(529, 261);
			this.Controls.Add(this.lblStatus2_2);
			this.Controls.Add(this.lblStatus1_2);
			this.Controls.Add(this.lblStatus1_1);
			this.Controls.Add(this.lblDupeFileActionText);
			this.Controls.Add(this.lblDuplicateFileAction);
			this.Controls.Add(this.lblFileExtensionFilterString);
			this.Controls.Add(this.lblFileFilter);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.picBxPush);
			this.Controls.Add(this.lblShowHide);
			this.Controls.Add(this.picBxShowHide);
			this.Controls.Add(this.pnlDetails);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Push Application";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.pnlDetails.ResumeLayout(false);
			this.splitContainerDetails.Panel1.ResumeLayout(false);
			this.splitContainerDetails.Panel1.PerformLayout();
			this.splitContainerDetails.Panel2.ResumeLayout(false);
			this.splitContainerDetails.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerDetails)).EndInit();
			this.splitContainerDetails.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBxPush)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picBxShowHide)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Panel pnlDetails;
		private System.Windows.Forms.SplitContainer splitContainerDetails;
		private System.Windows.Forms.ListView lvSource;
		private System.Windows.Forms.ColumnHeader FName;
		private System.Windows.Forms.ColumnHeader Type;
		private System.Windows.Forms.ColumnHeader FSize;
		private System.Windows.Forms.ColumnHeader Date;
		private System.Windows.Forms.ListView lvTarget;
		private System.Windows.Forms.ColumnHeader FNameTarget;
		private System.Windows.Forms.ColumnHeader TypeTarget;
		private System.Windows.Forms.ColumnHeader SizeTarget;
		private System.Windows.Forms.ColumnHeader DateTarget;
		private System.Windows.Forms.PictureBox picBxShowHide;
		private System.Windows.Forms.Label lblShowHide;
		private System.Windows.Forms.PictureBox picBxPush;
		private System.Windows.Forms.ToolStripButton toolStripBtnPush;
		private System.Windows.Forms.ToolStripButton toolStripBtnRefresh;
		private System.Windows.Forms.ToolStripButton toolStripBtnConfig;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.Label lblSource;
		private System.Windows.Forms.Label lblTarget;
		private System.Windows.Forms.Label lblSourcePath;
		private System.Windows.Forms.Label lblTargetPath;
		private System.Windows.Forms.Label lblFileFilter;
		private System.Windows.Forms.Label lblFileExtensionFilterString;
		private System.Windows.Forms.ToolTip ttDetails;
		private System.Windows.Forms.Label lblDuplicateFileAction;
		private System.Windows.Forms.Label lblDupeFileActionText;
		private System.Windows.Forms.Label lblStatus1_1;
		private System.Windows.Forms.Label lblStatus1_2;
		private System.Windows.Forms.Label lblStatus2_2;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
		private System.Windows.Forms.ToolStripLabel toolStripLblProgress;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
    }
}

