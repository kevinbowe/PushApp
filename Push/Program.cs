using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Itenso.Configuration;

namespace Push
{
    static class Program
    {
		[STAThread]
		static void Main()
		{
			// This loads the properties, defined in Settings, with values...
			//		If the value of a property has not been defined it will be set to null...
			appSettings.Load();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Form form1 = new Form1(appSettings);
			// Assign the FormClosed Event handler...
			form1.FormClosed += FormClosed;

			Application.Run(form1);
			
			/* 
			 *  When the form is closed, the Form.OnClosing and Form.OnFormClosed 
			 *		events is executed.
			 * 
			 *	The Form.OnClosing event calls Itenso.Configuration.FormSettings.FormClosing( ). 
			 *	This method takes care of writing the form properties to the XML File.
			 * 
			 *	Next the Form.OnFormClosed event calls Program.FormClosed( ).
			 *	This method copies the PushSettings properties into the 
			 *		MyApplicatinSettings collection.   
			 */

			// Save the MyApplicationSettings to the same XML file that holds the form properties...
			appSettings.Save();

		} // Main


		// ----------------------------------------------------------------------
		static void FormClosed(object sender, FormClosedEventArgs e)
		{
			appSettings.DisableSplashScreen = ((Form1)sender).pushSettings.DisableSplashScreen;
			appSettings.DisableXMLOptions = ((Form1)sender).pushSettings.DisableXMLOptions;
			appSettings.DuplicateFileAction = ((Form1)sender).pushSettings.DuplicateFileAction;
			appSettings.ExePath = ((Form1)sender).pushSettings.ExePath;
			appSettings.FileExtensionFilter = ((Form1)sender).pushSettings.FileExtensionFilter;
			appSettings.HideDupeMessage = ((Form1)sender).pushSettings.HideDupeMessage;
			appSettings.SourcePath = ((Form1)sender).pushSettings.SourcePath;
			appSettings.TargetPath = ((Form1)sender).pushSettings.TargetPath;
		
		} // FormClosed

		// --------------------------------------------------------------------
		// members
		private static readonly MyApplicationSettings appSettings = new MyApplicationSettings();


    } // END_CLASS


	// ------------------------------------------------------------------------
	// TODO: Move this to a different source code file...
	public class MyApplicationSettings : ApplicationSettings
	{

		// --------------------------------------------------------------------
		public MyApplicationSettings() :
			base(typeof(MyApplicationSettings))
		{
			//-----------------------------------------------------------------
			// At this point, the AutoUpgrade property has been added and setto true...

			
			// IMPORTANT:  This DOES NOT add the value related to the property...

			Settings.Add(new PropertySetting(this, "DisableSplashScreen"));
			Settings.Add(new PropertySetting(this, "DisableXMLOptions"));
			Settings.Add(new PropertySetting(this, "DuplicateFileAction"));
			Settings.Add(new PropertySetting(this, "ExePath"));
			Settings.Add(new PropertySetting(this, "FileExtensionFilter"));
			Settings.Add(new PropertySetting(this, "HideDupeMessage"));
			Settings.Add(new PropertySetting(this, "SourcePath"));
			Settings.Add(new PropertySetting(this, "TargetPath"));

		} // MyApplicationSettings

		// --------------------------------------------------------------------
		public bool DisableSplashScreen { get; set; }
		public bool DisableXMLOptions  { get; set; }
		public string DuplicateFileAction  { get; set; }
		public string ExePath  { get; set; }
		public string FileExtensionFilter  { get; set; }
		public bool HideDupeMessage  { get; set; }
		public string SourcePath  { get; set; }
		public string TargetPath  { get; set; }


	} // class MyApplicationSettings




}
