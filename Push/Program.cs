using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Itenso.Configuration;

namespace Push
{
    static class Program
    {
		///// <summary>
		///// The main entry point for the application.
		///// </summary>
		//[STAThread]
		//static void Main()
		//{
		//	Application.EnableVisualStyles();
		//	Application.SetCompatibleTextRenderingDefault(false);
		//	Application.Run(new Form1());
		//}

		[STAThread]
		static void Main()
		{
			appSettings.Load();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Form form1 = new Form1();
			form1.FormClosed += FormClosed;
			Application.Run(form1);

			appSettings.Save();
		} // Main


		// ----------------------------------------------------------------------
		static void FormClosed(object sender, FormClosedEventArgs e)
		{
			appSettings.SourcePath = ((Form1)sender).pushSettings.SourcePath;

		} // FormClosed

		// ----------------------------------------------------------------------
		// members
		private static readonly MyApplicationSettings appSettings = new MyApplicationSettings();


    } // END_CLASS


	// ------------------------------------------------------------------------
	public class MyApplicationSettings : ApplicationSettings
	{

		// ----------------------------------------------------------------------
		public MyApplicationSettings() :
			base(typeof(MyApplicationSettings))
		{
			Settings.Add(new PropertySetting(this, "SourcePath"));
		} // MyApplicationSettings

		// ----------------------------------------------------------------------
		public string SourcePath { get; set; }

	} // class MyApplicationSettings




}
