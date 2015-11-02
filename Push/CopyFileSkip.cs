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
		public static Tuple<int, int> SkipDuplicates(ArrayList fileSourceArrayList, string[] fileTargetStrArray, AppSettings appSettings)
		{
			#region [ DESIGN POINT ]
			/*  If a duplicate is SKIPPED, it should NOT be deleted from the source folder. 
			 *  We need to return a revised list of files that are only the ones that should be deleted.
			 *  fileSourceArrayList */
			#endregion

			string targetPath = appSettings.TargetPath;
			string sourcePath = appSettings.SourcePath;		
			bool okToCopy = true;
			ArrayList deleteSourceArrayList = new ArrayList();
			int skipCount = 0;
			int copyCount = 0;

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
					//---
					File.Copy(s, destFileName, true);
					File.Delete(s);
					//---
					copyCount++;
				}
				else
					skipCount++;

				// Raise the okToCopy flag...
				okToCopy = true;

			} // END_FOREACH_OUTER

			return new Tuple<int, int>(copyCount, skipCount);
		} // END_METHOD


		public static void SkipDuplicates_BackGround(object sender, DoWorkEventArgs doWorkEventArgs)
		{
			#region [ DESIGN POINT ]
			/*  If a duplicate is SKIPPED, it should NOT be deleted from the source folder. 
			 *  We need to return a revised list of files that are only the ones that should be deleted.
			 *  fileSourceArrayList */
			#endregion

			BackgroundWorker bgWorker = sender as BackgroundWorker;

			ArrayList fileSourceArrayList = (ArrayList)((List<object>)doWorkEventArgs.Argument)[0];
			string[] fileTargetStrArray = (string[])((List<object>)doWorkEventArgs.Argument)[1];
			AppSettings appSettings = (AppSettings)((List<object>)doWorkEventArgs.Argument)[2];

			int sourceFileListCount = fileSourceArrayList.Count;
			int sourceFileListCountComplete = 0;
			
			
			string targetPath = appSettings.TargetPath;
			string sourcePath = appSettings.SourcePath;
			bool okToCopy = true;
			ArrayList deleteSourceArrayList = new ArrayList();
			int skipCount = 0;
			int copyCount = 0;

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

			//return new Tuple<int, int>(copyCount, skipCount);
			doWorkEventArgs.Result = new Tuple<Helper.commandResult, int, int>(Helper.commandResult.Skip, copyCount, skipCount);


		} // END_METHOD

	}
}
