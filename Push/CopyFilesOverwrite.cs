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
	//public partial class MainForm
	public class CopyFilesOverwrite
	{
		//private void 
		public static Tuple<int> CopyOverwrite(ArrayList fileSourceArrayList, AppSettings appSettings)
		//private Tuple<int, int> CopyOverwrite(ArrayList fileSourceArrayList, string targetPath)
		{
			string targetPath = appSettings.TargetPath;
			int copyCount = 0;

			// Copy files...
			foreach (string s in fileSourceArrayList)
			{
				string srcfileName = Path.GetFileName(s);
				string destFileName = Path.Combine(targetPath, srcfileName);

				File.Copy(s, destFileName, true);

				//// Update UI...
				//lbStatus.Items.Add("Copying " + srcfileName + " to " + destFileName);
				//lbStatus.Update();

				copyCount++;
			}

			return new Tuple<int>(copyCount);

		} // END_METHOD


	}
}
