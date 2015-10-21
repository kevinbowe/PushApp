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
using System.Text.RegularExpressions;

namespace Push
{
	public partial class Form2 : Form
	{
		public enum DuplicateFileActionState { Overwrite, Rename, Skip, Cancel };
		public MyApplicationSettings appSettings;
		public MyApplicationSettings originalAppSettings;

		public Form2()
		{
			InitializeComponent();
			this.AutoValidate  = AutoValidate.EnableAllowFocusChange;
		} // END_METHOD


		// Form Load..
		private void Form2_Load(object sender, EventArgs e)
		{
			// Hydrate the controls with the current settings...
			checkBox1.Checked = appSettings.HideDupeMessage;
			textBox1.Text = appSettings.SourcePath;
			textBox2.Text = appSettings.TargetPath;
			textBox3.Text = appSettings.FileExtensionFilter;
			checkBox2.Checked = appSettings.DisableSplashScreen;
			checkBox3.Checked = appSettings.DisableXMLOptions;
			
			// Save the original values in the appSettings...
			originalAppSettings = new MyApplicationSettings
			{
				DisableSplashScreen = appSettings.DisableSplashScreen,
				DisableXMLOptions = appSettings.DisableXMLOptions,
				DuplicateFileAction = appSettings.DuplicateFileAction,
				ExePath = appSettings.ExePath,
				FileExtensionFilter = appSettings.FileExtensionFilter,
				HideDupeMessage = appSettings.HideDupeMessage,
				SourcePath = appSettings.SourcePath,
				TargetPath = appSettings.TargetPath
			};

			// Check to see if the Duplicate File Action has ever been set...
			if (appSettings.DuplicateFileAction == null)
			{
				// If we get here, the dupe action has never been set...

				checkBox1.Visible = false;
				groupBox1.Enabled = true;
				radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = radioButton4.Enabled = true;
				radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = radioButton4.Checked = false;
			}
			else
			{
				switch (appSettings.DuplicateFileAction)
				{
					case "Overwrite": radioButton1.Checked = true; break;
					case "Rename": radioButton2.Checked = true; break;
					case "Skip": radioButton3.Checked = true; break;
					case "Cancel":
					default:
						{
							radioButton4.Checked = true;
							break;
						}
				} // END_SWITCH
			}

			// Set the applications exe path...
			if (IsAppSettingsEmptyOrNull())
			{
				FileInfo exePath = new FileInfo("Push.exe");
				appSettings.ExePath = exePath.DirectoryName;
			}
		} // END_METHOD

		
		// OK, Cancel & 'X'...
		private void Form2_FormClosing(Object sender, FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.Cancel)
			{
				// If we get here, the user wants to discard all entered values...

				if (IsAppSettingsEmptyOrNull(originalAppSettings))
				{
					// If we get here, we are aborting the first-run, settings configuration...
					DialogResult dialogResult = MessageBox.Show( 
							this, 
							"Select Cancel to abort Application Setup\n\n"+
								"Select Retry to return to Application Setup", 
							"Cancel Push Application Setup", 
							MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);

					if (dialogResult == DialogResult.Cancel)
					{
						return;
					}
					else
					{
						// If we get here, the user wants to finish configuration...
						e.Cancel = true;
						return;
					}
				} // END_IF_RESULT = CANCEL

				// Restore the original appSettings
				appSettings = originalAppSettings;
				return;
			}

			//--------------------------------------------------------------------------
			// If we get here, the user did not select Cancel...

		} // END_METHOD

	
		private bool IsAppSettingsEmptyOrNull()
		{
			return IsAppSettingsEmptyOrNull(appSettings);
		} // END_METHOD

	
		private bool IsAppSettingsEmptyOrNull(MyApplicationSettings setting)
		{
			bool result =	string.IsNullOrEmpty(setting.DuplicateFileAction) &&
							string.IsNullOrEmpty(setting.ExePath) &&
							string.IsNullOrEmpty(setting.FileExtensionFilter) &&
							string.IsNullOrEmpty(setting.SourcePath) &&
							string.IsNullOrEmpty(setting.TargetPath);
			return result;
		} // END_METHOD

	
		// OK...
		private void button1_Click(object sender, EventArgs e)
		{
			// Validate ALL values before save...

			if (!IsValidFileExtensionFilters())
			{
				textBox3.Select(0, textBox3.Text.Length);
				errorProvider4.SetError(textBox3, "Valid file extension filter is required");
				DialogResult = DialogResult.None;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProvider4.SetError(textBox3, "");
				errorProvider4.Dispose();
			}
			
			if (!radioButton1.Checked && !radioButton2.Checked
						&& !radioButton3.Checked && !radioButton4.Checked)
			{
				errorProvider3.SetError(radioButton1, "Please choose a Duplicate Action");
				DialogResult = DialogResult.None;
			}
			else
				errorProvider3.Clear();
			
			
			if (!ValidatePath(textBox1.Text))
			{
				textBox1.Select(0, textBox1.Text.Length);
				errorProvider1.SetError(textBox1, errorMsg);
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
				textBox1.Select(0, textBox2.Text.Length);
				errorProvider2.SetError(textBox2, errorMsg);
				DialogResult = DialogResult.None;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProvider2.SetError(textBox2, "");
				errorProvider2.Dispose();
			}

			if (String.IsNullOrEmpty(appSettings.FileExtensionFilter) ||
				String.IsNullOrEmpty(appSettings.SourcePath) ||
				String.IsNullOrEmpty(appSettings.TargetPath) )
			{
				DialogResult = DialogResult.None;
			}

			if (DialogResult == DialogResult.None)
				return;
		}

		

		#region [ DUPLICATE RADIO BUTTONS ]
		
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DuplicateFileAction = DuplicateFileActionState.Overwrite.ToString("G");
		} // END_METHOD

		
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DuplicateFileAction = DuplicateFileActionState.Rename.ToString("G");
		} // END_METHOD

		
		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DuplicateFileAction = DuplicateFileActionState.Skip.ToString("G");
		} // END_METHOD

		
		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DuplicateFileAction = DuplicateFileActionState.Cancel.ToString("G");
		} // END_METHOD
	
		#endregion


		#region [ FILE EXTENSION FILTER ]

		// File Extension...
		private void textBox3_TextChanged(object sender, EventArgs e)
		{
			appSettings.FileExtensionFilter = textBox3.Text;
		} // END_METHOD


		private void textBox3_Validated(object sender, EventArgs e)
		{
			errorProvider4.SetError(textBox3, "");
			errorProvider4.Dispose();
		}


		private void textBox3_Validating(object sender, CancelEventArgs e)
		{
			if (!ValidatePath(textBox3.Text))
			{
				e.Cancel = true;
				textBox3.Select(0, textBox3.Text.Length);
				this.errorProvider4.SetError(textBox3, errorMsg);
			}
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
			appSettings.HideDupeMessage = checkBox1.Checked;

			if (checkBox1.Checked)
			{
				// Enable Duplicate Action radio buttons...
				groupBox1.Enabled = true;
				radioButton1.Enabled = true;
				radioButton2.Enabled = true;
				radioButton3.Enabled = true;
				radioButton4.Enabled = true;
			}
			else
			{
				groupBox1.Enabled = false;
				// Disable Duplicate Action radio buttons...
				radioButton1.Enabled = false;
				radioButton2.Enabled = false;
				radioButton3.Enabled = false;
				radioButton4.Enabled = false;
			}
		} // END_METHOD		
		
		
		// Splash Screen...
		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DisableSplashScreen = checkBox2.Checked;
		} // END_METHOD

		
		// Disable XMP..
		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DisableXMLOptions = checkBox3.Checked;
		} // END_METHOD


		#region [ VALIDATION ]

		//TODO: FIX THIS...
		// Add to resource file...
		string errorMsg = "Invalid Path";

		
		private bool ValidatePath(string path)
		{
			return Directory.Exists(path);
		} // END_METHOD


		private bool IsValidFileExtensionFilters()
		{
			if (string.IsNullOrEmpty(appSettings.FileExtensionFilter))
				return false;

			// Parse the File Extension Filter string...
			List<string> filterList = Form1.LoadFileExtensions(appSettings);

			#region [ DEBUG STRINGS_SAVE ]
			//// BAD_FILTERS
			//filterList/*_BAD*/ =
			//	new List<string>() { "", " ", "  ", ".*", "*.", "*.*text", "*.|*", "|",
			//						 "#.*", "*.#", "IMG*.JPG", "**.*", "***.*", "*.**",
			//						 "*.***", "**.*", "***.*", "*.**", "*.***", "IMG*.JPG",
			//						 "IMAGE**.JPG", "IMAGE**.*JPG", "*.JP*" };
			//// GOOD_FILTERS
			//filterList/*_GOOD*/ =
			//	new List<string>() { "*.*", "*.jpg", "*.JPEG", "*.tif", "*.TIFF", "*.BMP", 
			//						 "*.DOC", "*.WORD", "*.txt", "*.now", "*.Jpg_Good" };
			#endregion

			// Iterate over each filter looking for poorly formed filters...
			bool isValidFilExtensionFilters = true;
			string regExPattern = @"\*\.\w+|\*\.\*";

			foreach (string filter in filterList)
			{
				Match match = Regex.Match(filter, regExPattern);

				isValidFilExtensionFilters = IsValidExtensionFilter(match, filter);
				if (!isValidFilExtensionFilters)
				{
					// We would actually break here...
					break;
				}
			} // END_FOREACH

			return isValidFilExtensionFilters;
		} // END_METHOD


		private bool IsValidExtensionFilter(Match match, string filter)
		{
			if (match.Success)
			{
				// If we get here, a match was found...

				// Perform a Equals test to make sure there isn't a false positive.
				//		EG: *.JPG would match but the input is IMG*.JPG. == False Positive...
				if (!filter.Equals(match.Value, StringComparison.CurrentCultureIgnoreCase))
				{
					// If we get here, the match and the actual string are NOT the same.
					//		False Positive...
					return false;
				}
			}
			else
			{
				// If we get here, no match was found...
				return false;
			}

			// If we get here, the filter is well formed / valid...
			return true;
		} // END_METHOD
		
		#endregion


		#region [ SOURCE PATH ]

		// Source Path - Changed
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			appSettings.SourcePath = textBox1.Text;
		} // END_METHOD

		
		// Source Path - Validating...
		private void textBox1_Validating(object sender, CancelEventArgs e)
		{
			if (!ValidatePath(textBox1.Text))
			{
				e.Cancel = true;
				textBox1.Select(0, textBox1.Text.Length);
				this.errorProvider1.SetError(textBox1, errorMsg);
			}
		} // END_METHOD

		
		// Source Path - Validated...
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
				textBox1.Text = appSettings.SourcePath = fbd.SelectedPath;

				// Clear the ErrorProvider of errors if present...
				errorProvider1.SetError(textBox1, "");
				errorProvider1.Dispose();
			}
		} // END_METHOD
		
		#endregion


		#region [ TARGET PATH ]

		// Target Path - Changed
		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			appSettings.TargetPath = textBox2.Text;
		} // END_METHOD

		
		// Target Path - Validating...
		private void textBox2_Validating(object sender, CancelEventArgs e)
		{
			if (!ValidatePath(textBox2.Text))
			{
				e.Cancel = true;
				textBox2.Select(0, textBox2.Text.Length);
				this.errorProvider2.SetError(textBox2, errorMsg);
			}
		} // END_METHOD

		
		// Target Path - Validated...
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
				textBox2.Text = appSettings.TargetPath = fbd.SelectedPath;

				// Clear the ErrorProvider of errors if present...
				errorProvider2.SetError(textBox2, "");
				errorProvider2.Dispose();
			}
		}
		
		#endregion

	} // END_CLASS

} // END_NAMESPACE
