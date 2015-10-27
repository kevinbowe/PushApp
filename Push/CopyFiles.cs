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
	public partial class MainForm //: IDisposable
	{

		// Copy Files from Source folder to Target folder...
		private void CopyFiles(object sender, EventArgs e)
		{
			ArrayList fileSourceArrayList = new ArrayList();

			// Init Controls...
			lbStatus.Items.Clear();

			// File extension types...
			List<string> FileExtensionArrayList = LoadFileExtensions(appSettings);

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
				if (appSettings.HideDupeMessage.GetValueOrDefault(false))
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
							//...((MainForm)sender),
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
					lbStatus.Items.Add("CleanUp: Deleting " + s);
					lbStatus.Update();

					break; // Exit innter loop...

				} // END_FOREACH_INNER
			} // END_FOREACH_OUTER
			#endregion

			lbStatus.Items.Add("Copy Complete");
			lbStatus.Update();

			// Update Source & Target Listboxes...
			LoadListView(lvSource, appSettings.SourcePath);
			LoadListView(lvTarget, appSettings.TargetPath);

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
				lbStatus.Items.Add("Copying " + srcfileName + " to " + destFileName);
				lbStatus.Update();
			}
		} // END_METHOD


		private void RenameDulpicates(ArrayList fileSourceArrayList, string[] fileTargetStrArray, string targetPath, string sourcePath)
		{
			bool okToRename = false;
			int suffixInteger = 0;
			int matchInteger = 0;

			string regExPattern = @"(?<Prefix>(\w*))\s*\((?<integer>\d*)\)";

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

					Match match = Regex.Match(targetFileName, regExPattern);

					if (!match.Success)
					{
						// If we get here, the files do not match. Do nothing...
						continue;
					}

					// Compar source and target filename...
					string prefix = match.Groups["Prefix"].Value;
					if (!prefix.Equals(sourceFileNamePrefix, StringComparison.Ordinal) ||
						!targetFileExtension.Equals(sourceFileExtension, StringComparison.Ordinal))
					{
						// If we get here, the target file name prefix does not match the source file prefix 
						// -- OR -- The target file extension does not match the source file extension...
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
					string newfileName = string.Format("{0} ({1}){2}", sourcefileName, ++suffixInteger, sourceFileExtension);
					string destFileName = Path.Combine(targetPath, newfileName);
					File.Copy(s, destFileName, false);

					// Update UI...
					lbStatus.Items.Add("Copying " + sourcefileName + " to " + destFileName);
					lbStatus.Update();
				}
				else
				{
					// Copy the source file to the target folder...
					string sourcefileName = Path.GetFileName(s);
					string destFileName = Path.Combine(targetPath, sourcefileName);
					File.Copy(s, destFileName, false);

					// Update UI...
					lbStatus.Items.Add("Copying " + sourcefileName + " to " + destFileName);
					lbStatus.Update();
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
					lbStatus.Items.Add("Copying " + s + " to " + destFileName);
					lbStatus.Update();
				}

				// Raise the okToCopy flag...
				okToCopy = true;

			} // END_FOREACH_OUTER

			return deleteSourceArrayList;
		} // END_METHOD

	} //END_CLASS
} // END_NS
