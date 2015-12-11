using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
//
using System.Text.RegularExpressions;

namespace Push
{
	public static class Helper
	{

		public enum commandResult { Overwrite, Rename, Skip, Cancel, Fail };


		public static bool ValidateDataPaths(AppSettings appSettings)
		{
			// Validate the source and target folders...
			// Return true is either folder or both folders do not exist...
			return !Directory.Exists(appSettings.SourcePath) || !Directory.Exists(appSettings.TargetPath);
		} // END_METHOD


		public static bool ContainsFilterStarDotStar(string[] fefArray)
		{
			return fefArray.Where(f => f.Equals("*.*")).DefaultIfEmpty("").First() == "*.*";
		}


		// File Extensions...
		public static List<string> LoadFileExtensions(AppSettings appSettings)
		{
			/* NOTE:	
			 * The FOUR Linq queries below could be componded into one large query
			 * but it would make the code harder to read and debug.
			 * Performance is not dramatically affected.  */

			string[] delimiters = new string[] { ";", "|", ":" };

			// Split the FileExtensionFilter string into an array of strings.
			string[] fefArray = appSettings.FileExtensionFilter
						.Split(delimiters, StringSplitOptions.None)
						.ToArray();

			// Trim all leading and trailing spaces in each element...
			fefArray = fefArray.Select(fef => fef.Trim()).ToArray();

			// Scan for duplicate filters with Linq Query...
			fefArray = fefArray.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

			// Check for "*.*" filter. Discard all other filters since *.* rules...
			if (ContainsFilterStarDotStar(fefArray))
				fefArray = new string[] { "*.*" };

			return new List<string>(fefArray);
		} // END_METHOD


		public static bool AppSettingsEmptyOrNull(AppSettings setting)
		{
			bool result = string.IsNullOrEmpty(setting.DuplicateFileAction) &&
							string.IsNullOrEmpty(setting.ExePath) &&
							string.IsNullOrEmpty(setting.FileExtensionFilter) &&
							string.IsNullOrEmpty(setting.SourcePath) &&
							string.IsNullOrEmpty(setting.TargetPath) &&
							setting.DisableSplashScreen == null &&
							setting.DisableXMLOptions == null &&
							setting.HideDupeMessage == null &&
							setting.ShowDetails == null;
			return result;
		} // END_METHOD


		public static void DeleteSourcePaths(string pathArg)
		{
			foreach (string path in Directory.GetDirectories(pathArg))
			{
				// Recurse..
				DeleteSourcePaths(path);

				if (!Directory.GetFileSystemEntries(path).Any())
					Directory.Delete(path, false /*Recurse*/);
			}
		} // END_METHOD


		public static void CreateTargetPaths(string targetPath, string uniqueSourcePath)
		{
			if (string.IsNullOrEmpty(uniqueSourcePath)) return;

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


		public static List<string> BuildUniquePathList(List<string> all_FileSourceList, string sourcePath)
		{
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
			return uniqueSourcePathList;
		}



	}
}
