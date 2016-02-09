using System;
using System.Collections;
using System.IO;
//---
using System.ComponentModel;
using System.Threading;
//---
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Push
{
	public class CopyFileOverwrite
	{

		public static void CopyOverwrite_BackGround(object sender, DoWorkEventArgs doWorkEventArgs)
		{
			BackgroundWorker bgWorker = sender as BackgroundWorker;

			List<string> all_FileSourceList = (List<string>)((List<object>)doWorkEventArgs.Argument)[0];

			AppSettings appSettings = (AppSettings)((List<object>)doWorkEventArgs.Argument)[1];
			
			int sourceFileListCount = all_FileSourceList.Count;
			int copyCount = 0;	

			string targetPath = appSettings.TargetPath;
			string sourcePath = appSettings.SourcePath;

			// Find all of the unique paths in the file source array list...
			List<string> uniqueSourcePathList = Helper.BuildUniquePathList(all_FileSourceList, sourcePath);

			// Create each path that does not exist in the target path...
			foreach (string uniqueSourcePath in uniqueSourcePathList)
				Helper.CreateTargetPaths(targetPath, uniqueSourcePath);

			// Copy files...
			foreach (string s in all_FileSourceList)
			{
				string destFileName = s.Replace(sourcePath, targetPath);
				//---
				File.Copy(s, destFileName, true);
				File.Delete(s);
				//---
				copyCount++;

				// Calculate Progress...
				int percentComplete = (int)Math.Round((double)(100 * copyCount) / sourceFileListCount);
				//---
				// Update Progress bar...
				bgWorker.ReportProgress(percentComplete);
			}

			// Delete all source paths...
			Helper.DeleteSourcePaths(appSettings.SourcePath);

			doWorkEventArgs.Result = new Tuple<Helper.commandResult, int, int>(Helper.commandResult.Overwrite, copyCount, 0);
		}


	} // END_CLASS
} // END_NS
