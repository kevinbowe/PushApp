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
	public class CopyFiles
	{
		// Copy Files from Source folder to Target folder...
		public Tuple<Helper.commandResult, int, int> CopyAction(MainForm mainForm)
		{
			AppSettings appSettings = mainForm.appSettings;

			ArrayList fileSourceArrayList = new ArrayList();
			Helper.commandResult DuplicateAction;

			Tuple<int, int> copyResult;

			// File extension types...
			List<string> FileExtensionArrayList = Helper.LoadFileExtensions(appSettings);

			#region [ BUILD LIST OF SOURCE FILES ]
			// Build list of file to copy... 
			foreach (string fileExtension in FileExtensionArrayList)
			{
				string[] fileSourceStrArray = Directory.GetFiles(appSettings.SourcePath, fileExtension);

				if (fileSourceStrArray.Length <= 0)
					continue;

				foreach (string s in fileSourceStrArray)
					fileSourceArrayList.Add(s);
			} // END_FOREACH
			#endregion

			#region [ BUILD LIST OF TARGET FILES ]
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
			#endregion

			if (dupeFileCount <= 0)
			{
				// Save the Dupe Action. We need this for the statusing return value...
				DuplicateAction = Helper.commandResult.Overwrite;				
				
				copyResult = CopyFilesOverwrite.CopyOverwrite(fileSourceArrayList, appSettings);
			}
			else
			{
				if (appSettings.HideDupeMessage.GetValueOrDefault(false))
				{
					//---------------------------------------------------------
					// If we get here, perform whatever Dupe File Action has been configured...

					// Save the Dupe Action. We need this for the statusing return value...
					DuplicateAction = (Helper.commandResult)Enum.Parse(typeof(Helper.commandResult), appSettings.DuplicateFileAction);

					#region [ AUTO DUPE ACTION ]
					switch (DuplicateAction)
					{

						case Helper.commandResult.Rename:
							{
								copyResult = CopyFilesRename.RenameDulpicates(fileSourceArrayList, fileTargetStrArray, appSettings);
								break;
							}

						case Helper.commandResult.Skip:
							{
								copyResult = CopyFilesSkip.SkipDuplicates(ref fileSourceArrayList, fileTargetStrArray, appSettings);
								break;
							}

						case Helper.commandResult.Cancel:
							{
								copyResult = new Tuple<int, int>(0, 0);
								break;
							}

						case Helper.commandResult.Overwrite:
						default:
							{
								//Tuple<int> 
								copyResult = CopyFilesOverwrite.CopyOverwrite(fileSourceArrayList, appSettings);
								break;
							} 

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
							mainForm,
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

					// Fetch the Dupe Action. We need this for the statusing return value...
					DuplicateAction = (Helper.commandResult)cTaskDialog.CommandButtonResult;

					switch (DuplicateAction)
					{

						case Helper.commandResult.Rename:
							{
								copyResult = CopyFilesRename.RenameDulpicates(fileSourceArrayList, fileTargetStrArray, appSettings);
								break;
							}

						case Helper.commandResult.Skip:
							{
								copyResult = CopyFilesSkip.SkipDuplicates(ref fileSourceArrayList, fileTargetStrArray, appSettings);
								break;
							}

						case Helper.commandResult.Cancel:
							{
								copyResult = new Tuple<int,int>(0,0);
								break;
							}

						case Helper.commandResult.Overwrite:
						default:
							{
								copyResult = CopyFilesOverwrite.CopyOverwrite(fileSourceArrayList, appSettings);
								break;
							}

					} // END SWITCH
					#endregion

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

					break; // Exit innter loop...

				} // END_FOREACH_INNER
			} // END_FOREACH_OUTER
			#endregion

			return new Tuple<Helper.commandResult, int, int>(DuplicateAction, copyResult.Item1, copyResult.Item2);
		} // END_METHOD

	} //END_CLASS
} // END_NS
