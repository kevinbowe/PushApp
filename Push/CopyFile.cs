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
//---
using System.Threading;

namespace Push
{
	public class CopyFile
	{
		BackgroundWorker bgWorker;

		private List<string> GetFiles(string SourcePath, List<string> FileExtensionArrayList, List<string> all_FileSourceList)
		{
			// Grab all of the files in the current folder...

			// Build list of files to copy... 
			foreach (string fileExtension in FileExtensionArrayList)
			{
				string[] fileSourceStrArray = Directory.GetFiles(SourcePath, fileExtension);

				if (fileSourceStrArray.Length <= 0)
					continue;

				foreach (string s in fileSourceStrArray)
					all_FileSourceList.Add(s);
			} // END_FOREACH

			// Find any subfolders in the current source path...

			DirectoryInfo directoryInfo = new DirectoryInfo(SourcePath);
			DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
			foreach (DirectoryInfo dirInfo in directoryInfoArray)
			{
				string newSourcePath = SourcePath + "\\" + dirInfo;
				all_FileSourceList = GetFiles(newSourcePath, FileExtensionArrayList, all_FileSourceList);
			}

			// __DEBUG CODE__
			return all_FileSourceList;
		} // END_METHOD




		public void CopyFiles(MainForm mainForm)
		{
			AppSettings appSettings = mainForm.appSettings;

			ArrayList fileSourceArrayList = new ArrayList();
			Helper.commandResult DuplicateAction;

			bgWorker = new BackgroundWorker();
			bgWorker.ProgressChanged += new ProgressChangedEventHandler(mainForm.bgProgressChangedEventHandler);
			bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mainForm.bgRunWorkerCompletedEventHandler);
			bgWorker.WorkerReportsProgress = true;
			bgWorker.WorkerSupportsCancellation = true;

			// File extension types...
			List<string> FileExtensionArrayList = Helper.LoadFileExtensions(appSettings);






			#region [ BUILD LIST OF SOURCE FILES ]
			// Build list of files to copy... 
			foreach (string fileExtension in FileExtensionArrayList)
			{
				string[] fileSourceStrArray = Directory.GetFiles(appSettings.SourcePath, fileExtension);

				if (fileSourceStrArray.Length <= 0)
					continue;

				foreach (string s in fileSourceStrArray)
					fileSourceArrayList.Add(s);
			} // END_FOREACH

			List<string> all_FileSourceList = new List<string>();
			all_FileSourceList = GetFiles(appSettings.SourcePath, FileExtensionArrayList, all_FileSourceList);

			if (all_FileSourceList.Count == 0)
			{
				// If we get here, there are not files in the source folder to process...
				bgWorker.DoWork += new DoWorkEventHandler(CopyFile_BackGround);
				bgWorker.RunWorkerAsync();
				return;
			}
			#endregion

			#region [ BUILD LIST OF TARGET FILES ]
			List<string> all_FileTargetList = new List<string>();
			all_FileTargetList = GetFiles(appSettings.TargetPath, FileExtensionArrayList, all_FileTargetList);


			// Build a list of files on the target folder...
			string[] fileTargetStrArray = System.IO.Directory.GetFiles(appSettings.TargetPath);
			int dupeFileCount = 0;

			// OUTER LOOP -- Iterate over each file in the target list...
			foreach (string t in all_FileTargetList /*fileTargetStrArray*/)
			{
				FileInfo targetFileInfo = new FileInfo(t);

				// INNER_LOOP -- Iterate over each file in the source list...
				foreach (string s in all_FileSourceList /*fileSourceArrayList*/)
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
			#endregion

			if (dupeFileCount <= 0)
			{
				bgWorker.DoWork += new DoWorkEventHandler(CopyFileOverwrite.CopyOverwrite_BackGround);
				List<object> args = new List<object>() { all_FileSourceList, appSettings };
				bgWorker.RunWorkerAsync(args);
			}
			else
			{
				if (appSettings.HideDupeMessage.GetValueOrDefault(false))
				{
					//---------------------------------------------------------
					// If we get here, perform whatever Dupe File Action has been configured...

					List<object> args;

					// Save the Dupe Action. We need this for the statusing return value...
					DuplicateAction = (Helper.commandResult)Enum.Parse(typeof(Helper.commandResult), appSettings.DuplicateFileAction);

					#region [ AUTO DUPE ACTION ]
					switch (DuplicateAction)
					{
						case Helper.commandResult.Rename:
							// Assign the correct event handler to the worker...
							bgWorker.DoWork += new DoWorkEventHandler(CopyFileRename.RenameDuplicates_BackGround);
							// Load DoWorkEvent Arguments
							args = new List<object>() { fileSourceArrayList, fileTargetStrArray, appSettings };
							// Start the worker...
							bgWorker.RunWorkerAsync(args);
							break;

						case Helper.commandResult.Skip:
							bgWorker.DoWork += new DoWorkEventHandler(CopyFileSkip.SkipDuplicates_BackGround);
							args = new List<object>() { all_FileSourceList, all_FileTargetList, appSettings };
							//args = new List<object>() { fileSourceArrayList, fileTargetStrArray, appSettings };
							bgWorker.RunWorkerAsync(args);
							break;

						case Helper.commandResult.Cancel:
							break;

						case Helper.commandResult.Overwrite:
						default:
							bgWorker.DoWork += new DoWorkEventHandler(CopyFileOverwrite.CopyOverwrite_BackGround);
							args = new List<object>() { all_FileSourceList, appSettings };
							bgWorker.RunWorkerAsync(args);
							break;

					} // END SWITCH
					#endregion

				}
				else
				{
					//---------------------------------------------------------
					// If we get here, Hide Dupe Message checkbox is false.

					#region [ TASK DIALOG ]
					cTaskDialog.ForceEmulationMode = true;
					cTaskDialog.EmulatedFormWidth = 450;

					DialogResult res =
							cTaskDialog.ShowTaskDialogBox(

							mainForm, // BUG - Use Correct Thread...

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
					#endregion

					//-------------------------------------------------------------
					// Based on the configuration above, DialogResult and RadioButtonResult is ignored...

					if (cTaskDialog.VerificationChecked)
					{
						// If we get here, the Display Dupe Message checkbox 
						//		has been deselected. Save the currently selected 
						//		action to settings...
						appSettings.HideDupeMessage = cTaskDialog.VerificationChecked;
						appSettings.DuplicateFileAction = Enum.GetName(typeof(Helper.commandResult), cTaskDialog.CommandButtonResult);
					}


					#region [ MANUAL DUPE ACTION ]

					List<object> args;

					// Fetch the Dupe Action. We need this for the statusing return value...
					DuplicateAction = (Helper.commandResult)cTaskDialog.CommandButtonResult;

					switch (DuplicateAction)
					{

						case Helper.commandResult.Rename:
							// Assign the correct event handler to the worker...
							bgWorker.DoWork += new DoWorkEventHandler(CopyFileRename.RenameDuplicates_BackGround);
							// Load DoWorkEvent Arguments
							args = new List<object>() { fileSourceArrayList, fileTargetStrArray, appSettings };
							// Start the worker...
							bgWorker.RunWorkerAsync(args);
							break;

						case Helper.commandResult.Skip:
							bgWorker.DoWork += new DoWorkEventHandler(CopyFileSkip.SkipDuplicates_BackGround);
							args = new List<object>() { all_FileSourceList, all_FileTargetList, appSettings };
							//args = new List<object>() { fileSourceArrayList, fileTargetStrArray, appSettings };
							bgWorker.RunWorkerAsync(args);
							break;

						case Helper.commandResult.Cancel:
							break;

						case Helper.commandResult.Overwrite:
						default:
							bgWorker.DoWork += new DoWorkEventHandler(CopyFileOverwrite.CopyOverwrite_BackGround);
							args = new List<object>() { all_FileSourceList, appSettings };
							bgWorker.RunWorkerAsync(args);
							break;

					} // END SWITCH
					#endregion

				} // END_IF_ELSE HideDupeMessage

			} // END_IF_ELSE DupeFileCount

		}


		public static void CopyFile_BackGround(object sender, DoWorkEventArgs doWorkEventArgs)
		{
			doWorkEventArgs.Result = new Tuple<Helper.commandResult, int, int>(Helper.commandResult.Fail, 0, 0);
		} // END_METHOD




	} //END_CLASS
} // END_NS
