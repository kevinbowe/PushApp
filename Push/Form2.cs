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
			this.AutoValidate  = AutoValidate.EnableAllowFocusChange;
		} // END_METHOD


		// Form Load..
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
		} // END_METHOD


		// OK...
		private void button1_Click(object sender, EventArgs e)
		{
			// Validate ALL values before save...

			if (!ValidatePath(textBox1.Text))
			{
				textBox1.Select(0, textBox1.Text.Length);
				this.errorProvider1.SetError(textBox1, errorMsg);
				DialogResult = DialogResult.None;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProvider1.SetError(textBox1, "");
				errorProvider1.Dispose();
			}

			if (!ValidatePath(textBox2.Text))
			{
				textBox1.Select(0, textBox1.Text.Length);
				this.errorProvider2.SetError(textBox2, errorMsg);
				DialogResult = DialogResult.None;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProvider2.SetError(textBox2, "");
				errorProvider2.Dispose();
			}

			if (DialogResult == DialogResult.None)
				return;

			string json = new JavaScriptSerializer().Serialize(settings);
			string path = @"C:\SRC\PushApp\PushSettings";
			File.WriteAllText(path, json, System.Text.Encoding.ASCII);
		} // END_METHOD
		

		#region [ DUPLICATE RADIO BUTTONS ]

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			settings.DuplicateFileAction = DuplicateFileActionState.OverWrite.ToString("G");
		} // END_METHOD

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			settings.DuplicateFileAction = DuplicateFileActionState.Rename.ToString("G");
		} // END_METHOD

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			settings.DuplicateFileAction = DuplicateFileActionState.Skip.ToString("G");
		} // END_METHOD

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			settings.DuplicateFileAction = DuplicateFileActionState.Cancel.ToString("G");
		} // END_METHOD
	
		#endregion


		#region [ FILE EXTENSION FILTER ]

		// File Extension...
		private void textBox3_TextChanged(object sender, EventArgs e)
		{
			settings.FileExtensionFilter = textBox3.Text;
		} // END_METHOD

		// Clear File Extension
		private void button5_Click(object sender, EventArgs e)
		{
			textBox3.Clear();
		} // END_METHOD

		// Load File Extensions...
		private void button6_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
				textBox3.Text = sr.ReadToEnd();
				sr.Close();
			}
		} // END_METHOD
		
		#endregion

		
		// Duplicate Message Checkbox...
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			settings.DisplayDupeMessage = checkBox1.Checked;
		} // END_METHOD		
		
		// Splash Screen...
		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			settings.DisableSplashScreen = checkBox2.Checked;
		} // END_METHOD

		// Disable XMP..
		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			settings.DisableXMLOptions = checkBox3.Checked;
		} // END_METHOD


		#region [ VALIDATION ]

		//TODO: FIX THIS...
		// Add to resource file...
		string errorMsg = "Invalid Path";

		private bool ValidatePath(string path)
		{
			return Directory.Exists(path);
		} // END_METHOD
		
		#endregion


		#region [ SOURCE PATH ]

		// Source Path...
		private void textBox1_Validating(object sender, CancelEventArgs e)
		{
			if (!ValidatePath(textBox1.Text))
			{
				e.Cancel = true;
				textBox1.Select(0, textBox1.Text.Length);
				this.errorProvider1.SetError(textBox1, errorMsg);
			}
		} // END_METHOD

		// Source Path...
		private void textBox1_Validated(object sender, EventArgs e)
		{
			errorProvider1.SetError(textBox1, "");
			errorProvider1.Dispose();
		} // END_METHOD

		// Source Path - Clear...
		private void button8_Click(object sender, EventArgs e)
		{
			textBox1.Clear();
		} // END_METHOD

		// Source Path Browser...
		private void button3_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				textBox1.Text = settings.SourcePath = fbd.SelectedPath;

				// Clear the ErrorProvider of errors if present...
				errorProvider1.SetError(textBox1, "");
				errorProvider1.Dispose();
			}
		} // END_METHOD
		
		#endregion


		#region [ TARGET PATH ]

		// Target Path..
		private void textBox2_Validating(object sender, CancelEventArgs e)
		{
			if (!ValidatePath(textBox2.Text))
			{
				e.Cancel = true;
				textBox2.Select(0, textBox2.Text.Length);
				this.errorProvider2.SetError(textBox2, errorMsg);
			}

		} // END_METHOD

		// Target Path...
		private void textBox2_Validated(object sender, EventArgs e)
		{
			errorProvider2.SetError(textBox1, "");
			errorProvider2.Dispose();
		} // END_METHOD

		// Target Path - Clear...
		private void button7_Click(object sender, EventArgs e)
		{
			textBox2.Clear();
		} // END_METHOD

		// Target Path Browser...
		private void button4_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				textBox2.Text = settings.TargetPath = fbd.SelectedPath;

				// Clear the ErrorProvider of errors if present...
				errorProvider2.SetError(textBox2, "");
				errorProvider2.Dispose();
			}
		} // END_METHOD
		
		#endregion


	} // END_CLASS

} // END_NAMESPACE
