﻿using System;
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
	public partial class MainForm
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
							{
								Tuple<int, int> statusResult = RenameDulpicates(fileSourceArrayList, fileTargetStrArray, appSettings.TargetPath, appSettings.SourcePath);
								//...
								string status = string.Format("Copy={0} & Renamed={1}", statusResult.Item1, statusResult.Item2);
								lbStatus.Items.Add(status);
								lbStatus.Update();
								break;
							}

						case commandResult.Skip:
							{
								Tuple<int, int> statusResult = SkipDuplicates(ref fileSourceArrayList, fileTargetStrArray, appSettings.TargetPath, appSettings.SourcePath);
								//...
								string status = string.Format("Copy={0} & Skip={1}", statusResult.Item1, statusResult.Item2);
								lbStatus.Items.Add(status);
								lbStatus.Update();
								break;
							}
						case commandResult.Cancel:
							return;

						case commandResult.Overwrite:
						default:
							{
								Tuple<int, int> statusResult = CopyOverwrite(fileSourceArrayList, appSettings.TargetPath);
								//...
								string status = string.Format("Copy={0} & Overwrite={1}", statusResult.Item1, statusResult.Item2);
								lbStatus.Items.Add(status);
								lbStatus.Update();
								break;
							}

					} // END SWITCH
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
					#endregion					
					
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
							{
								Tuple<int, int> statusResult = RenameDulpicates(fileSourceArrayList, fileTargetStrArray, appSettings.TargetPath, appSettings.SourcePath);
								//...
								string status = string.Format("Copy={0} & Renamed={1}", statusResult.Item1, statusResult.Item2);
								lbStatus.Items.Add(status);
								lbStatus.Update();
								break;
							}

						case commandResult.Skip:
							{
								Tuple<int, int> statusResult = SkipDuplicates(ref fileSourceArrayList, fileTargetStrArray, appSettings.TargetPath, appSettings.SourcePath);
								//...
								string status = string.Format("Copy={0} & Skip={1}", statusResult.Item1, statusResult.Item2);
								lbStatus.Items.Add(status);
								lbStatus.Update();
								break;
							}
						case commandResult.Cancel:
							return;

						case commandResult.Overwrite:
						default:
							{
								Tuple<int, int> statusResult = CopyOverwrite(fileSourceArrayList, appSettings.TargetPath);
								//...
								string status = string.Format("Copy={0} & Overwrite={1}", statusResult.Item1, statusResult.Item2);
								lbStatus.Items.Add(status);
								lbStatus.Update();
								break;
							}

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

					break; // Exit innter loop...

				} // END_FOREACH_INNER
			} // END_FOREACH_OUTER
			#endregion

			// Update Source & Target Listboxes...
			LoadListView(lvSource, appSettings.SourcePath);
			LoadListView(lvTarget, appSettings.TargetPath);
		} // END_METHOD

	} //END_CLASS
} // END_NS
