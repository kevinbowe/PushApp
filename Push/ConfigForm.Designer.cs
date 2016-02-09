namespace Push
{
	partial class ConfigForm
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpDupeHandeling = new System.Windows.Forms.GroupBox();
			this.rbCancel = new System.Windows.Forms.RadioButton();
			this.rbSkip = new System.Windows.Forms.RadioButton();
			this.rbRename = new System.Windows.Forms.RadioButton();
			this.rbOverwrite = new System.Windows.Forms.RadioButton();
			this.cbHideDupeFileMessage = new System.Windows.Forms.CheckBox();
			this.tbSourceFolder = new System.Windows.Forms.TextBox();
			this.lblSourceFolder = new System.Windows.Forms.Label();
			this.btnSourceFolderBrowse = new System.Windows.Forms.Button();
			this.btnTargetFolderBrowse = new System.Windows.Forms.Button();
			this.lblTargetFolder = new System.Windows.Forms.Label();
			this.tbTargetFolder = new System.Windows.Forms.TextBox();
			this.lblFileExtensions = new System.Windows.Forms.Label();
			this.tbFileExtensions = new System.Windows.Forms.TextBox();
			this.btnFileExtensionsClear = new System.Windows.Forms.Button();
			this.btnFileExtensionsFileSelect = new System.Windows.Forms.Button();
			this.btnTargetClear = new System.Windows.Forms.Button();
			this.btnSourceFolderClear = new System.Windows.Forms.Button();
			this.errorProviderSourceFolder = new System.Windows.Forms.ErrorProvider(this.components);
			this.errorProviderTargetFolder = new System.Windows.Forms.ErrorProvider(this.components);
			this.errorProviderDuplicateHandeling = new System.Windows.Forms.ErrorProvider(this.components);
			this.errorProviderFileExtensions = new System.Windows.Forms.ErrorProvider(this.components);
			this.grpDupeHandeling.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProviderSourceFolder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProviderTargetFolder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProviderDuplicateHandeling)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProviderFileExtensions)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(217, 210);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(298, 210);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// grpDupeHandeling
			// 
			this.grpDupeHandeling.Controls.Add(this.rbCancel);
			this.grpDupeHandeling.Controls.Add(this.rbSkip);
			this.grpDupeHandeling.Controls.Add(this.rbRename);
			this.grpDupeHandeling.Controls.Add(this.rbOverwrite);
			this.grpDupeHandeling.Enabled = false;
			this.grpDupeHandeling.Location = new System.Drawing.Point(20, 152);
			this.grpDupeHandeling.Name = "grpDupeHandeling";
			this.grpDupeHandeling.Size = new System.Drawing.Size(353, 52);
			this.grpDupeHandeling.TabIndex = 4;
			this.grpDupeHandeling.TabStop = false;
			this.grpDupeHandeling.Text = "Duplicate Handling";
			// 
			// rbCancel
			// 
			this.rbCancel.AutoSize = true;
			this.rbCancel.Enabled = false;
			this.rbCancel.Location = new System.Drawing.Point(289, 21);
			this.rbCancel.Name = "rbCancel";
			this.rbCancel.Size = new System.Drawing.Size(58, 17);
			this.rbCancel.TabIndex = 0;
			this.rbCancel.TabStop = true;
			this.rbCancel.Text = "Cancel";
			this.rbCancel.UseVisualStyleBackColor = true;
			this.rbCancel.CheckedChanged += new System.EventHandler(this.rbCancel_CheckedChanged);
			// 
			// rbSkip
			// 
			this.rbSkip.AutoSize = true;
			this.rbSkip.Enabled = false;
			this.rbSkip.Location = new System.Drawing.Point(209, 21);
			this.rbSkip.Name = "rbSkip";
			this.rbSkip.Size = new System.Drawing.Size(46, 17);
			this.rbSkip.TabIndex = 3;
			this.rbSkip.TabStop = true;
			this.rbSkip.Text = "Skip";
			this.rbSkip.UseVisualStyleBackColor = true;
			this.rbSkip.CheckedChanged += new System.EventHandler(this.rbSkip_CheckedChanged);
			// 
			// rbRename
			// 
			this.rbRename.AutoSize = true;
			this.rbRename.Enabled = false;
			this.rbRename.Location = new System.Drawing.Point(110, 21);
			this.rbRename.Name = "rbRename";
			this.rbRename.Size = new System.Drawing.Size(65, 17);
			this.rbRename.TabIndex = 2;
			this.rbRename.TabStop = true;
			this.rbRename.Text = "Rename";
			this.rbRename.UseVisualStyleBackColor = true;
			this.rbRename.CheckedChanged += new System.EventHandler(this.rbRename_CheckedChanged);
			// 
			// rbOverwrite
			// 
			this.rbOverwrite.AutoSize = true;
			this.rbOverwrite.Enabled = false;
			this.rbOverwrite.Location = new System.Drawing.Point(6, 21);
			this.rbOverwrite.Name = "rbOverwrite";
			this.rbOverwrite.Size = new System.Drawing.Size(70, 17);
			this.rbOverwrite.TabIndex = 1;
			this.rbOverwrite.TabStop = true;
			this.rbOverwrite.Text = "Overwrite";
			this.rbOverwrite.UseVisualStyleBackColor = true;
			this.rbOverwrite.CheckedChanged += new System.EventHandler(this.rbOverwrite_CheckedChanged);
			// 
			// cbHideDupeFileMessage
			// 
			this.cbHideDupeFileMessage.AutoSize = true;
			this.cbHideDupeFileMessage.Location = new System.Drawing.Point(20, 129);
			this.cbHideDupeFileMessage.Name = "cbHideDupeFileMessage";
			this.cbHideDupeFileMessage.Size = new System.Drawing.Size(164, 17);
			this.cbHideDupeFileMessage.TabIndex = 3;
			this.cbHideDupeFileMessage.Text = "Hide Duplicate File Message ";
			this.cbHideDupeFileMessage.UseVisualStyleBackColor = true;
			this.cbHideDupeFileMessage.CheckedChanged += new System.EventHandler(this.cbHideDupeFileMessage_CheckedChanged);
			// 
			// tbSourceFolder
			// 
			this.tbSourceFolder.Location = new System.Drawing.Point(93, 20);
			this.tbSourceFolder.Name = "tbSourceFolder";
			this.tbSourceFolder.Size = new System.Drawing.Size(191, 20);
			this.tbSourceFolder.TabIndex = 0;
			this.tbSourceFolder.TextChanged += new System.EventHandler(this.tbSourceFolder_TextChanged);
			this.tbSourceFolder.Validating += new System.ComponentModel.CancelEventHandler(this.tbSourceFolder_Validating);
			this.tbSourceFolder.Validated += new System.EventHandler(this.tbSourceFolder_Validated);
			// 
			// lblSourceFolder
			// 
			this.lblSourceFolder.AutoSize = true;
			this.lblSourceFolder.Location = new System.Drawing.Point(17, 24);
			this.lblSourceFolder.Name = "lblSourceFolder";
			this.lblSourceFolder.Size = new System.Drawing.Size(73, 13);
			this.lblSourceFolder.TabIndex = 14;
			this.lblSourceFolder.Text = "Source Folder";
			// 
			// btnSourceFolderBrowse
			// 
			this.btnSourceFolderBrowse.Location = new System.Drawing.Point(334, 19);
			this.btnSourceFolderBrowse.Name = "btnSourceFolderBrowse";
			this.btnSourceFolderBrowse.Size = new System.Drawing.Size(25, 23);
			this.btnSourceFolderBrowse.TabIndex = 9;
			this.btnSourceFolderBrowse.Text = "...";
			this.btnSourceFolderBrowse.UseVisualStyleBackColor = true;
			this.btnSourceFolderBrowse.Click += new System.EventHandler(this.btnSourceFolderBrowse_Click);
			// 
			// btnTargetFolderBrowse
			// 
			this.btnTargetFolderBrowse.Location = new System.Drawing.Point(334, 44);
			this.btnTargetFolderBrowse.Name = "btnTargetFolderBrowse";
			this.btnTargetFolderBrowse.Size = new System.Drawing.Size(25, 23);
			this.btnTargetFolderBrowse.TabIndex = 11;
			this.btnTargetFolderBrowse.Text = "...";
			this.btnTargetFolderBrowse.UseVisualStyleBackColor = true;
			this.btnTargetFolderBrowse.Click += new System.EventHandler(this.btnTargetFolderBrowse_Click);
			// 
			// lblTargetFolder
			// 
			this.lblTargetFolder.AutoSize = true;
			this.lblTargetFolder.Location = new System.Drawing.Point(20, 50);
			this.lblTargetFolder.Name = "lblTargetFolder";
			this.lblTargetFolder.Size = new System.Drawing.Size(70, 13);
			this.lblTargetFolder.TabIndex = 15;
			this.lblTargetFolder.Text = "Target Folder";
			// 
			// tbTargetFolder
			// 
			this.tbTargetFolder.Location = new System.Drawing.Point(93, 46);
			this.tbTargetFolder.Name = "tbTargetFolder";
			this.tbTargetFolder.Size = new System.Drawing.Size(191, 20);
			this.tbTargetFolder.TabIndex = 1;
			this.tbTargetFolder.TextChanged += new System.EventHandler(this.tbTargetFolder_TextChanged);
			this.tbTargetFolder.Validating += new System.ComponentModel.CancelEventHandler(this.tbTargetFolder_Validating);
			this.tbTargetFolder.Validated += new System.EventHandler(this.tbTargetFolder_Validated);
			// 
			// lblFileExtensions
			// 
			this.lblFileExtensions.AutoSize = true;
			this.lblFileExtensions.Location = new System.Drawing.Point(13, 91);
			this.lblFileExtensions.Name = "lblFileExtensions";
			this.lblFileExtensions.Size = new System.Drawing.Size(77, 13);
			this.lblFileExtensions.TabIndex = 16;
			this.lblFileExtensions.Text = "File Extensions";
			// 
			// tbFileExtensions
			// 
			this.tbFileExtensions.Location = new System.Drawing.Point(93, 88);
			this.tbFileExtensions.Name = "tbFileExtensions";
			this.tbFileExtensions.Size = new System.Drawing.Size(191, 20);
			this.tbFileExtensions.TabIndex = 2;
			this.tbFileExtensions.TextChanged += new System.EventHandler(this.tbFileExtensions_TextChanged);
			this.tbFileExtensions.Validating += new System.ComponentModel.CancelEventHandler(this.tbFileExtensions_Validating);
			this.tbFileExtensions.Validated += new System.EventHandler(this.tbFileExtensions_Validated);
			// 
			// btnFileExtensionsClear
			// 
			this.btnFileExtensionsClear.Location = new System.Drawing.Point(290, 86);
			this.btnFileExtensionsClear.Name = "btnFileExtensionsClear";
			this.btnFileExtensionsClear.Size = new System.Drawing.Size(40, 23);
			this.btnFileExtensionsClear.TabIndex = 12;
			this.btnFileExtensionsClear.Text = "Clear";
			this.btnFileExtensionsClear.UseVisualStyleBackColor = true;
			this.btnFileExtensionsClear.Click += new System.EventHandler(this.btnFileExtensionsClear_Click);
			// 
			// btnFileExtensionsFileSelect
			// 
			this.btnFileExtensionsFileSelect.Location = new System.Drawing.Point(334, 86);
			this.btnFileExtensionsFileSelect.Name = "btnFileExtensionsFileSelect";
			this.btnFileExtensionsFileSelect.Size = new System.Drawing.Size(39, 23);
			this.btnFileExtensionsFileSelect.TabIndex = 13;
			this.btnFileExtensionsFileSelect.Text = "Load";
			this.btnFileExtensionsFileSelect.UseVisualStyleBackColor = true;
			this.btnFileExtensionsFileSelect.Click += new System.EventHandler(this.btnFileExtensionsFileSelect_Click);
			// 
			// btnTargetClear
			// 
			this.btnTargetClear.Location = new System.Drawing.Point(290, 44);
			this.btnTargetClear.Name = "btnTargetClear";
			this.btnTargetClear.Size = new System.Drawing.Size(40, 23);
			this.btnTargetClear.TabIndex = 10;
			this.btnTargetClear.Text = "Clear";
			this.btnTargetClear.UseVisualStyleBackColor = true;
			this.btnTargetClear.Click += new System.EventHandler(this.btnTargetClear_Click);
			// 
			// btnSourceFolderClear
			// 
			this.btnSourceFolderClear.Location = new System.Drawing.Point(290, 19);
			this.btnSourceFolderClear.Name = "btnSourceFolderClear";
			this.btnSourceFolderClear.Size = new System.Drawing.Size(40, 23);
			this.btnSourceFolderClear.TabIndex = 8;
			this.btnSourceFolderClear.Text = "Clear";
			this.btnSourceFolderClear.UseVisualStyleBackColor = true;
			this.btnSourceFolderClear.Click += new System.EventHandler(this.btnSourceFolderClear_Click);
			// 
			// errorProviderSourceFolder
			// 
			this.errorProviderSourceFolder.ContainerControl = this;
			// 
			// errorProviderTargetFolder
			// 
			this.errorProviderTargetFolder.ContainerControl = this;
			// 
			// errorProviderDuplicateHandeling
			// 
			this.errorProviderDuplicateHandeling.ContainerControl = this;
			// 
			// errorProviderFileExtensions
			// 
			this.errorProviderFileExtensions.ContainerControl = this;
			// 
			// ConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(383, 244);
			this.Controls.Add(this.btnSourceFolderClear);
			this.Controls.Add(this.btnTargetClear);
			this.Controls.Add(this.cbHideDupeFileMessage);
			this.Controls.Add(this.btnFileExtensionsFileSelect);
			this.Controls.Add(this.btnFileExtensionsClear);
			this.Controls.Add(this.tbFileExtensions);
			this.Controls.Add(this.lblFileExtensions);
			this.Controls.Add(this.btnTargetFolderBrowse);
			this.Controls.Add(this.lblTargetFolder);
			this.Controls.Add(this.tbTargetFolder);
			this.Controls.Add(this.btnSourceFolderBrowse);
			this.Controls.Add(this.lblSourceFolder);
			this.Controls.Add(this.tbSourceFolder);
			this.Controls.Add(this.grpDupeHandeling);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Name = "ConfigForm";
			this.Text = "Push Configuration";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigForm_FormClosing);
			this.Load += new System.EventHandler(this.ConfigForm_Load);
			this.grpDupeHandeling.ResumeLayout(false);
			this.grpDupeHandeling.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProviderSourceFolder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProviderTargetFolder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProviderDuplicateHandeling)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProviderFileExtensions)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox grpDupeHandeling;
		private System.Windows.Forms.CheckBox cbHideDupeFileMessage;
		private System.Windows.Forms.TextBox tbSourceFolder;
		private System.Windows.Forms.Label lblSourceFolder;
		private System.Windows.Forms.Button btnSourceFolderBrowse;
		private System.Windows.Forms.Button btnTargetFolderBrowse;
		private System.Windows.Forms.Label lblTargetFolder;
		private System.Windows.Forms.TextBox tbTargetFolder;
		private System.Windows.Forms.RadioButton rbSkip;
		private System.Windows.Forms.RadioButton rbRename;
		private System.Windows.Forms.Label lblFileExtensions;
		private System.Windows.Forms.TextBox tbFileExtensions;
		private System.Windows.Forms.Button btnFileExtensionsClear;
		private System.Windows.Forms.Button btnFileExtensionsFileSelect;
		private System.Windows.Forms.Button btnTargetClear;
		private System.Windows.Forms.Button btnSourceFolderClear;
		private System.Windows.Forms.ErrorProvider errorProviderSourceFolder;
		private System.Windows.Forms.ErrorProvider errorProviderTargetFolder;
		private System.Windows.Forms.RadioButton rbCancel;
		private System.Windows.Forms.RadioButton rbOverwrite;
		private System.Windows.Forms.ErrorProvider errorProviderDuplicateHandeling;
		private System.Windows.Forms.ErrorProvider errorProviderFileExtensions;
	}
}