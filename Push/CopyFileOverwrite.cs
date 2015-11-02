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
			
			ArrayList fileSourceArrayList = (ArrayList)((List<object>)doWorkEventArgs.Argument)[0];
			AppSettings appSettings = (AppSettings)((List<object>)doWorkEventArgs.Argument)[1];
			
			int sourceFileListCount = fileSourceArrayList.Count;
			int copyCount = 0;	

			string targetPath = appSettings.TargetPath;

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

				// Calculate Progress...
				int percentComplete = (int)Math.Round((double)(100 * copyCount) / sourceFileListCount);
				//---
				// Update Progress bar...
				bgWorker.ReportProgress(percentComplete);
			}

			doWorkEventArgs.Result = new Tuple<Helper.commandResult, int, int>(Helper.commandResult.Overwrite, copyCount, 0);
		} // END_METHOD




	} // END_CLASS
} // END_NS
