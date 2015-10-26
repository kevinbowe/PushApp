﻿namespace Push
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
			this.lbStatus = new System.Windows.Forms.ListBox();
			this.btnDebug2 = new System.Windows.Forms.Button();
			this.btnDebug4 = new System.Windows.Forms.Button();
			this.btnDebug5 = new System.Windows.Forms.Button();
			this.pnlDetails = new System.Windows.Forms.Panel();
			this.splitContainerDetails = new System.Windows.Forms.SplitContainer();
			this.lvSource = new System.Windows.Forms.ListView();
			this.FName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.FSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lvTarget = new System.Windows.Forms.ListView();
			this.FNameTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.TypeTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SizeTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.DateTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblSource = new System.Windows.Forms.Label();
			this.lblTarget = new System.Windows.Forms.Label();
			this.lblShowHide = new System.Windows.Forms.Label();
			this.picBxPush = new System.Windows.Forms.PictureBox();
			this.picBxShowHide = new System.Windows.Forms.PictureBox();
			this.toolStripBtnPush = new System.Windows.Forms.ToolStripButton();
			this.toolStripBtnRefresh = new System.Windows.Forms.ToolStripButton();
			this.tooStripBtnConfig = new System.Windows.Forms.ToolStripButton();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.pnlDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerDetails)).BeginInit();
			this.splitContainerDetails.Panel1.SuspendLayout();
			this.splitContainerDetails.Panel2.SuspendLayout();
			this.splitContainerDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBxPush)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picBxShowHide)).BeginInit();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbStatus
			// 
			this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbStatus.FormattingEnabled = true;
			this.lbStatus.Location = new System.Drawing.Point(95, 26);
			this.lbStatus.Name = "lbStatus";
			this.lbStatus.ScrollAlwaysVisible = true;
			this.lbStatus.Size = new System.Drawing.Size(639, 69);
			this.lbStatus.TabIndex = 1;
			// 
			// btnDebug2
			// 
			this.btnDebug2.BackColor = System.Drawing.Color.MistyRose;
			this.btnDebug2.FlatAppearance.BorderColor = System.Drawing.Color.MistyRose;
			this.btnDebug2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDebug2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDebug2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnDebug2.Location = new System.Drawing.Point(730, 18);
			this.btnDebug2.Name = "btnDebug2";
			this.btnDebug2.Size = new System.Drawing.Size(21, 23);
			this.btnDebug2.TabIndex = 2;
			this.btnDebug2.Text = "<< DEBUG >>";
			this.btnDebug2.UseVisualStyleBackColor = false;
			this.btnDebug2.Visible = false;
			this.btnDebug2.Click += new System.EventHandler(this.btnDebug2_Click);
			// 
			// btnDebug4
			// 
			this.btnDebug4.BackColor = System.Drawing.Color.PaleGreen;
			this.btnDebug4.FlatAppearance.BorderColor = System.Drawing.Color.MistyRose;
			this.btnDebug4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDebug4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDebug4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnDebug4.Location = new System.Drawing.Point(730, 47);
			this.btnDebug4.Name = "btnDebug4";
			this.btnDebug4.Size = new System.Drawing.Size(21, 23);
			this.btnDebug4.TabIndex = 12;
			this.btnDebug4.Text = "<< DEBUG >>";
			this.btnDebug4.UseVisualStyleBackColor = false;
			this.btnDebug4.Visible = false;
			this.btnDebug4.Click += new System.EventHandler(this.btnDebug4_Click);
			// 
			// btnDebug5
			// 
			this.btnDebug5.BackColor = System.Drawing.Color.PowderBlue;
			this.btnDebug5.FlatAppearance.BorderColor = System.Drawing.Color.MistyRose;
			this.btnDebug5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDebug5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDebug5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnDebug5.Location = new System.Drawing.Point(730, 76);
			this.btnDebug5.Name = "btnDebug5";
			this.btnDebug5.Size = new System.Drawing.Size(21, 23);
			this.btnDebug5.TabIndex = 13;
			this.btnDebug5.Text = "<< DEBUG >>";
			this.btnDebug5.UseVisualStyleBackColor = false;
			this.btnDebug5.Visible = false;
			this.btnDebug5.Click += new System.EventHandler(this.btnDebug5_Click);
			// 
			// pnlDetails
			// 
			this.pnlDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlDetails.Controls.Add(this.splitContainerDetails);
			this.pnlDetails.Location = new System.Drawing.Point(16, 139);
			this.pnlDetails.Name = "pnlDetails";
			this.pnlDetails.Size = new System.Drawing.Size(718, 96);
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
			this.splitContainerDetails.Panel1.Controls.Add(this.lvSource);
			// 
			// splitContainerDetails.Panel2
			// 
			this.splitContainerDetails.Panel2.Controls.Add(this.lvTarget);
			this.splitContainerDetails.Size = new System.Drawing.Size(718, 96);
			this.splitContainerDetails.SplitterDistance = 357;
			this.splitContainerDetails.TabIndex = 0;
			// 
			// lvSource
			// 
			this.lvSource.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FName,
            this.Type,
            this.FSize,
            this.Date});
			this.lvSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvSource.FullRowSelect = true;
			this.lvSource.Location = new System.Drawing.Point(0, 0);
			this.lvSource.Name = "lvSource";
			this.lvSource.Size = new System.Drawing.Size(357, 96);
			this.lvSource.TabIndex = 7;
			this.lvSource.UseCompatibleStateImageBehavior = false;
			this.lvSource.View = System.Windows.Forms.View.Details;
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
			// lvTarget
			// 
			this.lvTarget.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FNameTarget,
            this.TypeTarget,
            this.SizeTarget,
            this.DateTarget});
			this.lvTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvTarget.Location = new System.Drawing.Point(0, 0);
			this.lvTarget.Name = "lvTarget";
			this.lvTarget.Size = new System.Drawing.Size(357, 96);
			this.lvTarget.TabIndex = 8;
			this.lvTarget.UseCompatibleStateImageBehavior = false;
			this.lvTarget.View = System.Windows.Forms.View.Details;
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
			// lblSource
			// 
			this.lblSource.AutoSize = true;
			this.lblSource.Location = new System.Drawing.Point(16, 120);
			this.lblSource.Name = "lblSource";
			this.lblSource.Size = new System.Drawing.Size(73, 13);
			this.lblSource.TabIndex = 17;
			this.lblSource.Text = "Source Folder";
			// 
			// lblTarget
			// 
			this.lblTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTarget.AutoSize = true;
			this.lblTarget.Location = new System.Drawing.Point(664, 120);
			this.lblTarget.Name = "lblTarget";
			this.lblTarget.Size = new System.Drawing.Size(70, 13);
			this.lblTarget.TabIndex = 18;
			this.lblTarget.Text = "Target Folder";
			this.lblTarget.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblShowHide
			// 
			this.lblShowHide.AutoSize = true;
			this.lblShowHide.Location = new System.Drawing.Point(34, 103);
			this.lblShowHide.Name = "lblShowHide";
			this.lblShowHide.Size = new System.Drawing.Size(64, 13);
			this.lblShowHide.TabIndex = 20;
			this.lblShowHide.Text = "Hide Details";
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
			this.picBxShowHide.Location = new System.Drawing.Point(15, 101);
			this.picBxShowHide.Name = "picBxShowHide";
			this.picBxShowHide.Size = new System.Drawing.Size(17, 19);
			this.picBxShowHide.TabIndex = 19;
			this.picBxShowHide.TabStop = false;
			this.picBxShowHide.Click += new System.EventHandler(this.picBoxShowHide_Click);
			// 
			// toolStripBtnPush
			// 
			this.toolStripBtnPush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripBtnPush.Image = global::Push.Properties.Resources.Run;
			this.toolStripBtnPush.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripBtnPush.Name = "toolStripBtnPush";
			this.toolStripBtnPush.Size = new System.Drawing.Size(23, 22);
			this.toolStripBtnPush.Text = "toolStripButton4";
			this.toolStripBtnPush.Click += new System.EventHandler(this.toolStripBtnPush_Click);
			// 
			// toolStripBtnRefresh
			// 
			this.toolStripBtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripBtnRefresh.Image = global::Push.Properties.Resources.Refresh;
			this.toolStripBtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripBtnRefresh.Name = "toolStripBtnRefresh";
			this.toolStripBtnRefresh.Size = new System.Drawing.Size(23, 22);
			this.toolStripBtnRefresh.Text = "toolStripButton5";
			this.toolStripBtnRefresh.Click += new System.EventHandler(this.toolStripBtnRefresh_Click);
			// 
			// tooStripBtnConfig
			// 
			this.tooStripBtnConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tooStripBtnConfig.Image = global::Push.Properties.Resources.gear;
			this.tooStripBtnConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tooStripBtnConfig.Name = "tooStripBtnConfig";
			this.tooStripBtnConfig.Size = new System.Drawing.Size(23, 22);
			this.tooStripBtnConfig.Text = "toolStripButton6";
			this.tooStripBtnConfig.Click += new System.EventHandler(this.tooStripBtnConfig_Click);
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnPush,
            this.toolStripBtnRefresh,
            this.tooStripBtnConfig});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(748, 25);
			this.toolStrip.TabIndex = 22;
			this.toolStrip.Text = "toolStrip1";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(748, 247);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.picBxPush);
			this.Controls.Add(this.lblShowHide);
			this.Controls.Add(this.picBxShowHide);
			this.Controls.Add(this.lblTarget);
			this.Controls.Add(this.lblSource);
			this.Controls.Add(this.pnlDetails);
			this.Controls.Add(this.btnDebug5);
			this.Controls.Add(this.btnDebug4);
			this.Controls.Add(this.btnDebug2);
			this.Controls.Add(this.lbStatus);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Push Application";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.pnlDetails.ResumeLayout(false);
			this.splitContainerDetails.Panel1.ResumeLayout(false);
			this.splitContainerDetails.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerDetails)).EndInit();
			this.splitContainerDetails.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picBxPush)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picBxShowHide)).EndInit();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.ListBox lbStatus;
		private System.Windows.Forms.Button btnDebug2;
		private System.Windows.Forms.Button btnDebug4;
		private System.Windows.Forms.Button btnDebug5;
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
		private System.Windows.Forms.Label lblSource;
		private System.Windows.Forms.Label lblTarget;
		private System.Windows.Forms.PictureBox picBxShowHide;
		private System.Windows.Forms.Label lblShowHide;
		private System.Windows.Forms.PictureBox picBxPush;
		private System.Windows.Forms.ToolStripButton toolStripBtnPush;
		private System.Windows.Forms.ToolStripButton toolStripBtnRefresh;
		private System.Windows.Forms.ToolStripButton tooStripBtnConfig;
		private System.Windows.Forms.ToolStrip toolStrip;
    }
}
