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
		public static Tuple<int,int> CopyOverwrite(ArrayList fileSourceArrayList, AppSettings appSettings)
		{
			string targetPath = appSettings.TargetPath;
			int copyCount = 0;

			// Copy files...
			foreach (string s in fileSourceArrayList)
			{
				string srcfileName = Path.GetFileName(s);
				string destFileName = Path.Combine(targetPath, srcfileName);
				//---
				File.Copy(s, destFileName, true);
				File.Delete(s);
				//---
				copyCount++;
			}

			return new Tuple<int,int>(copyCount, 0);
		} // END_METHOD


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

			List<string> uniqueSourcePathList = new List<string>();
			foreach (string s in all_FileSourceList)
			{
				// Grab the path fragment...
				string p = Path.GetDirectoryName(s);

				// Strip the source path from the current path fragment...
				//		If the remainder is empty, continue...
				string sp = p.Replace(sourcePath, string.Empty);
				if (string.IsNullOrEmpty(sp))
					continue;
				
				// Linq query that tests to see if the current path is in the path collection...
				if (uniqueSourcePathList.Any(e => e == sp))
					continue;

				// If we get here, add the subpath to the unique path list...
				uniqueSourcePathList.Add(sp);
			}

			// Create each path that does not exist in the target path...
			foreach (string uniqueSourcePath in uniqueSourcePathList)
				CreateTargetPaths(targetPath, uniqueSourcePath);

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

			doWorkEventArgs.Result = new Tuple<Helper.commandResult, int, int>(Helper.commandResult.Overwrite, copyCount, 0);
		}

		private static void CreateTargetPaths(string targetPath, string uniqueSourcePath)
		{
			if(string.IsNullOrEmpty(uniqueSourcePath))  return;

			// Build the path to check and create...
			
			// Grab the next folder in the pathFragment...
			string nextFolderArray = uniqueSourcePath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).First();
			
			// Remove the the nextFolder from the uniqueSourcePath...
			Regex regEx = new Regex(Regex.Escape("\\" + nextFolderArray));
			uniqueSourcePath = regEx.Replace(uniqueSourcePath, string.Empty, 1);
			
			// Append next folder to the targetPath...
			targetPath = targetPath + "\\" + nextFolderArray;

			if (!Directory.Exists(targetPath))
			{
				// If we get here, the path doesn't exist, so create it...

				Directory.CreateDirectory(targetPath);
			}

			CreateTargetPaths(targetPath, uniqueSourcePath);

			return;

		} // END_METHOD




	} // END_CLASS
} // END_NS
