using System;
using System.Collections;
using System.IO;
//---
using System.Text.RegularExpressions;
using System.Threading;
//---
using System.ComponentModel;
//---
using System.Collections.Generic;


namespace Push
{
	public class CopyFileRename
	{
	
		public static void RenameDuplicates_BackGround(object sender, DoWorkEventArgs doWorkEventArgs)
		{
			BackgroundWorker bgWorker = sender as BackgroundWorker;

			List<string> all_FileSourceList = (List<string>)((List<object>)doWorkEventArgs.Argument)[0];
			List<string> all_FileTargetList = (List<string>)((List<object>)doWorkEventArgs.Argument)[1];
			AppSettings appSettings = (AppSettings)((List<object>)doWorkEventArgs.Argument)[2];

			string targetPath = appSettings.TargetPath;
			string sourcePath = appSettings.SourcePath;
			bool okToRename = false;
			ArrayList deleteSourceArrayList = new ArrayList();
			int suffixInteger = 0;
			int matchInteger = 0;
			int renameCount = 0;
			int copyCount = 0;

			string regExPattern = @"(?<Prefix>(\w*))\s*\((?<integer>\d*)\)";

			int all_SourceListCount = all_FileSourceList.Count;
			int all_SourceListCountComplete = 0;
			
			// Find all of the unique paths in the file source array list...
			List<string> uniqueSourcePathList = Helper.BuildUniquePathList(all_FileSourceList, sourcePath);

			// Create each path that does not exist in the target path...
			foreach (string uniqueSourcePath in uniqueSourcePathList)
				Helper.CreateTargetPaths(targetPath, uniqueSourcePath);

			// OUTER LOOP - Interate over each file in the source folder...
			foreach (string s in all_FileSourceList)
				{
				string sourceFileNamePrefix = Path.GetFileNameWithoutExtension(s);
				string sourceFileExtension = Path.GetExtension(s);
				string sourceFileName = s.Replace(appSettings.SourcePath, string.Empty);
	
				// INNER LOOP - Iterate over each file in the target folder...
				foreach (string t in all_FileTargetList)
					{
					matchInteger = 0;
					string targetFileName = t.Replace(appSettings.TargetPath, string.Empty);

					// Compair source and target filename...
					if (targetFileName.Equals(sourceFileName, StringComparison.Ordinal))
					{
						okToRename = true;
						continue;
					}

					//------------------------------------------------------------------------
					// If we get here, the source and target filenames are 
					//		not EXACTLY the same...

					// Compare the target file to the renamed file pattern 
					//		"file_name_prefix(integer)"...
					Match match = Regex.Match(targetFileName, regExPattern);
					if (!match.Success)
					{
						// If we get here, the target file is not a renamed file...
						continue;
					}

					//------------------------------------------------------------------------
					// If we get here, the target filename fits the file name prefix + integer
					//		rename pattern "file_name_prefix(integer)"...

					// Compare source and target file Prefix.
					// Compare source and target file Extension... 
					string prefix = match.Groups["Prefix"].Value;
					string targetFileExtension = Path.GetExtension(t);
					//--
					if (!prefix.Equals(sourceFileNamePrefix, StringComparison.Ordinal) ||
						!targetFileExtension.Equals(sourceFileExtension, StringComparison.Ordinal))
					{
						// If we get here, the target file name prefix does not match the source file prefix 
						//		-- OR -- The target file extension does not match the source file extension...
						//		Do nothing...
						continue;
					}

					//------------------------------------------------------------------------
					// If we get here, the file name prefixs and extensions match...

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

				#region [ COPY OR RENAME ]
				if (okToRename)
				{
					// Copy/Rename the source file to the target folder...
					string sourcefileNameNoExtension = Path.GetFileNameWithoutExtension(s);
					sourceFileName = Path.GetFileName(s);
					string newfileName = string.Format("{0} ({1}){2}", sourcefileNameNoExtension, ++suffixInteger, sourceFileExtension);
					string f = s.Replace(appSettings.SourcePath, appSettings.TargetPath);
					string destFileName = f.Replace(sourceFileName, newfileName);
					//---
					File.Copy(s, destFileName, false);
					File.Delete(s);
					//---
					renameCount++;
				}
				else
				{
					// Copy the source file to the target folder...
					string destFileName = s.Replace(appSettings.SourcePath, appSettings.TargetPath);
					//---
					File.Copy(s, destFileName, false);
					File.Delete(s);
					//---
					copyCount++;
				}// END_IF
				#endregion

				// Calculate Progress...
				int percentComplete = (int)Math.Round((double)(100 * all_SourceListCountComplete++) / all_SourceListCount);
				//---
				// Update Progress bar...
				bgWorker.ReportProgress(percentComplete);

				// Reset...
				okToRename = false;
				suffixInteger = 0;
				matchInteger = 0;

			} // END_OUTER_LOOP

			// Delete all source paths...
			Helper.DeleteSourcePaths(appSettings.SourcePath);

			// Update Progress bar...
			bgWorker.ReportProgress(100);

			doWorkEventArgs.Result = new Tuple<Helper.commandResult, int, int>(Helper.commandResult.Rename, copyCount, renameCount);

		} // END_METHOD

	} // END_CLASS
} // END_NS
