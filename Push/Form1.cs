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

namespace Push
{
	public partial class Form1 : Form
	{
		enum commandResult { Overwrite, Rename, Skip, Cancel };
		PushSettings settings;

	
		public Form1()
		{
			InitializeComponent();

		} // END_METHOD


		// Load Target ListView...
		private bool LoadTarget()
		{
			// Fetch all of the files in the source filder...
			if (!Directory.Exists(settings.TargetPath))
			{
				//listBox1.Items.Add("The Target path does not exist!");
				return false;
			}

			listView2.Items.Clear();

			ArrayList FileExtensionArrayList = LoadFileExtensions();

			ArrayList fileSourceArrayList = new ArrayList();

			foreach (string FileExtension in FileExtensionArrayList)
			{
				string[] fileSourceStrArray = Directory.GetFiles(settings.TargetPath, FileExtension);

				if (fileSourceStrArray.Length <= 0) continue;

				foreach (string s in fileSourceStrArray) fileSourceArrayList.Add(s);

			} // END_FOREACH

			foreach (string s in fileSourceArrayList)
			{

				FileInfo targetFileInfo = new FileInfo(s);

				string friendlyFileSize = StrFormatByteSize(targetFileInfo.Length);
				DateTime fileDate = File.GetCreationTime(s);
				string friendlyFileType = GetFileTypeDescription(s);
				string fileName = Path.GetFileNameWithoutExtension(targetFileInfo.Name);

				ListViewItem item = new ListViewItem(fileName);
				item.SubItems.Add(friendlyFileType);
				item.SubItems.Add(friendlyFileSize);
				item.SubItems.Add(fileDate.ToString("MM/dd/yyyy h:mm tt"));
				// 
				ListViewItem[] itemArray = new ListViewItem[] { item };
				listView2.Items.Add(item);
			}

			return true;
		} // END_METHOD


		private ArrayList LoadFileExtensions()
		{
			ArrayList FileExtensionArrayList;
			// File extension types
			string[] delimiters = new string[] { ";", "; ", "|", "| ", " |", " | ", ":", ": ", " " };
			string[] fefArray = settings.FileExtensionFilter.Split(delimiters, StringSplitOptions.None);
			FileExtensionArrayList = new ArrayList(fefArray);
			return FileExtensionArrayList;
		}

		
		// Load Source ListView...
		private bool LoadSource()
		{
			// Fetch all of the files in the source filder...
			if (!Directory.Exists(settings.SourcePath))
			{
				//listBox1.Items.Add("The Source path do not exist!");
				return false;
			}

			listView1.Items.Clear();

			// File extension types...
			ArrayList FileExtensionArrayList = LoadFileExtensions();

			ArrayList fileSourceArrayList = new ArrayList();

			foreach (string fileExtension in FileExtensionArrayList)
			{
				string[] fileSourceStrArray = Directory.GetFiles(settings.SourcePath, fileExtension);

				if (fileSourceStrArray.Length <= 0) continue;
				
				foreach (string s in fileSourceStrArray) fileSourceArrayList.Add(s);
			} // END_FOREACH

			foreach (string s in fileSourceArrayList)
			{
				FileInfo sourceFileInfo = new FileInfo(s);
				string friendlyFileSize = StrFormatByteSize(sourceFileInfo.Length);
				DateTime fileDate = File.GetCreationTime(s);
				string friendlyFileType = GetFileTypeDescription(s);
				string fileName = Path.GetFileNameWithoutExtension(sourceFileInfo.Name);

				ListViewItem item = new ListViewItem(fileName);
				item.SubItems.Add(friendlyFileType); 
				item.SubItems.Add(friendlyFileSize);
				item.SubItems.Add(fileDate.ToString("MM/dd/yyyy h:mm tt"));
				// 
				ListViewItem[] itemArray = new ListViewItem[] {item};
				listView1.Items.Add(item);
			}

			return true;
		} // END_METHOD


		// Copy Files from Source folder to Target folder...
		private void button1_Click(object sender, EventArgs e)
		{
			ArrayList fileSourceArrayList = new ArrayList();
			
			// Validation...
			if (!Directory.Exists(settings.SourcePath) || !Directory.Exists(settings.TargetPath))
			{
				LoadConfigurationDialog();
				return;
			}

			// Validation...
			if (settings.SourcePath.Equals(settings.TargetPath))
			{
				LoadConfigurationDialog();
				return;
			}

			// Validation...
			if (string.IsNullOrEmpty(settings.SourcePath) || string.IsNullOrEmpty(settings.TargetPath))
			{
				LoadConfigurationDialog();
				return;
			}

			// Init Controls...
			listBox1.Items.Clear();

			// File extension types...
			ArrayList FileExtensionArrayList = LoadFileExtensions();
			
			// Build list of file to copy... 
			foreach (string fileExtension in FileExtensionArrayList)
			{
				string[] fileSourceStrArray = Directory.GetFiles(settings.SourcePath, fileExtension);

				if (fileSourceStrArray.Length <= 0) 
					continue;
   
				foreach (string s in fileSourceStrArray) 
					fileSourceArrayList.Add(s);
			} // END_FOREACH

			// Build a list of files on the target folder...
			string[] fileTargetStrArray = System.IO.Directory.GetFiles(settings.TargetPath);
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
				CopyOverwrite(fileSourceArrayList, settings.TargetPath);
			}
			else
			{
				// Check Display Dupe Message...
				//TODO: Flip the IF/ELSE code blocks and remove the '!'...
				if (!settings.HideDupeMessage)
				{

					cTaskDialog.ForceEmulationMode = true;
					cTaskDialog.EmulatedFormWidth = 450;

					DialogResult res =
							cTaskDialog.ShowTaskDialogBox(
							this,
							"Duplicate Files Found",
							string.Format("There were {0} duplicate files found in the Target Folder.", dupeFileCount),
							"What would you like to do?",
							"Renamed files will have the format: original_File_Name(n).ext, where (n) is a nemeric value. " +
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
						settings.HideDupeMessage = cTaskDialog.VerificationChecked;
						settings.DuplicateFileAction = Enum.GetName(typeof(commandResult), cTaskDialog.CommandButtonResult);
					}

					switch ((commandResult)cTaskDialog.CommandButtonResult)
					{
						case commandResult.Rename:
							RenameDulpicates(fileSourceArrayList, fileTargetStrArray, settings.TargetPath, settings.SourcePath);
							break;
						case commandResult.Skip:
							fileSourceArrayList = SkipDuplicates(fileSourceArrayList, fileTargetStrArray, settings.TargetPath, settings.SourcePath);
							break;
						case commandResult.Cancel:
							return;
						case commandResult.Overwrite:
						default:
							CopyOverwrite(fileSourceArrayList, settings.TargetPath);
							break;

					} // END SWITCH
				}
				else
				{
					//---------------------------------------------------------
					// If we get here, Display dupe message is false.
					// Perform whatever Duplicate File Action is set...

					switch ((commandResult)Enum.Parse(typeof(commandResult), settings.DuplicateFileAction))
					{
						case commandResult.Rename:
							RenameDulpicates(fileSourceArrayList, fileTargetStrArray, settings.TargetPath, settings.SourcePath);
							break;
						case commandResult.Skip:
							fileSourceArrayList = SkipDuplicates(fileSourceArrayList, fileTargetStrArray, settings.TargetPath, settings.SourcePath);
							break;
						case commandResult.Cancel:
							return;
						case commandResult.Overwrite:
						default:
							CopyOverwrite(fileSourceArrayList, settings.TargetPath);
							break;
					} // END SWITCH

				} // END_IF_ELSE

			}

			/*----------------------------------------------------------------- 
			 * If we get here, all of the files have been copied from the source folder to 
			 * the target folder.
			 * 
			 * Now verify and remove each file has been copied.
			 *----------------------------------------------------------------*/

			// Rebuild the Target file list with the new files that have been copied...
			fileTargetStrArray = System.IO.Directory.GetFiles(settings.TargetPath);

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
			LoadSource();
			LoadTarget();

		} // END_METHOD


		private void LoadConfigurationDialog()
		{
			string s = string.Empty;
			Form2 dlg = new Form2();
			// Copy the current settings into the Configuration form...
			dlg.settings = settings;
			if (dlg.ShowDialog(this) == DialogResult.OK) s = "OK";
			else s = "Cancel";

			// Copy settings...
			settings = dlg.settings;

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
					// suffixInteger 
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


		// DEVELOPMENT ONLY -- Reset Application...
		private void button2_Click(object sender, EventArgs e){

			//-----------------------------------------------------------------
			//  Clear the Target Folder
			
			string[] filesStrArray = Directory.GetFiles(settings.TargetPath);
			foreach (string s in filesStrArray) 
				File.Delete(s);

			//-----------------------------------------------------------------
			//  Clear the Source Folder

			filesStrArray = Directory.GetFiles(settings.SourcePath);
			foreach (string t in filesStrArray) 
				File.Delete(t);

			//-----------------------------------------------------------------
			//  Initialize the Source Folder with Test Data

			string DEBUG_testSourcePicturesDataPath = @"C:\DEV_TESTDATA\Pictures";
			string DEBUG_testTargetPicturesDataPath = @"C:\DEV_TESTDATA\TargetPictures";

			// File extension types
			ArrayList FileExtensionArrayList = LoadFileExtensions();

			if (!Directory.Exists(settings.SourcePath) || !Directory.Exists(settings.TargetPath))
			{
				//listBox1.Items.Add("The Source or Target path do not exist!");
				return;
			}

			ArrayList fileTestDataArrayList = new ArrayList();

			//-----------------------------------------------------------------
			// Load the Source folder with test data...
			foreach (string fileExtension in FileExtensionArrayList)
			{
				string[] fileTestDataStrArray = Directory.GetFiles(DEBUG_testSourcePicturesDataPath, fileExtension);

				if (fileTestDataStrArray.Length <= 0) 
					continue;

				foreach (string s in fileTestDataStrArray) 
					fileTestDataArrayList.Add(s);
			} // END_FOREACH

			string testDataFileName;
			string destFileName;
			foreach (string s in fileTestDataArrayList)
			{
				// Use static Path methods to extract only the file name from the path.
				testDataFileName = Path.GetFileName(s);
				destFileName = Path.Combine(settings.SourcePath, testDataFileName);
				File.Copy(s, destFileName, true);
			}

			//-----------------------------------------------------------------
			// Load the Target folder with test data...
			fileTestDataArrayList.Clear();
			foreach (string fileExtension in FileExtensionArrayList)
			{
				string[] fileTestDataStrArray = Directory.GetFiles(DEBUG_testTargetPicturesDataPath, fileExtension);

				if (fileTestDataStrArray.Length <= 0)
					continue;

				foreach (string s in fileTestDataStrArray)
					fileTestDataArrayList.Add(s);
			} // END_FOREACH

			testDataFileName = string.Empty;
			destFileName = string.Empty;
			foreach (string s in fileTestDataArrayList)
			{
				// Use static Path methods to extract only the file name from the path.
				testDataFileName = Path.GetFileName(s);
				destFileName = Path.Combine(settings.TargetPath, testDataFileName);
				File.Copy(s, destFileName, true);
			}

			//-----------------------------------------------------------------
			// Clear the status list box...
			listBox1.Items.Clear();

			// Hydrate the Source and Target Listboxes
			LoadSource();

			LoadTarget();

		} // END_METHOD


		// Empty... 
		private void Form1_Load(object sender, EventArgs e)
		{
			// Open and read PushSettings file...

			// Fetch the path where the application is running...
			FileInfo sourceFileInfo = new FileInfo("Push.exe");
			string exePath = sourceFileInfo.DirectoryName;

			string pushSettingsJSON;
			try
			{
				pushSettingsJSON = System.IO.File.ReadAllText(exePath + @"\PushSettings");
			}
			catch
			{
				pushSettingsJSON = System.IO.File.ReadAllText(exePath + @"\PushSettingsDefault");
				File.Copy(exePath + @"\PushAppConfig", exePath + @"\PushSettings", true);
			}

			settings = new PushSettings();
			// Convert to object...
			settings = (PushSettings)new JavaScriptSerializer().Deserialize(pushSettingsJSON, typeof(PushSettings));
			settings.ExePath = exePath;

			// Hydrate the Source and Target Listboxes
			LoadSource();
			LoadTarget();

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


		// Refresh Form...
		private void button3_Click(object sender, EventArgs e)
		{
			LoadSource();
			LoadTarget();

		} // END_METHOD


		#region [ TOOL STRIP ]

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			button1_Click(sender, e);
		} // END_METHOD

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			button3_Click(sender, e);
		} // END_METHOD

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			LoadConfigurationDialog();
		} // END_METHOD
		
		#endregion


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
