using Itenso.Configuration;

namespace Push
{
	// TODO: Move this to a different source code file...
	public class AppSettings : ApplicationSettings
	{
		public string DuplicateFileAction { get; set; }
		public string ExePath { get; set; }
		public string FileExtensionFilter { get; set; }
		public bool? HideDupeMessage { get; set; }
		public string SourcePath { get; set; }
		public string TargetPath { get; set; }
		public bool? ShowDetails { get; set; }


		public AppSettings()
			: base(typeof(AppSettings))
		{
			//-----------------------------------------------------------------
			// At this point, the AutoUpgrade property has been added and setto true...

			// IMPORTANT:  This code block DOES NOT add the value related to the property...
			Settings.Add(new PropertySetting(this, "DuplicateFileAction"));
			Settings.Add(new PropertySetting(this, "ExePath"));
			Settings.Add(new PropertySetting(this, "FileExtensionFilter"));
			Settings.Add(new PropertySetting(this, "HideDupeMessage"));
			Settings.Add(new PropertySetting(this, "SourcePath"));
			Settings.Add(new PropertySetting(this, "TargetPath"));
			Settings.Add(new PropertySetting(this, "ShowDetails"));

		} // END_METHOD

	} // END_CLASS

} // END_NS
