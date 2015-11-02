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
		public static Tuple<Helper.commandResult, int, int> RenameDulpicates(BackgroundWorker bgWorker, ArrayList fileSourceArrayList, string[] fileTargetStrArray, AppSettings appSettings)
		{
			string targetPath = appSettings.TargetPath;
			string sourcePath = appSettings.SourcePath;
			bool okToRename = false;
			ArrayList deleteSourceArrayList = new ArrayList();
			int suffixInteger = 0;
			int matchInteger = 0;
			int renameCount = 0;
			int copyCount = 0;

			string regExPattern = @"(?<Prefix>(\w*))\s*\((?<integer>\d*)\)";

			int sourceFileListCount = fileSourceArrayList.Count;
			int sourceFileListCountComplete = 0;

			// OUTER LOOP - Interate over each file in the source folder...
			foreach (string s in fileSourceArrayList)
			{
				string sourceFileNamePrefix = Path.GetFileNameWithoutExtension(s);
				string sourceFileName = Path.GetFileName(s);
				string sourceFileExtension = Path.GetExtension(s);

				#region [ DESIGN POINT ]
				/*		We must compare all target files to each source file to 
				 *		make sure that if more that one file with the same name
				 *		exists (renamed) that the highest rename number is found.  */
				#endregion

				// INNER LOOP - Iterate over each file in the target folder...
				foreach (string t in fileTargetStrArray)
				{
					matchInteger = 0;
					string targetFileName = Path.GetFileName(t);

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
					string sourcefileName = Path.GetFileNameWithoutExtension(s);
					string newfileName = string.Format("{0} ({1}){2}", sourcefileName, ++suffixInteger, sourceFileExtension);
					string destFileName = Path.Combine(targetPath, newfileName);
					//---
					File.Copy(s, destFileName, false);
					File.Delete(s);
					//---
					renameCount++;
				}
				else
				{
					// Copy the source file to the target folder...
					string sourcefileName = Path.GetFileName(s);
					string destFileName = Path.Combine(targetPath, sourcefileName);
					//---
					File.Copy(s, destFileName, false);
					File.Delete(s);
					//---
					copyCount++;
				}// END_IF
				#endregion

				// Calculate Progress...
				int percentComplete = (int)Math.Round((double)(100 * sourceFileListCountComplete++) / sourceFileListCount);
				//---
				// Update Progress bar...
				bgWorker.ReportProgress(percentComplete);

				// Reset...
				okToRename = false;
				suffixInteger = 0;
				matchInteger = 0;

			} // END_OUTER_LOOP

			return new Tuple<Helper.commandResult, int, int>(Helper.commandResult.Rename,	copyCount, renameCount);
		} // END_METHOD

		public static void RenameDuplicates_BackGround(object sender, DoWorkEventArgs doWorkEventArgs)
		{
			BackgroundWorker bgWorker = sender as BackgroundWorker;

			ArrayList fileSourceArrayList = (ArrayList)((List<object>)doWorkEventArgs.Argument)[0];
			string[] fileTargetStrArray = (string[])((List<object>)doWorkEventArgs.Argument)[1];
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

			int sourceFileListCount = fileSourceArrayList.Count;
			int sourceFileListCountComplete = 0;

			// OUTER LOOP - Interate over each file in the source folder...
			foreach (string s in fileSourceArrayList)
			{
				string sourceFileNamePrefix = Path.GetFileNameWithoutExtension(s);
				string sourceFileName = Path.GetFileName(s);
				string sourceFileExtension = Path.GetExtension(s);

				#region [ DESIGN POINT ]
				/*		We must compare all target files to each source file to 
				 *		make sure that if more that one file with the same name
				 *		exists (renamed) that the highest rename number is found.  */
				#endregion

				// INNER LOOP - Iterate over each file in the target folder...
				foreach (string t in fileTargetStrArray)
				{
					matchInteger = 0;
					string targetFileName = Path.GetFileName(t);

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
					string sourcefileName = Path.GetFileNameWithoutExtension(s);
					string newfileName = string.Format("{0} ({1}){2}", sourcefileName, ++suffixInteger, sourceFileExtension);
					string destFileName = Path.Combine(targetPath, newfileName);
					//---
					File.Copy(s, destFileName, false);
					File.Delete(s);
					//---
					renameCount++;
				}
				else
				{
					// Copy the source file to the target folder...
					string sourcefileName = Path.GetFileName(s);
					string destFileName = Path.Combine(targetPath, sourcefileName);
					//---
					File.Copy(s, destFileName, false);
					File.Delete(s);
					//---
					copyCount++;
				}// END_IF
				#endregion

				// Calculate Progress...
				int percentComplete = (int)Math.Round((double)(100 * sourceFileListCountComplete++) / sourceFileListCount);
				//---
				// Update Progress bar...
				bgWorker.ReportProgress(percentComplete);

				// Reset...
				okToRename = false;
				suffixInteger = 0;
				matchInteger = 0;

			} // END_OUTER_LOOP

			doWorkEventArgs.Result = new Tuple<Helper.commandResult, int, int>(Helper.commandResult.Rename, copyCount, renameCount);

		} // END_METHOD




	} // END_CLASS
} // END_NS
