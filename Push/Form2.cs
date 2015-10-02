using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
//---
using System.Web.Script.Serialization;

namespace Push
{
	public partial class Form2 : Form
	{

		public CheckBox DisplayDupeMessage { get; set; }
		
		public TextBox SourcePath { get; set; }
		public TextBox TargetPath { get; set; }
		public TextBox FileExtensionFilter { get; set; }
		
		public enum DuplicateFileActionState { OverWrite, Rename, Skip, Cancel };
		public string DuplicateFileAction;
		
		public CheckBox DisableSplashScreen { get; set; }
		public CheckBox DisableXMLOptions { get; set; }
		
		public Form2()
		{
			InitializeComponent();
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			DisplayDupeMessage = checkBox1;
			SourcePath = textBox1;
			TargetPath = textBox2;
			DuplicateFileAction = string.Empty; //new DuplicateFileActionState();
			FileExtensionFilter = textBox3;
			DisableSplashScreen = checkBox2;
			DisableXMLOptions = checkBox3;
		}		
		
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			DisplayDupeMessage = checkBox1;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				textBox1.Text = fbd.SelectedPath;
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			SourcePath = textBox1;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				textBox2.Text = fbd.SelectedPath;
			}
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			TargetPath = textBox2;
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			DuplicateFileAction = DuplicateFileActionState.OverWrite.ToString("G");
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			DuplicateFileAction = DuplicateFileActionState.Rename.ToString("G"); 
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			DuplicateFileAction = DuplicateFileActionState.Skip.ToString("G"); ;
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			DuplicateFileAction = DuplicateFileActionState.Cancel.ToString("G"); ;
		}

		private void textBox3_TextChanged(object sender, EventArgs e)
		{
			FileExtensionFilter = textBox3;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			textBox3.Clear();
		}

		private void button6_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
				textBox3.Text = sr.ReadToEnd();
				sr.Close();
			}
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			DisableSplashScreen = checkBox2;
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			DisableXMLOptions = checkBox3;
		}

		// OK...
		private void button1_Click(object sender, EventArgs e)
		{

			PushSettings settings = new PushSettings()
			{
				DisplayDupeMessage = DisplayDupeMessage.Checked,
				SourcePath = SourcePath.Text,
				TargetPath = TargetPath.Text,
				FileExtensionFilter = FileExtensionFilter.Text,
				DuplicateFileAction = DuplicateFileAction,
				DisableSplashScreen = DisableSplashScreen.Checked,
				DisableXMLOptions = DisableXMLOptions.Checked
			};

			string json = new JavaScriptSerializer().Serialize(settings);
			string path = @"C:\Win_SourceCode\PushApp\PushSettings";
			File.WriteAllText(path, json, System.Text.Encoding.ASCII);

		} // END_METHOD
	
	} // END_CLASS


	public class PushSettings
	{
		public bool DisplayDupeMessage;
		public string SourcePath;
		public string TargetPath;
		public string FileExtensionFilter;
		public string DuplicateFileAction;

		public bool DisableSplashScreen;
		public bool DisableXMLOptions;

	} // END_CLASS



}
