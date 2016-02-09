using System;
using System.Collections;
using System.IO;
//---
using System.ComponentModel;
using System.Threading;
//---
using System.Collections.Generic;

namespace Push
{
	public class CopyFileSkip
	{

		public static void SkipDuplicates_BackGround(object sender, DoWorkEventArgs doWorkEventArgs)
		{
			// Initialization
			BackgroundWorker bgWorker = sender as BackgroundWorker;
			List<string> all_FileSourceList = (List<string>)((List<object>)doWorkEventArgs.Argument)[0];
			List<string> all_FileTargetList = (List<string>)((List<object>)doWorkEventArgs.Argument)[1];
			AppSettings appSettings = (AppSettings)((List<object>)doWorkEventArgs.Argument)[2];
			int sourceFileListCount = all_FileSourceList.Count;
			int sourceFileListCountComplete = 0;
			string targetPath = appSettings.TargetPath;
			string sourcePath = appSettings.SourcePath;
			bool okToCopy = true;
			ArrayList deleteSourceArrayList = new ArrayList();
			int skipCount = 0;
			int copyCount = 0;

			// Find all of the unique paths in the file source array list...
			List<string> uniqueSourcePathList = Helper.BuildUniquePathList(all_FileSourceList, sourcePath);

			// Create each path that does not exist in the target path...
			foreach (string uniqueSourcePath in uniqueSourcePathList)
				Helper.CreateTargetPaths(targetPath, uniqueSourcePath);
			
			foreach (string s in all_FileSourceList)
			{
				FileInfo sourceFileInfo = new FileInfo(s);

				// INNER_LOOP -- Iterate over each file in the source list...
				foreach (string t in all_FileTargetList)
				{
					FileInfo targetFileInfo = new FileInfo(t);
					if (sourceFileInfo.Name.Equals(targetFileInfo.Name, StringComparison.Ordinal))
					{
						// If we get here, the file esists in the target folder.
						//      Skip this file...

						okToCopy = false;
						break; // Exit Inner loop...5
					} // END_IF

				} // END_FOREACH_INNER

				if (okToCopy)
				{
					// Copy the source file to the target folder...
					string sourcefileName = Path.GetFileName(s);
					string sourceFilePath = Path.GetDirectoryName(s);
					string destinationPath = sourceFilePath.Replace(sourcePath, targetPath);

					string destFileName = Path.Combine(destinationPath, sourcefileName);
					//---
					File.Copy(s, destFileName, true);
					File.Delete(s);
					//---
					copyCount++;
				}
				else
					skipCount++;

				// Calculate Progress...
				int percentComplete = (int)Math.Round((double)(100 * sourceFileListCountComplete++) / sourceFileListCount);
				//---
				// Update Progress bar...
				bgWorker.ReportProgress(percentComplete);
				
				// Raise the okToCopy flag...
				okToCopy = true;

			} // END_FOREACH_OUTER

			Helper.DeleteSourcePaths(appSettings.SourcePath);

			// Update Progress bar...
			bgWorker.ReportProgress(100);

			doWorkEventArgs.Result = new Tuple<Helper.commandResult, int, int>(Helper.commandResult.Skip, copyCount, skipCount);

		} // END_METHOD
	
	}
}
