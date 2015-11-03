using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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

	}
}
