using System;
using System.Collections;
using System.IO;

namespace Push
{
	public class CopyFileSkip
	{
		public static Tuple<int, int> SkipDuplicates(ref ArrayList fileSourceArrayList, string[] fileTargetStrArray, AppSettings appSettings)
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

					File.Copy(s, destFileName, true);

					// Update the list of files that should be deleted from the source folder...
					deleteSourceArrayList.Add(s);

					copyCount++;
				}
				else
					skipCount++;

				// Raise the okToCopy flag...
				okToCopy = true;

			} // END_FOREACH_OUTER

			fileSourceArrayList = deleteSourceArrayList;

			return new Tuple<int, int>(copyCount, skipCount);

		} // END_METHOD

	}
}
