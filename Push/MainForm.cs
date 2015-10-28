using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
//---
using Itenso.Configuration;

namespace Push
{
	public partial class MainForm : Form
	{
		private readonly FormSettings formSettings;
		public AppSettings appSettings;
		Size savedMainFormSize;

		// The minimum window size which will hide the source and target and shrink the status listbox...
		private Size MinHideDetailSize = new Size(400, 161);
		
		// The minimum window size so the source and target can not be hidden when resizing the window...
		private Size MinShowDetailSize = new Size(764, 286);


		#region [ MainForm Constructor + Support ]

		// Main Form Ctor...
		public MainForm(AppSettings appSettings)
		{
			InitializeComponent();

			// Create settings group...
			//		FormSettings contains all of the Window properties...
			//		It does NOT contain the controls or the app configuration data...
			formSettings = new FormSettings(this);

			// Enable Auto-Save...
			formSettings.SaveOnClose = true;

			// Copy the appSettings argument into this forms appSettings property...
			this.appSettings = appSettings;

			// Set the default form properties and then update them with the last used properties...
			InitControls();
		} // END_CTOR


		private void InitControls()
		{

			// Test to see if any of the required properties are missing...
			if (Helper.IsAppSettingsEmptyOrNull(appSettings))
			{
				ConfigForm configFormDialog = new ConfigForm();

				// Copy the current settings into the Configuration form...
				configFormDialog.appSettings = appSettings;
				configFormDialog.StartPosition = FormStartPosition.CenterParent;
				configFormDialog.Text = "Push Application Setup";

				if (configFormDialog.ShowDialog(this) == DialogResult.Cancel)
					// If we get here, exit the application imeadiatly...
					this.Close();

				// Copy settings...
				appSettings = configFormDialog.appSettings;

				configFormDialog.Dispose();
			}

			// Update the form properties to the last used...
			UpdateControls();
		} // END_METHOD


		private void UpdateControls()
		{
			lblSourcePath.Text = appSettings.SourcePath.ToUpperInvariant();
			lblTargetPath.Text = appSettings.TargetPath.ToUpperInvariant();
			lblFileExtensionFilterString.Text = appSettings.FileExtensionFilter.ToUpperInvariant();
			lblDupeFileActionText.Text = GetDuplicateFileAction();
			ttDetails.ToolTipTitle = "Details";
			ttDetails.ToolTipIcon = ToolTipIcon.Info;
			ttDetails.IsBalloon = true;

			string ttDetailsText = string.Format(
					"Source Folder = {0}\n" +
					"Target Folder = {1}\n" +
					"FileFilter = {2}\n" +
					"Duplicate File Action = {3}",
					appSettings.SourcePath.ToUpperInvariant(),
					appSettings.TargetPath.ToUpperInvariant(),
					appSettings.FileExtensionFilter.ToUpperInvariant(),
					GetDuplicateFileAction()
					);
			ttDetails.SetToolTip(lblShowHide, ttDetailsText);


			if (appSettings.ShowDetails.GetValueOrDefault(true))
			{
				picBxShowHide.Image = global::Push.Properties.Resources.Control_Collapser1;
				lblShowHide.Text = "Hide Details";
				pnlDetails.Visible = true;
				formSettings.Form.MinimumSize = MinimumSize = MinShowDetailSize;

				LoadListView(lvSource, appSettings.SourcePath);
				LoadListView(lvTarget, appSettings.TargetPath);
			}
			else
			{
				picBxShowHide.Image = global::Push.Properties.Resources.Control_Expander1;
				lblShowHide.Text = "Show Details";
				pnlDetails.Visible = false;
				formSettings.Form.MinimumSize = MinimumSize = MaximumSize = MinHideDetailSize;
			}
		} // END_METHOD


		private string GetDuplicateFileAction()
		{
			return !(bool)appSettings.HideDupeMessage ? "MANUAL" : appSettings.DuplicateFileAction.ToUpperInvariant();
		} // END_METHOD

		#endregion
	

		// Load MainForm Data...
		private void MainForm_Load(object sender, EventArgs e)
		{
			// Disable the Maximize control on the form...
			this.MaximizeBox = false;

			// Hydrate the Source and Target Listboxes
			LoadListView(lvSource, appSettings.SourcePath);
			LoadListView(lvTarget, appSettings.TargetPath);
		} // END_METHOD

		
		// Push Button...
		private void picBoxPush_Click(object sender, EventArgs e)
		{
			// Init Controls...
			lbStatus.Items.Clear();

			Tuple<Helper.commandResult, int, int> r = new CopyFiles().CopyAction(this);

			string status = string.Empty;
			switch (r.Item1)
			{
				case Helper.commandResult.Cancel:
					status = "Copy Canceled";
					break;

				case Helper.commandResult.Overwrite:
					status = string.Format("Overwrite={0}", r.Item2);
					break;

				case Helper.commandResult.Rename: 
					status = string.Format("Copy={0} & Renamed={1}", r.Item2, r.Item3);
					break;

				case Helper.commandResult.Skip: 
					status = string.Format("Copy={0} & Skip={1}", r.Item2, r.Item3);
					break;
			}
			
			lbStatus.Items.Add(status);
			lbStatus.Update();

			// Update Source & Target Listboxes...
			LoadListView(lvSource, appSettings.SourcePath);
			LoadListView(lvTarget, appSettings.TargetPath);	
		
		
		} // END_METHOD


		// Show-Hide ListViews...
		private void picBoxShowHide_Click(object sender, EventArgs e)
		{
			if (splitContainerDetails.Visible)
				{
				// If we get here, hide the source and target ListViews...

				picBxShowHide.Image = global::Push.Properties.Resources.Control_Expander1;
				lblShowHide.Text = "Show Details";
				appSettings.ShowDetails = false;

				// Save the current window size...
				savedMainFormSize = Size;

				// Set the minimum window size which will hide the source and target and shrink the status listbox...
				//	This 'locks' the window so it can not be resized...
				Size = MaximumSize = MinimumSize = MinHideDetailSize;
				pnlDetails.Visible = false;
			}
			else
			{
				// If we get here,	restore the original windows size...

				picBxShowHide.Image = global::Push.Properties.Resources.Control_Collapser1;
				lblShowHide.Text = "Hide Details";
				appSettings.ShowDetails = true;

				// Reset the minimum size so the source and target can not be hidden when resizing the window...
				MinimumSize = MinShowDetailSize; 
				// Clear the maximum size so the user can resize the window...
				MaximumSize = new Size(); 
				// Restore the previous window size...
				Size = savedMainFormSize;
				pnlDetails.Visible = true;
			}
		} // END_METHOD


		// Load Source or Target ListView...
		private bool LoadListView(ListView DestinationListView, string DestinationPath)
		{
			// Fetch all of the files in the source filder...
			if (!Directory.Exists(DestinationPath)) return false;

			DestinationListView.Items.Clear();

			List<string> fileExtensionList = new List<string>();
			fileExtensionList.AddRange(Helper.LoadFileExtensions(appSettings));

			List<string> fileSourceArrayList = new List<string>();
			foreach (string FileExtension in fileExtensionList)
			{
				fileSourceArrayList.AddRange(Directory.GetFiles(DestinationPath, FileExtension));
			}

			foreach (string file in fileSourceArrayList)
			{
				FileInfo fileInfo = new FileInfo(file);
				//
				string fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
				string friendlyFileType = FileTypes.GetFileTypeDescription(file);
				string friendlyFileSize = FileTypes.StrFormatByteSize(fileInfo.Length);
				string fileDate = File.GetCreationTime(file).ToString("MM/dd/yyyy h:mm tt");

				ListViewItem itemArray = new ListViewItem(new string[] 
						{ fileName, friendlyFileType, friendlyFileSize, fileDate });
				DestinationListView.Items.Add(itemArray);

			} // END_FOREACH

			return true;
		} // END_METHOD


		// Configuration Dialog...
		private void LoadConfigurationDialog()
		{
			string s = string.Empty;
			ConfigForm configDialog = new ConfigForm();

			// Copy the current settings into the Configuration form...
			configDialog.appSettings = appSettings;
			configDialog.StartPosition = FormStartPosition.CenterParent;
			
			if (configDialog.ShowDialog(this) == DialogResult.OK) s = "OK";
			else s = "Cancel";

			// Copy settings...
			appSettings = configDialog.appSettings;

			configDialog.Dispose();
		} // END_METHOD


		#region [ TOOL STRIP ]

		private void toolStripBtnPush_Click(object sender, EventArgs e)
		{
			new CopyFiles().CopyAction(this);
		} // END_METHOD


		private void toolStripBtnRefresh_Click(object sender, EventArgs e)
		{
			LoadListView(lvSource, appSettings.SourcePath);
			LoadListView(lvTarget, appSettings.TargetPath);
		} // END_METHOD


		private void tooStripBtnConfig_Click(object sender, EventArgs e)
		{
			LoadConfigurationDialog();

			// Update the MainForm controls...
			UpdateControls();

		} // END_METHOD

		#endregion


		#region [ DEBUG BUTTONS ]

		// DEBUG Button - MistyRose -- Reset source and target data...
		private void btnDebug2_Click(object sender, EventArgs e)
		{
			DEBUG_MistyRose();
		} // END_METHOD


		public ListBox StatusControl { get { return this.lbStatus; } set { this.lbStatus = value; } }

		public ListView SourceControl { get { return this.lvSource; } set { this.lvSource = value; } }

		public ListView TargetControl { get { return this.lvTarget; } set { this.lvTarget = value; } }



		private void DEBUG_MistyRose()
		{
			DEBUG_InitFolders();

			DEBUG_LoadFolderTestData(@"C:\DEV_TESTDATA\Pictures", appSettings.SourcePath);
			DEBUG_LoadFolderTestData(@"C:\DEV_TESTDATA\TargetPictures", appSettings.TargetPath);

			//-----------------------------------------------------------------
			// Clear the status list box...
			lbStatus.Items.Clear();

			// Hydrate the Source and Target Listboxes
			LoadListView(lvSource, appSettings.SourcePath);
			LoadListView(lvTarget, appSettings.TargetPath);
		} // END_METHOD


		private void DEBUG_InitFolders()
		{
			// Generate a collection of ALL files, source and target, that must be deleted...
			List<string> fileList = new List<string>(Directory.GetFiles(appSettings.SourcePath));
			fileList.AddRange(new List<string>(Directory.GetFiles(appSettings.TargetPath)));
			
			foreach (string file in fileList) 
				File.Delete(file);
		} // END_METHOD

		
		private void DEBUG_LoadFolderTestData(string TestDataPath, string destinationPath)
		{
			List<string> fileExtensionList = new List<string>();
			fileExtensionList.AddRange(Helper.LoadFileExtensions(appSettings));

			List<string> fileTestDataList = new List<string>();
			foreach (string fileExtension in fileExtensionList)
			{
				fileTestDataList.AddRange(Directory.GetFiles(TestDataPath, fileExtension));
			} // END_FOREACH

			foreach (string s in fileTestDataList)
			{
				File.Copy(s, Path.Combine(destinationPath, Path.GetFileName(s)), true);
			}
		} // END_METHOD

		
		// DEBUG Button - Pale Green -- Reset source and target data...
		private void btnDebug4_Click(object sender, EventArgs e)
		{
			DEBUG_PaleGreen();
		} // END_METHOD


		private void DEBUG_PaleGreen()
		{
			DEBUG_InitFolders();

			DEBUG_LoadFolderTestData(@"C:\DEV_TESTDATA\Pictures", appSettings.SourcePath);
			DEBUG_LoadFolderTestData(@"C:\DEV_TESTDATA_2", appSettings.TargetPath);

			//-----------------------------------------------------------------
			// Clear the status list box...
			lbStatus.Items.Clear();

			// Hydrate the Source and Target Listboxes
			LoadListView(lvSource, appSettings.SourcePath);
			LoadListView(lvTarget, appSettings.TargetPath);
		} // END_METHOD


		// DEBUG Button - Powder Blue -- Reset source and target data...
		private void btnDebug5_Click(object sender, EventArgs e)
		{
			DEBUG_PowderBlue();
		} // END_METHOD


		private void DEBUG_PowderBlue()
		{
			DEBUG_InitFolders();

			DEBUG_LoadFolderTestData(@"C:\DEV_TESTDATA\Pictures", appSettings.SourcePath);
			DEBUG_LoadFolderTestData(@"C:\DEV_TESTDATA_1", appSettings.TargetPath);

			//-----------------------------------------------------------------
			// Clear the status list box...
			lbStatus.Items.Clear();

			// Hydrate the Source and Target Listboxes
			LoadListView(lvSource, appSettings.SourcePath);
			LoadListView(lvTarget, appSettings.TargetPath);
		} // END_METHOD


		private void DEBUG_Pink()
		{
			DEBUG_InitFolders();

			DEBUG_LoadFolderTestData(@"C:\DEV_TESTDATA_3\Source", appSettings.SourcePath);
			DEBUG_LoadFolderTestData(@"C:\DEV_TESTDATA_3\Target", appSettings.TargetPath);

			//-----------------------------------------------------------------
			// Clear the status list box...
			lbStatus.Items.Clear();

			// Hydrate the Source and Target Listboxes
			LoadListView(lvSource, appSettings.SourcePath);
			LoadListView(lvTarget, appSettings.TargetPath);
		} // END_METHOD



		#endregion		


		#region [ HOT-KEY // SHORT-CUTS ]

		private bool prefixSeen;


		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (prefixSeen)
			{
				//-------------------------------------------------------------
				// If we get here, we are processing the second character of the cord...

				switch (keyData)
				{
					// Enter 1 (ONE)...
					case (Keys.LButton | Keys.ShiftKey | Keys.Space):
						DEBUG_MistyRose();
						break;
					// Enter 2 (TWO)
					case (Keys.RButton | Keys.ShiftKey | Keys.Space):
						DEBUG_PaleGreen();
						break;
					// ENTER 3 (THREE)
					case (Keys.LButton | Keys.RButton | Keys.ShiftKey | Keys.Space):
						DEBUG_PowderBlue();
						break;

					// ENTER (FOUR)
					case (Keys.MButton | Keys.ShiftKey | Keys.Space):
						DEBUG_Pink();
						break;


					default:
						break;
				} // END_SWITCH

				prefixSeen = false;
				return true;
			}

			// Enter Ctrl+D // Debug prefix...
			if (keyData == (Keys.Control | Keys.D))
			{
				prefixSeen = true;
				return true;
			}

			// Enter Ctrl+R // Refresh...
			if (keyData == (Keys.Control | Keys.R))
			{
				LoadListView(lvSource, appSettings.SourcePath);
				LoadListView(lvTarget, appSettings.TargetPath);
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		} // END_METHOD
	
		#endregion


	} // END_CLASS


} // END_NAMESPACE
