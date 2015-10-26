using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Push
{
	public static class Helper
	{

		public static bool IsAppSettingsEmptyOrNull(AppSettings setting)
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
