using System;
using System.Windows.Forms;

namespace Push
{
    static class Program
    {
		[STAThread]
		static void Main()
		{
			// This loads the properties, defined in AppSettings, with values...
			//		If the value of a property has not been defined, all properties will be set to null...
			appSettings.Load();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			MainForm mainForm = new MainForm(appSettings);
			
			// Assign the FormClosed Event handler...
			mainForm.FormClosed += FormClosed;

			// This handles the first-run + cancel scenario...
			if (mainForm.IsDisposed)
				return;

			Application.Run(mainForm);

			#region [ Comments ]
			/* When the form is closed, the Form.OnClosing and Form.OnFormClosed 
			 *		events is executed.
			 * 
			 *	The Form.OnClosing event calls Itenso.Configuration.FormSettings.FormClosing( ). 
			 *	This method takes care of writing the form properties to the XML File.
			 * 
			 *	Next the Form.OnFormClosed event calls Program.FormClosed( ).
			 *	This method copies the PushSettings properties into the 
			 *		AppSettings collection.   */
			#endregion			
			
			// Save the AppSettings to the same XML file that holds the form properties...
			appSettings.Save();

		} // END_METHOD


		static void FormClosed(object sender, FormClosedEventArgs e)
		{
			appSettings.DisableSplashScreen = ((MainForm)sender).appSettings.DisableSplashScreen;
			appSettings.DisableXMLOptions = ((MainForm)sender).appSettings.DisableXMLOptions;
			appSettings.DuplicateFileAction = ((MainForm)sender).appSettings.DuplicateFileAction;
			appSettings.ExePath = ((MainForm)sender).appSettings.ExePath;
			appSettings.FileExtensionFilter = ((MainForm)sender).appSettings.FileExtensionFilter;
			appSettings.HideDupeMessage = ((MainForm)sender).appSettings.HideDupeMessage;
			appSettings.SourcePath = ((MainForm)sender).appSettings.SourcePath;
			appSettings.TargetPath = ((MainForm)sender).appSettings.TargetPath;
			appSettings.ShowDetails = ((MainForm)sender).appSettings.ShowDetails;
		} // END_METHOD


		private static readonly AppSettings appSettings = new AppSettings();


    } // END_CLASS

} // END_NS
