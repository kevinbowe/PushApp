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
		public enum DuplicateFileActionState { OverWrite, Rename, Skip, Cancel };
		public PushSettings settings { get; set; }
		
		public Form2()
		{
			InitializeComponent();
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			// Hydrate the controls with the current settings...
			checkBox1.Checked = settings.DisplayDupeMessage;
			textBox1.Text = settings.SourcePath;
			textBox2.Text = settings.TargetPath;
			textBox3.Text = settings.FileExtensionFilter;
			checkBox2.Checked = settings.DisableSplashScreen;
			checkBox3.Checked = settings.DisableXMLOptions;

			switch (settings.DuplicateFileAction)
			{
				case "OverWrite":   radioButton1.Checked = true; break;
				case "Rename":      radioButton2.Checked = true; break;
				case "Skip":        radioButton3.Checked = true; break;
				case "Cancel":      
				default:            radioButton4.Checked = true; break;
			} // END_SWITCH
		}		
		
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			settings.DisplayDupeMessage = checkBox1.Checked;
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
			settings.SourcePath = textBox1.Text;
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
			settings.TargetPath = textBox2.Text;
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			settings.DuplicateFileAction = DuplicateFileActionState.OverWrite.ToString("G");
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			settings.DuplicateFileAction = DuplicateFileActionState.Rename.ToString("G");
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			settings.DuplicateFileAction = DuplicateFileActionState.Skip.ToString("G");
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			settings.DuplicateFileAction = DuplicateFileActionState.Cancel.ToString("G");		
		}

		private void textBox3_TextChanged(object sender, EventArgs e)
		{
			settings.FileExtensionFilter = textBox3.Text;
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
			settings.DisableSplashScreen = checkBox2.Checked;
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			settings.DisableXMLOptions = checkBox3.Checked;
		}

		// OK...
		private void button1_Click(object sender, EventArgs e)
		{
			string json = new JavaScriptSerializer().Serialize(settings);
			string path = @"C:\Win_SourceCode\PushApp\PushSettings";
			File.WriteAllText(path, json, System.Text.Encoding.ASCII);

		} // END_METHOD
	
	} // END_CLASS

}
