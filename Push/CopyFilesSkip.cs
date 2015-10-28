using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
//---
using PSTaskDialog;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
//---
using Itenso.Configuration;
using System.Globalization;

namespace Push
{
	public partial class MainForm
	{
		private Tuple<int, int> SkipDuplicates(ref ArrayList fileSourceArrayList, string[] fileTargetStrArray, string targetPath, string sourcePath)
		//...private void SkipDuplicates(ref ArrayList fileSourceArrayList, string[] fileTargetStrArray, string targetPath, string sourcePath)
		{
			#region [ DESIGN POINT ]
			/*  If a duplicate is SKIPPED, it should NOT be deleted from the source folder. 
			 *  We need to return a revised list of files that are only the ones that should be deleted.
			 *  fileSourceArrayList */
			#endregion

			bool okToCopy = true;
			ArrayList deleteSourceArrayList = new ArrayList();

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

					// Update the lisst of files that should be deleted from the source folder...
					//      Verify that 't' is the correct file name...
					deleteSourceArrayList.Add(s);

					//// Update UI...
					//lbStatus.Items.Add("Copying " + s + " to " + destFileName);
					//lbStatus.Update();
				}

				// Raise the okToCopy flag...
				okToCopy = true;

			} // END_FOREACH_OUTER

			fileSourceArrayList = deleteSourceArrayList;

			return new Tuple<int, int>(33, 44);

		} // END_METHOD

	}
}
