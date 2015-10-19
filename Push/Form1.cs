using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
//---
using PSTaskDialog;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
//---
using Itenso.Configuration;
using System.Globalization;

namespace Push
{
	public partial class Form1 : Form
	{
		private readonly FormSettings formSettings;
		enum commandResult { Overwrite, Rename, Skip, Cancel };
		public MyApplicationSettings appSettings;
		Size frmSize;

		public Form1(MyApplicationSettings appSettings)
		{
			InitializeComponent();

			// Create settings group...
			//		FormSettings contains all of the Window properties...
			//		It does NOT contain the controls or the app configuration data...
			formSettings = new FormSettings(this);

			// Enable Auto-Save...
			formSettings.SaveOnClose = true;

			this.appSettings = appSettings;
			
			// Set the default form properties and then update them with the last used properties...
			InitControls();

		} // END_CTOR


		//	TODO: Re-factor... Necessary?
		private void InitControls()
		{
			// If we get here, Set the default values for the controls...

			if (appSettings == null)
			{
				// We shoud NEVER get here...

				// Fetch the path where the application is running...
				FileInfo sourceFileInfo = new FileInfo("Push.exe");
				
				appSettings = new MyApplicationSettings()
					{
						DisableSplashScreen = false,
						DisableXMLOptions = false,
						DuplicateFileAction = "Overwrite",
						ExePath = sourceFileInfo.DirectoryName,
						FileExtensionFilter = "*.*",
						HideDupeMessage = false,
						SourcePath = sourceFileInfo.DirectoryName +  @"Source",
						TargetPath = sourceFileInfo.DirectoryName +  @"Target"
					};
			}


			// Test to see if any of the required properties are missing...
			if (appSettings.DuplicateFileAction == null ||
				appSettings.FileExtensionFilter == null ||
				appSettings.SourcePath == null ||
				appSettings.TargetPath == null)
			{
				string s = string.Empty;
				Form2 dlg = new Form2();

				// Copy the current settings into the Configuration form...
				dlg.appSettings = appSettings;
				dlg.StartPosition = FormStartPosition.CenterParent;

				if (dlg.ShowDialog(this) == DialogResult.OK) s = "OK";
				else s = "Cancel";

				// Copy settings...
				appSettings = dlg.appSettings;

				dlg.Dispose();

				LoadListView(listView1, appSettings.SourcePath);
				LoadListView(listView2, appSettings.TargetPath);

				// Buy default, assume the details are visible...
				pictureBox1.Image = global::Push.Properties.Resources.Control_Collapser1;
				label4.Text = "Hide Details";
				panel1.Visible = true;
				splitContainer1.Visible = true;
				appSettings.ShowDetails = true;

				formSettings.Form.MinimumSize = new Size(764, 286);

				// Load the default position for the application...
				//Width = 764;
				//Height = 286;
				//Size = new Size(764, 286);
				//MinimumSize = new Size(764, 286);

				//StartPosition = FormStartPosition.CenterScreen;
			}



			// Update the form properties to the last used...
			UpdateControls();

		} // END_METHOD


		// TODO: Re-factor... Or Delete... Necessary?
		private void UpdateControls()
		{
			// If we get here, update the controls to the last used settings...

			if (appSettings.ShowDetails)
			{
				pictureBox1.Image = global::Push.Properties.Resources.Control_Collapser1;
				label4.Text = "Hide Details";
				panel1.Visible = true;
				formSettings.Form.MinimumSize = new Size(764, 286);
				MinimumSize = new Size(764, 286);
			}
			else
			{
				pictureBox1.Image = global::Push.Properties.Resources.Control_Expander1;
				label4.Text = "Show Details";
				panel1.Visible = false;
				formSettings.Form.MinimumSize = new Size(400, 161);
				MinimumSize = new Size(400, 161);
				MaximumSize = MinimumSize;
			}

		} // END_METHOD

	
		// Load data...
		private void Form1_Load(object sender, EventArgs e)
		{
			// Disable the Maximize control on the form...
			this.MaximizeBox = false;

			// Hydrate the Source and Target Listboxes
			LoadListView(listView1, appSettings.SourcePath);
			LoadListView(listView2, appSettings.TargetPath);
		} // END_METHOD


		// Show-Hide ListViews...
		private void pictureBox1_Click(object sender, EventArgs e)
		{
			//if (appSettings.ShowDetails)
			if (splitContainer1.Visible)
				{
				// If we get here, hide the source and target ListViews...

				pictureBox1.Image = global::Push.Properties.Resources.Control_Expander1;
				label4.Text = "Show Details";
				appSettings.ShowDetails = false;

				// Save the current window size...
				frmSize = this.Size;

				// Set the minimum window size which will hide the source and target and shrink the status listbox...
				MinimumSize = new Size(400, 161);
				MaximumSize = MinimumSize;
				Size = MinimumSize;
				panel1.Visible = false;
				//splitContainer1.Visible = false;

			}
			else
			{
				// If we get here,	restore the original windows size...

				pictureBox1.Image = global::Push.Properties.Resources.Control_Collapser1;
				label4.Text = "Hide Details";
				appSettings.ShowDetails = true;

				// Reset the minimum size so the source and target can not be hidden when resizing the window...
				MinimumSize = new Size(764, 286);
				// Clear the maximum size so the user can resize the window...
				MaximumSize = new Size();
				// Restore the previous window size...
				Size = frmSize;
				panel1.Visible = true;
				//splitContainer1.Visible = true;
			}
		} // END_METHOD


		// Load Source or Target ListView...
		private bool LoadListView(ListView DestinationListView, string DestinationPath)
		{
			// Fetch all of the files in the source filder...
			if (!Directory.Exists(DestinationPath)) return false;

			DestinationListView.Items.Clear();

			List<string> fileExtensionList = new List<string>();
			fileExtensionList.AddRange(LoadFileExtensions());

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
				string friendlyFileType = GetFileTypeDescription(file);
				string friendlyFileSize = StrFormatByteSize(fileInfo.Length);
				string fileDate = File.GetCreationTime(file).ToString("MM/dd/yyyy h:mm tt");

				ListViewItem itemArray = new ListViewItem(new string[] 
						{ fileName, friendlyFileType, friendlyFileSize, fileDate });
				DestinationListView.Items.Add(itemArray);

			} // END_FOREACH

			return true;

		} // END_METHOD


		private List<string> LoadFileExtensions()
		{
			// File extension types
			string[] delimiters = new string[] { ";", "; ", "|", "| ", " |", " | ", ":", ": ", " " };
			string[] fefArray = appSettings.FileExtensionFilter.Split(delimiters, StringSplitOptions.None);
			return new List<string>(fefArray);
		} // END_METHOD


		private void pictureBox2_Click(object sender, EventArgs e)
		{
			button1_Click(sender, e);
		}

		// Copy Files from Source folder to Target folder...
		private void button1_Click(object sender, EventArgs e)
		{
			ArrayList fileSourceArrayList = new ArrayList();
			
			// Validation...
			if (!Directory.Exists(appSettings.SourcePath) || !Directory.Exists(appSettings.TargetPath))
			{
				LoadConfigurationDialog();
				return;
			}

			// Validation...
			if (appSettings.SourcePath.Equals(appSettings.TargetPath))
			{
				LoadConfigurationDialog();
				return;
			}

			// Validation...
			if (string.IsNullOrEmpty(appSettings.SourcePath) || string.IsNullOrEmpty(appSettings.TargetPath))
			{
				LoadConfigurationDialog();
				return;
			}

			// Init Controls...
			listBox1.Items.Clear();

			// File extension types...
			List<string> FileExtensionArrayList = LoadFileExtensions();
			
			// Build list of file to copy... 
			foreach (string fileExtension in FileExtensionArrayList)
			{
				string[] fileSourceStrArray = Directory.GetFiles(appSettings.SourcePath, fileExtension);

				if (fileSourceStrArray.Length <= 0) 
					continue;
   
				foreach (string s in fileSourceStrArray) 
					fileSourceArrayList.Add(s);
			} // END_FOREACH

			// Build a list of files on the target folder...
			string[] fileTargetStrArray = System.IO.Directory.GetFiles(appSettings.TargetPath);
			int dupeFileCount = 0;
			// OUTER LOOP -- Iterate over each file in the target list...
			foreach (string t in fileTargetStrArray)
			{
				FileInfo targetFileInfo = new FileInfo(t);

				// INNER_LOOP -- Iterate over each file in the source list...
				foreach (string s in fileSourceArrayList)
				{
					FileInfo sourceFileInfo = new FileInfo(s);

					// Compare the fileName.ext (ignore the path)...
					if (!targetFileInfo.Name.Equals(sourceFileInfo.Name, StringComparison.Ordinal))
					{
						continue;
					}
					++dupeFileCount;
					break; // Exit innter loop...

				} // END_FOREACH_INNER
			} // END_FOREACH_OUTER

			if (dupeFileCount <= 0)
			{
				CopyOverwrite(fileSourceArrayList, appSettings.TargetPath);
			}
			else
			{

				if (appSettings.HideDupeMessage)
				{
					//---------------------------------------------------------
					// If we get here, perform whatever Dupe File Action has been configured...

					switch ((commandResult)Enum.Parse(typeof(commandResult), appSettings.DuplicateFileAction))
					{
						case commandResult.Rename:
							RenameDulpicates(fileSourceArrayList, fileTargetStrArray, appSettings.TargetPath, appSettings.SourcePath);
							break;
						case commandResult.Skip:
							fileSourceArrayList = SkipDuplicates(fileSourceArrayList, fileTargetStrArray, appSettings.TargetPath, appSettings.SourcePath);
							break;
						case commandResult.Cancel:
							return;
						case commandResult.Overwrite:
						default:
							CopyOverwrite(fileSourceArrayList, appSettings.TargetPath);
							break;
					} // END SWITCH

				}
				else
				{
					//---------------------------------------------------------
					// If we get here, Hide Dupe Message checkbox is false.

					cTaskDialog.ForceEmulationMode = true;
					cTaskDialog.EmulatedFormWidth = 450;

					DialogResult res =
							cTaskDialog.ShowTaskDialogBox(
							this,
							"Duplicate Files Found",
							string.Format("There were {0} duplicate files found in the Target Folder.", dupeFileCount),
							"What would you like to do?",
							"Renamed files will have the format: original_File_Name(n).ext, where (n) is a numeric value. " +
								"When multiple copies exist the latest duplicate will always have the highest value.\n\n" +
								"These settings may be modified in the Configuration Dialog.",
							string.Empty,
							"Don't show me this message again",
							string.Empty,
							"Overwrite All Duplicates|Copy/Rename All Duplicates|Skip All Duplicates|Cancel Copy",
							eTaskDialogButtons.None,
							eSysIcons.Information,
							eSysIcons.Warning);

					//-------------------------------------------------------------
					// Based on the configuration above, DialogResult and RadioButtonResult is ignored...

					if (cTaskDialog.VerificationChecked)
					{
						// If we get here, the Display Dupe Message checkbox 
						//		has been deselected. Save the currently selected 
						//		action to settings...
						appSettings.HideDupeMessage = cTaskDialog.VerificationChecked;
						appSettings.DuplicateFileAction = Enum.GetName(typeof(commandResult), cTaskDialog.CommandButtonResult);
					}

					switch ((commandResult)cTaskDialog.CommandButtonResult)
					{
						case commandResult.Rename:
							RenameDulpicates(fileSourceArrayList, fileTargetStrArray, appSettings.TargetPath, appSettings.SourcePath);
							break;
						case commandResult.Skip:
							fileSourceArrayList = SkipDuplicates(fileSourceArrayList, fileTargetStrArray, appSettings.TargetPath, appSettings.SourcePath);
							break;
						case commandResult.Cancel:
							return;
						case commandResult.Overwrite:
						default:
							CopyOverwrite(fileSourceArrayList, appSettings.TargetPath);
							break;

					} // END SWITCH

				} // END_IF_ELSE HideDupeMessage

			} // END_IF_ELSE DupeFileCount

			/*----------------------------------------------------------------- 
			 * If we get here, all of the files have been copied from the source folder to 
			 * the target folder.
			 * 
			 * Now verify and remove each file has been copied.
			 *----------------------------------------------------------------*/

			// Rebuild the Target file list with the new files that have been copied...
			fileTargetStrArray = System.IO.Directory.GetFiles(appSettings.TargetPath);

			#region [ DELETE COPIED FILES ]
			// OUTER LOOP -- Iterate over each file in the target list...
			foreach (string t in fileTargetStrArray)
			{
				FileInfo targetFileInfo = new FileInfo(t);

				// INNER_LOOP -- Iterate over each file in the source list...
				foreach (string s in fileSourceArrayList)
				{
					FileInfo sourceFileInfo = new FileInfo(s);

					// Compare the fileName.ext (ignore the path)...
					if (!targetFileInfo.Name.Equals(sourceFileInfo.Name, StringComparison.Ordinal))
						continue;

					// Compare size and create date...
					// TODO: Try using CRC Checksum later...
					if (sourceFileInfo.Length != targetFileInfo.Length)
						break;

					//---------------------------------------------------------
					// If we get here, the files match...

					// Delete the source file...
					File.Delete(s);

					// Update UI...
					listBox1.Items.Add("CleanUp: Deleting " + s);
					listBox1.Update();
					break; // Exit innter loop...

				} // END_FOREACH_INNER
			} // END_FOREACH_OUTER
			#endregion

			listBox1.Items.Add("Copy Complete");
			listBox1.Update();

			// Update Source & Target Listboxes...
			LoadListView(listView1, appSettings.SourcePath);
			LoadListView(listView2, appSettings.TargetPath);

		} // END_METHOD


		private void LoadConfigurationDialog()
		{
			string s = string.Empty;
			Form2 dlg = new Form2();
			

			// Copy the current settings into the Configuration form...
			dlg.appSettings = appSettings;
			dlg.StartPosition = FormStartPosition.CenterParent;
			
			if (dlg.ShowDialog(this) == DialogResult.OK) s = "OK";
			else s = "Cancel";

			// Copy settings...
			appSettings = dlg.appSettings;

			dlg.Dispose();

		} // END_METHOD


		private void RenameDulpicates(ArrayList fileSourceArrayList, string[] fileTargetStrArray, string targetPath, string sourcePath)
		{
			bool okToRename = false;
			int suffixInteger = 0;
			int matchInteger = 0;

			string pattern = @"(?<Prefix>(\w*))\((?<integer>\d*)\)";

			// OUTER LOOP
			// Interate over each file in the source folder...
			foreach (string s in fileSourceArrayList)
			{
				string sourceFileNamePrefix = Path.GetFileNameWithoutExtension(s);
				string sourceFileName = Path.GetFileName(s);
				string sourceFileExtension = Path.GetExtension(s);

				
				// INNER LOOP
				// Iterate over each file in the target folder...
				foreach (string t in fileTargetStrArray)
				{
					// init...
					matchInteger = 0;
					
					//string targetFileName = Path.GetFileNameWithoutExtension(t);
					string targetFileExtension = Path.GetExtension(t);
					string targetFileName = Path.GetFileName(t);

					// Compair source and target filename...
					if (targetFileName.Equals(sourceFileName, StringComparison.Ordinal))
					{
						okToRename = true;
						continue;
					}

					//------------------------------------------------------------------------
					// If we get here, the source and target filenames are not the same...

					Match match = Regex.Match(targetFileName, pattern);

					if (!match.Success )
					{
						// If we get here, the files do not match. Do nothing...
						continue;
					}

					// Compar source and target filename...
					string prefix = match.Groups["Prefix"].Value;
					if (!prefix.Equals(sourceFileNamePrefix, StringComparison.Ordinal) || 
						!targetFileExtension.Equals(sourceFileExtension, StringComparison.Ordinal) )
					{
						// If we get here, the target file name prefix does not match the source file name...
						continue;
					}
					
					//------------------------------------------------------------------------
					// If we get here, we found a similar file name...

					// Fetch the suffix-integer...
					string value = match.Groups["integer"].Value;
					Int32.TryParse(value, out matchInteger);

					if (suffixInteger <= matchInteger)
					{
						suffixInteger = matchInteger;
						okToRename = true;
						continue;
					}
		
					//------------------------------------------------------------------------
					// If we get here, target integer is less than the current sufficInteger...

					continue;
				} // END_INNER_LOOP

				if (okToRename)
				{
					// Copy the source file to the target folder...
					string sourcefileName = Path.GetFileNameWithoutExtension(s);
					sourcefileName = string.Format("{0} ({1}){2}", sourcefileName, ++suffixInteger, sourceFileExtension);
					string destFileName = Path.Combine(targetPath, sourcefileName);
					File.Copy(s, destFileName, false);
				}
				else
				{
					// Copy the source file to the target folder...
					string sourcefileName = Path.GetFileName(s);
					string destFileName = Path.Combine(targetPath, sourcefileName);
					File.Copy(s, destFileName, false);
				}// END_IF

				// Reset...
				okToRename = false;
				suffixInteger = 0;
				matchInteger = 0;

			} // END_OUTER_LOOP

		} // END_METHOD


		private ArrayList SkipDuplicates(ArrayList fileSourceArrayList, string[] fileTargetStrArray, string targetPath, string sourcePath)
		{
			/*  
			 *  If a duplicate is SKIPPED, it should NOT be deleted from the source folder. 
			 *  We need to return a revised list of files that are only the ones that should be deleted.
			 *  
			 *  fileSourceArrayList
			 */
			bool okToCopy = true;
			ArrayList deleteSourceArrayList = new ArrayList();

			foreach (string s in fileSourceArrayList)
			{
				FileInfo sourceFileInfo = new FileInfo(s);

				// INNER_LOOP -- Iterate over each file in the source list...
				foreach (string t in fileTargetStrArray)
				{
					FileInfo targetFileInfo = new FileInfo(t);
					if (sourceFileInfo.Name.Equals(targetFileInfo.Name, StringComparison.Ordinal))
					{
						// If we get here, the file esists in the target folder.
						//      Skip this file...

						okToCopy = false;
						break; // Exit Inner loop...

					} // END_IF

				} // END_FOREACH_INNER

				if (okToCopy)
				{
					// Copy the source file to the target folder...
					string sourcefileName = Path.GetFileName(s);
					string destFileName = Path.Combine(targetPath, sourcefileName);

					File.Copy(s, destFileName, true);

					// Update the lisst of files that should be deleted from the source folder...
					//      Verify that 't' is the correct file name...
					deleteSourceArrayList.Add(s);

					// Update UI...
					listBox1.Items.Add("Copying " + s + " to " + destFileName);
					listBox1.Update();
				}

				// Raise the okToCopy flag...
				okToCopy = true;

			} // END_FOREACH_OUTER

			return deleteSourceArrayList;

		} // END_METHOD


		private void CopyOverwrite(ArrayList fileSourceArrayList, string targetPath)
		{
			// Copy files...
			foreach (string s in fileSourceArrayList)
			{
				string srcfileName = Path.GetFileName(s);
				string destFileName = Path.Combine(targetPath, srcfileName);

				File.Copy(s, destFileName, true);

				// Update UI...
				listBox1.Items.Add("Copying " + srcfileName + " to " + destFileName);
				listBox1.Update();
			}

		} // END_METHOD


		#region [ CONSTANTS ]

		private const uint FILE_ATTRIBUTE_READONLY = 0x00000001;
		private const uint FILE_ATTRIBUTE_HIDDEN = 0x00000002;
		private const uint FILE_ATTRIBUTE_SYSTEM = 0x00000004;
		private const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
		private const uint FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
		private const uint FILE_ATTRIBUTE_DEVICE = 0x00000040;
		private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
		private const uint FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
		private const uint FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
		private const uint FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
		private const uint FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
		private const uint FILE_ATTRIBUTE_OFFLINE = 0x00001000;
		private const uint FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
		private const uint FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
		private const uint FILE_ATTRIBUTE_VIRTUAL = 0x00010000;

		private const uint SHGFI_ICON = 0x000000100;     // get icon
		private const uint SHGFI_DISPLAYNAME = 0x000000200;     // get display name
		private const uint SHGFI_TYPENAME = 0x000000400;     // get type name
		private const uint SHGFI_ATTRIBUTES = 0x000000800;     // get attributes
		private const uint SHGFI_ICONLOCATION = 0x000001000;     // get icon location
		private const uint SHGFI_EXETYPE = 0x000002000;     // return exe type
		private const uint SHGFI_SYSICONINDEX = 0x000004000;     // get system icon index
		private const uint SHGFI_LINKOVERLAY = 0x000008000;     // put a link overlay on icon
		private const uint SHGFI_SELECTED = 0x000010000;     // show icon in selected state
		private const uint SHGFI_ATTR_SPECIFIED = 0x000020000;     // get only specified attributes
		private const uint SHGFI_LARGEICON = 0x000000000;     // get large icon
		private const uint SHGFI_SMALLICON = 0x000000001;     // get small icon
		private const uint SHGFI_OPENICON = 0x000000002;     // get open icon
		private const uint SHGFI_SHELLICONSIZE = 0x000000004;     // get shell size icon
		private const uint SHGFI_PIDL = 0x000000008;     // pszPath is a pidl
		private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;     // use passed dwFileAttribute

		#endregion


		public static string GetFileTypeDescription(string fileNameOrExtension)
		{
			SHFILEINFO shfi;
			if (IntPtr.Zero != SHGetFileInfo(
								fileNameOrExtension,
								FILE_ATTRIBUTE_NORMAL,
								out shfi,
								(uint)Marshal.SizeOf(typeof(SHFILEINFO)),
								SHGFI_USEFILEATTRIBUTES | SHGFI_TYPENAME))
			{
				return shfi.szTypeName;
			}
			return null;
		} // END_METHOD


		[DllImport("shell32")]
		private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, uint flags);


		#region [ STRUCT ]
		[StructLayout(LayoutKind.Sequential)]
		private struct SHFILEINFO
		{
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		} // END_STRUCT
		#endregion

		
		[DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
		public static extern long StrFormatByteSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);


		/// <summary>
		/// Converts a numeric value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
		/// </summary>
		/// <param name="filelength">The numeric value to be converted.</param>
		/// <returns>the converted string</returns>
		public static string StrFormatByteSize(long filesize)
		{
			StringBuilder sb = new StringBuilder(11);
			StrFormatByteSize(filesize, sb, sb.Capacity);
			return sb.ToString();

		} // END_METHOD


		#region [ TOOL STRIP ]

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			button1_Click(sender, e);
		} // END_METHOD

		private void toolStripButton5_Click(object sender, EventArgs e)
		{
			LoadListView(listView1, appSettings.SourcePath);
			LoadListView(listView2, appSettings.TargetPath);
		} // END_METHOD

		private void toolStripButton6_Click(object sender, EventArgs e)
		{
			LoadConfigurationDialog();
		}

		#endregion


		#region [ DEBUG BUTTONS ]

		// DEBUG Button - MistyRose -- Reset source and target data...
		private void button2_Click(object sender, EventArgs e)
		{
			DEBUG_MistyRose();
		} // END_METHOD

		private void DEBUG_MistyRose()
		{
			DEBUG_InitFolders();

			DEBUG_LoadFolderTestData(@"C:\DEV_TESTDATA\Pictures", appSettings.SourcePath);
			DEBUG_LoadFolderTestData(@"C:\DEV_TESTDATA\TargetPictures", appSettings.TargetPath);

			//-----------------------------------------------------------------
			// Clear the status list box...
			listBox1.Items.Clear();

			// Hydrate the Source and Target Listboxes
			LoadListView(listView1, appSettings.SourcePath);
			LoadListView(listView2, appSettings.TargetPath);
		}

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
			fileExtensionList.AddRange(LoadFileExtensions());

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
		private void button4_Click(object sender, EventArgs e)
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
			listBox1.Items.Clear();

			// Hydrate the Source and Target Listboxes
			LoadListView(listView1, appSettings.SourcePath);
			LoadListView(listView2, appSettings.TargetPath);
		}

		// DEBUG Button - Powder Blue -- Reset source and target data...
		private void button5_Click(object sender, EventArgs e)
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
			listBox1.Items.Clear();

			// Hydrate the Source and Target Listboxes
			LoadListView(listView1, appSettings.SourcePath);
			LoadListView(listView2, appSettings.TargetPath);
		}
		
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
				LoadListView(listView1, appSettings.SourcePath);
				LoadListView(listView2, appSettings.TargetPath);
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
	
		#endregion

		// Close...
		private void Form1_Click(object sender, EventArgs e)
		{
			formSettings.Save();
		}

	} // END_CLASS


	public class PushSettings
	{
		public bool HideDupeMessage;
		public string SourcePath;
		public string TargetPath;
		public string FileExtensionFilter;
		public string DuplicateFileAction;

		public bool DisableSplashScreen;
		public bool DisableXMLOptions;

		public string ExePath;

	} // END_CLASS
} // END_NAMESPACE
