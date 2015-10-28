using System;
using System.Collections;
using System.IO;

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

				File.Copy(s, destFileName, true);

				copyCount++;
			}

			return new Tuple<int,int>(copyCount, 0);
		} // END_METHOD

	} // END_CLASS
} // END_NS
