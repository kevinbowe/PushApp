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
using System.ComponentModel;
using System.Threading;

namespace Push
{
	public class CopyFile
	{
		BackgroundWorker bgWorker;
	
		// Copy Files from Source folder to Target folder...
		public Tuple<Helper.commandResult, int, int> CopyFiles(MainForm mainForm)
		{
			AppSettings appSettings = mainForm.appSettings;

			ArrayList fileSourceArrayList = new ArrayList();
			Helper.commandResult DuplicateAction;

			Tuple<int, int> copyResult;


			// 
			bgWorker = new BackgroundWorker();
			//bgWorker.DoWork += new DoWorkEventHandler(bgDoWorkEventHandler);
			bgWorker.ProgressChanged += new ProgressChangedEventHandler(mainForm.bgProgressChangedEventHandler);
			bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgRunWorkerCompletedEventHandler);
			bgWorker.WorkerReportsProgress = true;
			bgWorker.WorkerSupportsCancellation = true;


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
				bgWorker.DoWork += new DoWorkEventHandler(bgDoWorkEventHandler);
				//copyResult = CopyFileOverwrite.CopyOverwrite(fileSourceArrayList, appSettings);
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
								//copyResult = CopyFileRename.RenameDulpicates(fileSourceArrayList, fileTargetStrArray, appSettings);
								break;

						case Helper.commandResult.Skip:
								//copyResult = CopyFileSkip.SkipDuplicates(fileSourceArrayList, fileTargetStrArray, appSettings);
								break;

						case Helper.commandResult.Cancel:
								return new Tuple<Helper.commandResult, int, int>(DuplicateAction, 0, 0);

						case Helper.commandResult.Overwrite:
						default:
								//copyResult = CopyFileOverwrite.CopyOverwrite(fileSourceArrayList, appSettings);
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
							bgWorker.DoWork += new DoWorkEventHandler(bgDoWorkEventHandlerRenameDulpicates);

								// Load DoWorkEvent Arguments
								args = new List<object>() { fileSourceArrayList, fileTargetStrArray, appSettings, mainForm };
								bgWorker.RunWorkerAsync(args);
								//copyResult = CopyFileRename.RenameDulpicates(fileSourceArrayList, fileTargetStrArray, appSettings);
								break;

						case Helper.commandResult.Skip:
								bgWorker.DoWork += new DoWorkEventHandler(bgDoWorkEventHandlerSkipDuplicates);

								args = new List<object>() { fileSourceArrayList, fileTargetStrArray, appSettings, mainForm };
								bgWorker.RunWorkerAsync(args);
								//copyResult = CopyFileSkip.SkipDuplicates(fileSourceArrayList, fileTargetStrArray, appSettings);
								break;

						case Helper.commandResult.Cancel:
								// __DEBUG_CODE__
								// bgWorker.RunWorkerAsync();
								return new Tuple<Helper.commandResult, int, int>(DuplicateAction, 0, 0);

						case Helper.commandResult.Overwrite:
						default:
								bgWorker.DoWork += new DoWorkEventHandler(bgDoWorkEventHandlerCopyOverwrite);
	
								args = new List<object>() { fileSourceArrayList, appSettings, mainForm };
								bgWorker.RunWorkerAsync(args);
								//copyResult = CopyFileOverwrite.CopyOverwrite(fileSourceArrayList, appSettings);
								break;

					} // END SWITCH
					#endregion

				} // END_IF_ELSE HideDupeMessage

			} // END_IF_ELSE DupeFileCount

			return new Tuple<Helper.commandResult, int, int>(DuplicateAction, 10101010, 90909090);
			//return new Tuple<Helper.commandResult, int, int>(DuplicateAction, copyResult.Item1, copyResult.Item2);
		} // END_METHOD


		void bgDoWorkEventHandler(object sender, DoWorkEventArgs e)
		{
			//Tuple<Helper.commandResult, int, int> copyFileResult = new CopyFile().CopyFiles(this);
			//UpdateStatus(copyFileResult);
			//// Update Source & Target Listboxes...
			//LoadListView(lvSource, appSettings.SourcePath);
			//LoadListView(lvTarget, appSettings.TargetPath);
			var doWorkEventArgs = e;

			Thread.Sleep(2500);

			e.Result = new Tuple<int, int>(202020, 505050);

		}


		void bgDoWorkEventHandlerCopyOverwrite(object sender, DoWorkEventArgs e)
		{
			var doWorkEventArgs = e;

			Thread.Sleep(2500);

			e.Result = new Tuple<int, int>(303030, 606060);

		}

		void bgDoWorkEventHandlerSkipDuplicates(object sender, DoWorkEventArgs e)
		{
			var doWorkEventArgs = e;

			Thread.Sleep(2500);

			e.Result = new Tuple<int, int>(404040, 707070);

		}


		void bgDoWorkEventHandlerRenameDulpicates(object sender, DoWorkEventArgs e)
		{
			var doWorkEventArgs = e;
			BackgroundWorker bgWorker = sender as BackgroundWorker;

			for (int i = 0; i < 100; i++)
			{
				bgWorker.ReportProgress(i /** 10*/); // Must return an integer representing the percent progress...

				Thread.Sleep(160);
			}

			e.Result = new Tuple<int,int>(505050,808080);
		}



		void bgRunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e) 
		{
			var obj = e.Result;

			MessageBox.Show(string.Format("{0} Tuple-1 {1} Tuple-2", ((Tuple<int, int>)obj).Item1, ((Tuple<int, int>)obj).Item2) );

			Console.WriteLine(); 
		}




	} //END_CLASS
} // END_NS
