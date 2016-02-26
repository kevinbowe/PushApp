using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
//---
using Itenso.Configuration;
using System.ComponentModel;
using System.Threading;
//---
using System.Text;
//---
using System.Text.RegularExpressions;

namespace Push
{
	public partial class MainForm : Form
	{

		private ListViewColumnSorter listViewColumnSorter;
		private readonly FormSettings formSettings;
		public AppSettings appSettings;
		Size savedMainFormSize;
		public enum ctrlStatus { Enable=1 /*true*/, Disable=0 /*false*/ };

		// The minimum window size which will hide the source and target and shrink the status listbox...
		private Size MinHideDetailSize = new Size(295, 161);
		private Size MinShowDetailSize = new Size(700, 350);

		public string ignorePattern;


		public void bgProgressChangedEventHandler(object sender, ProgressChangedEventArgs e)
		{
			toolStripLblProgress.Visible = true;
			toolStripProgressBar.Visible = true;
			toolStripLblProgress.Text = "Processing... "+ e.ProgressPercentage.ToString() + "%";

			toolStripProgressBar.Value = e.ProgressPercentage;
		} // END_METHOD


		public void bgRunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e)
		{

			if (e.Error != null)
			{
				MessageBox.Show(
					e.Error.Message,
					"Exception:",
					System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Information);
			}
			else
			{
				UpdateStatus((Tuple<Helper.commandResult, int, int>)e.Result);
			}
	
			// Update Source & Target Listboxes...
			LoadListView(lvSource, appSettings.SourcePath, ignorePattern);
			LoadListView(lvTarget, appSettings.TargetPath);
			
			FitListView(lvSource);
			FitListView(lvTarget);

			// Enable Controls...
			SetControlStatus(ctrlStatus.Enable);
		} // END_METHOD


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

			listViewColumnSorter = new ListViewColumnSorter();
			this.lvSource.ListViewItemSorter = listViewColumnSorter;
			this.lvTarget.ListViewItemSorter = listViewColumnSorter;
		} // END_CTOR


		private void InitControls()
		{
			if (!ValidateAppSettings(appSettings))
			{
				ConfigForm configFormDialog = new ConfigForm();

				// Copy the current settings into the Configuration form...
				configFormDialog.appSettings = appSettings;
				configFormDialog.StartPosition = FormStartPosition.CenterParent;
				configFormDialog.Text = "Push Application Setup";

				if (configFormDialog.ShowDialog(this) == DialogResult.Cancel)
				{
					// If we get here, exit the application imeadiatly...
					this.Close();
					return;
				}

				// Copy settings...
				appSettings = configFormDialog.appSettings;

				configFormDialog.Dispose();
			}

			// Build the Ignore File regular expression pattern...
			ignorePattern = BuildIgnorePattern();

			// Init Controls...
			lblStatus1_1.Text = string.Empty;
			lblStatus1_2.Text = string.Empty;
			lblStatus2_2.Text = string.Empty;

			lblStatus1_1.Visible = false;
			lblStatus1_2.Visible = false;
			lblStatus2_2.Visible = false;

			toolStripProgressBar.Visible = false;
			toolStripLblProgress.Visible = false;

			// Update the form properties to the last used...
			UpdateControls();
		}


		private bool ValidateAppSettings(AppSettings appSettings)
		{
			if (Helper.AppSettingsEmptyOrNull(appSettings))
				// Invalid
				return false;

			// If we get here, there is data in appSettings...
			// Validate the data using the existing ConfigForm validation code...

			// Verify the ExePath...
			if (!Directory.Exists(appSettings.ExePath))
			{
				appSettings.ExePath = null;
				// Invalid
				return false;
			}

			// NOTE: We are not going to show the form. We are only going to use the
			//		 validation code.
			ConfigForm configFormDialog = new ConfigForm();
			configFormDialog.appSettings = appSettings;
			configFormDialog.ConfigForm_Load(null, null);

			// ValidateConfigForm return: True = valid || False = invalid;
			bool isValid = configFormDialog.ValidateConfigForm();

			// Make sure the form object is completely cleaned up...
			configFormDialog.Close();
			configFormDialog.Dispose();

			// true = invalid || false = valid
			return isValid;
		}


		private string BuildIgnorePattern()
		{
			StringBuilder sb = new StringBuilder();

			try
			{
				// Check the ExePath...
				if (appSettings.ExePath == null)
				{
					FileInfo exePath = new FileInfo("Push.exe");
					appSettings.ExePath = exePath.DirectoryName;
				}

				// Verify that the PushIgnore.txt file exists...
				if (!File.Exists(appSettings.ExePath + @"\Config\PushIgnore.txt"))
				{
					// If we get here, the PushIgnore.txt file does NOT exist...
					File.Copy(appSettings.ExePath + @"\Config\PushIgnore.Default", appSettings.ExePath + @"\Config\PushIgnore.txt", true);
				}

				List<string> ignoreList = new List<string>();

				using (StreamReader sr = new StreamReader(appSettings.ExePath + @"\Config\PushIgnore.txt"))
				{
					while (sr.Peek() >= 0)
					{
						string s = sr.ReadLine();

						// Discard comments and empty lines...
						if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s) || s[0] == '#')
							continue;

						// Split any string that has [space], [comma], [semicolon] tokens dividing multiple filter values...
						string[] splitArray = s.Split(
							new char[] {',', ';', ' '},
							StringSplitOptions.RemoveEmptyEntries);

						// Add result to working List<string>...
						ignoreList.AddRange(splitArray);
					}

					foreach(string ignoreItem in ignoreList)
					{
						// Swap forward and backward slashes...
						string s = ignoreItem.Replace(@"/", @"\");

						// Replace Global Find characters with equivelent RegEx expressions...
						s = s.Replace(@"\", @"\\");  // Escape all slashes - Only Folders are effected...
						s = s.Replace(".", @"\.");	// Escape periods
						s = s.Replace(@"*", ".*");	// Convert *
						s = s.Replace(@"?", ".");	// Convert ?
						// Add token...					
						sb.Append(sb.Length == 0 ? s : '|' + s);
					}
				}
			}
			catch
			{
				MessageBox.Show("Error initializing PushIgnore.", "Error:");
				System.Environment.Exit(1000);
			}

			// Add valid Prefix and Suffix to regex pattern...
			sb.Append(')');
			sb.Insert(0, "(");

			return sb.ToString();
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

				LoadListView(lvSource, appSettings.SourcePath, ignorePattern);
				LoadListView(lvTarget, appSettings.TargetPath);

				FitListView(lvSource);
				FitListView(lvTarget);
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
			LoadListView(lvSource, appSettings.SourcePath, ignorePattern);
			LoadListView(lvTarget, appSettings.TargetPath);

			FitListView(lvSource);
			FitListView(lvTarget);
		} // END_METHOD

		
		// Push Button...
		private void picBoxPush_Click(object sender, EventArgs e)
		{
			// Hide the status listboxes...
			lblStatus1_1.Visible = false;
			lblStatus1_2.Visible = false;
			lblStatus2_2.Visible = false;
			//---
			toolStripProgressBar.Visible = false;
			toolStripLblProgress.Visible = false;

			// Disable Controls...
			SetControlStatus(ctrlStatus.Disable);

			new CopyFile().CopyFiles(this);
		}


		private void SetControlStatus(ctrlStatus ctrlStatus)
		{
			bool status = Convert.ToBoolean(ctrlStatus);
			picBxPush.Enabled = status;

			if (status)
				picBxPush.Image = Properties.Resources.Green_Button1;
			else
				picBxPush.Image = Properties.Resources.Grey_Button1;
			
			toolStripBtnPush.Enabled = status;
			toolStripBtnRefresh.Enabled = status;
			toolStripBtnConfig.Enabled = status;
		} // END_METHOD


		private void UpdateStatus(Tuple<Helper.commandResult, int, int> copyFileResult)
		{
			
			List<string> statusList = new List<string>();
			//---
			switch (copyFileResult.Item1)
			{
				case Helper.commandResult.Cancel:
					statusList.Add(string.Format("Duplicates Found", copyFileResult.Item2));
					statusList.Add(string.Format("Copy Canceled", copyFileResult.Item3));
					break;

				case Helper.commandResult.Overwrite:
					statusList.Add(string.Format("{0} Files Overwritten", copyFileResult.Item2));
					break;

				case Helper.commandResult.Rename:
					statusList.Add(string.Format("{0} Files Copied", copyFileResult.Item2));
					statusList.Add(string.Format("{0} Files Renamed", copyFileResult.Item3));
					break;

				case Helper.commandResult.Skip:
					statusList.Add(string.Format("{0} Files Copied", copyFileResult.Item2));
					statusList.Add(string.Format("{0} Files Skipped", copyFileResult.Item3));
					break;
				case Helper.commandResult.Fail:
					statusList.Add("No Files to Copy");
					break;
			}

			if (statusList.Count <= 1)
			{
				lblStatus1_1.Text = statusList[0];
				lblStatus1_1.Visible = true;
			}
			else
			{
				lblStatus1_2.Text = statusList[0];
				lblStatus2_2.Text = statusList[1];
				//---
				lblStatus1_2.Visible = true;
				lblStatus2_2.Visible = true;
			}
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
		private bool LoadListView(ListView DestinationListView, string DestinationPath, string ignorePattern = null)
		{

			if (!Directory.Exists(DestinationPath)) return false;

			DestinationListView.Items.Clear();

			List<string> fileSourceArrayList = new List<string>();

			// Fetch all of the subfolders in the root destination folder...
			DirectoryInfo directoryInfo = new DirectoryInfo(DestinationPath);
			DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
			foreach (DirectoryInfo dirInfo in directoryInfoArray)
			{
				if (!string.IsNullOrEmpty(ignorePattern) && Regex.IsMatch(dirInfo.FullName + "\\\\", ignorePattern))
					continue;
				
				fileSourceArrayList.Add(dirInfo.FullName);
			}

			// Fetch all of the file extensions that shouldbe displayed...
			List<string> fileExtensionList = new List<string>();
			fileExtensionList.AddRange(Helper.LoadFileExtensions(appSettings));

			// Fetch all of the files in the root destination based on the file extension list...
			foreach (string FileExtension in fileExtensionList)
			{
				string[] fileArray = Directory.GetFiles(DestinationPath, FileExtension);

				foreach (string file in fileArray)
				{
					// Strip the path portion of the file name...
					string f = Path.GetFileName(file);

					if (!string.IsNullOrEmpty(ignorePattern) && Regex.IsMatch(f, ignorePattern))
						continue;

					fileSourceArrayList.Add(file);
				}
			} // END_OUTER_FOREACH

			// Load the list view with the folders and files that have been fetched...
			string fileName, friendlyFileType, friendlyFileSize, fileDate;
			foreach (string file in fileSourceArrayList)
			{
				fileName = friendlyFileType = friendlyFileSize = fileDate = string.Empty;

				// Is the current 'file' a file or a folder??
				FileAttributes fileAttributes = File.GetAttributes(file);
				if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
				{
					fileName = Path.GetFileNameWithoutExtension(file);
					friendlyFileType = "File folder";
					fileDate = File.GetCreationTime(file).ToString("MM/dd/yyyy h:mm tt");
				}
				else
				{
					FileInfo fileInfo = new FileInfo(file);
					//
					fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
					friendlyFileType = FileTypes.GetFileTypeDescription(file);
					friendlyFileSize = FileTypes.StrFormatByteSize(fileInfo.Length);
					fileDate = File.GetCreationTime(file).ToString("MM/dd/yyyy h:mm tt");
				}

				ListViewItem itemArray = new ListViewItem(new string[] { fileName, friendlyFileType, friendlyFileSize, fileDate });
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
			picBoxPush_Click(sender, e);
		} // END_METHOD


		private void toolStripBtnRefresh_Click(object sender, EventArgs e)
		{
			lblStatus1_1.Text = string.Empty;
			lblStatus1_2.Text = string.Empty;
			lblStatus2_2.Text = string.Empty;
			//---
			toolStripProgressBar.Visible = false;
			toolStripLblProgress.Visible = false;
			//---
			LoadListView(lvSource, appSettings.SourcePath, ignorePattern);
			LoadListView(lvTarget, appSettings.TargetPath);

			FitListView(lvSource);
			FitListView(lvTarget);
		} // END_METHOD


		private void tooStripBtnConfig_Click(object sender, EventArgs e)
		{
			LoadConfigurationDialog();

			// Update the MainForm controls...
			UpdateControls();

		} // END_METHOD

		#endregion


		#region [ DEBUG BUTTONS ]


		// Hot-Key ZERO
		private void DEBUG_Red()
		{
			var dirDic = new Dictionary<string,string>()
			{
				{ "SRC_DATA", @"C:\Program Files\Push\DEBUG\DATA\SRC"},
				{"DEST_DATA", @"C:\Program Files\Push\DEBUG\DATA\DEST"},
				{      "SRC", @"C:\Program Files\Push\DEBUG\SRC"},
				{     "DEST", @"C:\Program Files\Push\DEBUG\DEST"}
			};

			// Make sure the required degug folders exist... 
			if (!IsValidDebugDependencies(dirDic))
				return;
			
			// Delete any existing data in the source and target filders...
			DEBUG_InitFolders(dirDic);

			DEBUG_LoadSubFolderTestData(dirDic["SRC_DATA"], dirDic["SRC"]);
			DEBUG_LoadSubFolderTestData(dirDic["DEST_DATA"], dirDic["DEST"]);

			//-----------------------------------------------------------------
			//	NOTE:
			//	The LoadListView and FitListView methods use the appSettings 
			//	properties --NOT-- the DEBUG directories...

			// Hydrate the Source and Target Listboxes
			LoadListView(lvSource, appSettings.SourcePath, ignorePattern);
			LoadListView(lvTarget, appSettings.TargetPath);

			FitListView(lvSource);
			FitListView(lvTarget);

		} // END_METHOD


		private bool IsValidDebugDependencies(Dictionary<string,string> dirDic)
		{
			// Validate the test data folders... 
			foreach (string folder in dirDic.Values)
			{
				if (Directory.Exists(folder)) continue;

				// If we get here, one of the folders doesn't exist...
				MessageBox.Show(string.Format(
					"This folder is required for Debug Hot-Keys to function. \n\t{0}", folder),
					"Debug Hot-Key Canceled");
				return false;
			}
			return true;
		} // END_METHOD


		private void DEBUG_InitFolders(Dictionary<string, string> dirDic)
		{
			// Delete all files and subfolders in the source folder...
			DirectoryInfo directoryInfo = new DirectoryInfo(dirDic["SRC"]);

			foreach (System.IO.FileInfo file in directoryInfo.GetFiles())
				file.Delete();

			foreach (System.IO.DirectoryInfo subDirectory in directoryInfo.GetDirectories())
				subDirectory.Delete(true);
			
			// Delete all files and subfolders in the folder...
			directoryInfo = new DirectoryInfo(dirDic["DEST"]);

			foreach (System.IO.FileInfo file in directoryInfo.GetFiles())
				file.Delete();

			foreach (System.IO.DirectoryInfo subDirectory in directoryInfo.GetDirectories())
				subDirectory.Delete(true);

		} // END_METHOD

		
		private void DEBUG_LoadSubFolderTestData(string TestDataPath, string DestinationPath)
		{
			DirectoryInfo dirInfo_TestData = new DirectoryInfo(TestDataPath);
			DirectoryInfo dirInfo_Destination = new DirectoryInfo(DestinationPath);

			CopyAll(dirInfo_TestData, dirInfo_Destination);
		} // END_METHOD


		public static void CopyAll(DirectoryInfo dirInfo_TestData, DirectoryInfo dirInfo_Destination)
		{
			Directory.CreateDirectory(dirInfo_Destination.FullName);

			// Copy each file into the new directory.
			foreach (FileInfo fileInfo_TestData in dirInfo_TestData.GetFiles())
			{
				fileInfo_TestData.CopyTo(Path.Combine(dirInfo_Destination.FullName, fileInfo_TestData.Name), true);
			}

			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo dirInfo_TestDataSubDir in dirInfo_TestData.GetDirectories())
			{
				DirectoryInfo dirInfo_nextDestinationtSubDir = dirInfo_Destination.CreateSubdirectory(dirInfo_TestDataSubDir.Name);
				CopyAll(dirInfo_TestDataSubDir, dirInfo_nextDestinationtSubDir);
			}
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
					// Enter 0 (ZERO)...
					case (Keys.ShiftKey | Keys.Space):
						DEBUG_Red();
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
				lblStatus1_1.Text = string.Empty;
				lblStatus1_2.Text = string.Empty;
				lblStatus2_2.Text = string.Empty;
				//--
				LoadListView(lvSource, appSettings.SourcePath, ignorePattern);
				LoadListView(lvTarget, appSettings.TargetPath);

				FitListView(lvSource);
				FitListView(lvTarget);
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		} // END_METHOD
	
		#endregion


		private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			ListView listView = (ListView)sender;

			// Determine if clicked column is already the column that is being sorted.
			if (e.Column == listViewColumnSorter.SortColumn)
			{
				// Reverse the current sort direction for this column.
				if (listViewColumnSorter.Order == SortOrder.Ascending)
				{
					listViewColumnSorter.Order = SortOrder.Descending;
				}
				else
				{
					listViewColumnSorter.Order = SortOrder.Ascending;
				}
			}
			else
			{
				// Set the column number that is to be sorted; default to ascending.
				listViewColumnSorter.SortColumn = e.Column;
				listViewColumnSorter.Order = SortOrder.Ascending;
			}

			// Perform the sort with these new sort options.
			listView.Sort();

		} // END_METHOD


		private void MainForm_Shown(object sender, EventArgs e)
		{
			FitListView(lvSource);
			FitListView(lvTarget);
		}  //END_METHOD


		private void FitListView(ListView listView)
		{
			/*	DESIGN POINT:
			 *	-1 = resize the column to the length of the longet value in the column.
			 *	-2 = resize the column to the length of the column header.		
			 *
			 *	NOTE: Keep this code for a while just in case It may get recycled..
			 */

			//if (listView.Items.Count == 0)
			//{
			//	// If we get here, there are no entries in the source ListView...

			//	// Set the width of the Type, and Size so they will be at least the width of the header...
			//	listView.Columns[1].Width = -2;
			//	listView.Columns[2].Width = -2;
			//	listView.Columns[3].Width = -2;
			//}
			//else
			//{
			//	// If we get herem there are entries in the ListView...

			//	// Set the width of the Type, Size, and Date so they will be at least the width of the longest item in the columnr...
			//	listView.Columns[1].Width = -1;
			//	listView.Columns[2].Width = -1;
			//	listView.Columns[3].Width = -1;
			//}

			// Set the min column width for File Type, Size and Date...
			listView.Columns[1].Width = listView.Columns[1].Width < 74 ? 74 : listView.Columns[1].Width;
			listView.Columns[2].Width = listView.Columns[2].Width < 60 ? 60 : listView.Columns[2].Width;
			listView.Columns[3].Width = listView.Columns[3].Width < 118 ? 118 : listView.Columns[3].Width;

			// Set the File Name to 'fill' the rest of the List View...
			// Force a minimum column width...
			int otherColumnWidths = listView.Columns[1].Width + listView.Columns[2].Width + listView.Columns[3].Width;
			int tweek = 25;
			int ColZeroWidth = (listView.Width - otherColumnWidths) - tweek;
			int minColZeroWidth = 50;
			if (ColZeroWidth < minColZeroWidth)
				listView.Columns[0].Width = minColZeroWidth;
			else
				listView.Columns[0].Width = ColZeroWidth;
		} // END_METHOD


		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
		} // END_METHOD


		private void splitContainerDetails_SplitterMoved(object sender, SplitterEventArgs e)
		{
		} // END_METHOD


		private void lvSource_SizeChanged(object sender, EventArgs e)
		{
		} // END_METHOD


		private void pictureBox1_Click(object sender, EventArgs e)
		{
			lvSource.Columns[0].Width = -2;
		} // END_METHOD


		private void pictureBox2_Click(object sender, EventArgs e)
		{
			lvTarget.Columns[0].Width = -2;
		} // END_METHOD

	} // END_CLASS


} // END_NAMESPACE
