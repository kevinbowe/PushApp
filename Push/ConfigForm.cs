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
	public partial class ConfigForm : Form
	{
		public enum DuplicateFileActionState { Overwrite, Rename, Skip, Cancel };
		public MyApplicationSettings appSettings;
		public MyApplicationSettings originalAppSettings;

		public ConfigForm()
		{
			InitializeComponent();
			this.AutoValidate  = AutoValidate.EnableAllowFocusChange;
		} // END_METHOD


		// Form Load..
		private void ConfigForm_Load(object sender, EventArgs e)
		{
			// Hydrate the controls with the current settings...
			cbHideDupeFileMessage.Checked = appSettings.HideDupeMessage;
			tbSourceFolder.Text = appSettings.SourcePath;
			tbTargetFolder.Text = appSettings.TargetPath;
			tbFileExtensions.Text = appSettings.FileExtensionFilter;
			cbDisableSplashScreen.Checked = appSettings.DisableSplashScreen;
			cbDisableXMLOptions.Checked = appSettings.DisableXMLOptions;
			
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

				cbHideDupeFileMessage.Visible = false;
				grpDupeHandeling.Enabled = true;
				rbOverwrite.Enabled = rbRename.Enabled = rbSkip.Enabled = rbCancel.Enabled = true;
				rbOverwrite.Checked = rbRename.Checked = rbSkip.Checked = rbCancel.Checked = false;
			}
			else
			{
				switch (appSettings.DuplicateFileAction)
				{
					case "Overwrite": rbOverwrite.Checked = true; break;
					case "Rename": rbRename.Checked = true; break;
					case "Skip": rbSkip.Checked = true; break;
					case "Cancel":
					default:
						{
							rbCancel.Checked = true;
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
		private void ConfigForm_FormClosing(Object sender, FormClosingEventArgs e)
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
		private void btnOK_Click(object sender, EventArgs e)
		{
			// Validate ALL values before save...

			if (!IsValidFileExtensionFilters())
			{
				tbFileExtensions.Select(0, tbFileExtensions.Text.Length);
				errorProviderFileExtensions.SetError(tbFileExtensions, "Valid file extension filter is required");
				DialogResult = DialogResult.None;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProviderFileExtensions.SetError(tbFileExtensions, "");
				errorProviderFileExtensions.Dispose();
			}
			
			if (!rbOverwrite.Checked && !rbRename.Checked
						&& !rbSkip.Checked && !rbCancel.Checked)
			{
				errorProviderDuplicateHandeling.SetError(rbOverwrite, "Please choose a Duplicate Action");
				DialogResult = DialogResult.None;
			}
			else
				errorProviderDuplicateHandeling.Clear();
			
			
			if (!ValidatePath(tbSourceFolder.Text))
			{
				tbSourceFolder.Select(0, tbSourceFolder.Text.Length);
				errorProviderSourceFolder.SetError(tbSourceFolder, errorMsg);
				DialogResult = DialogResult.None;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProviderSourceFolder.SetError(tbSourceFolder, "");
				errorProviderSourceFolder.Dispose();
			}

			if (!ValidatePath(tbTargetFolder.Text))
			{
				tbSourceFolder.Select(0, tbTargetFolder.Text.Length);
				errorProviderTargetFolder.SetError(tbTargetFolder, errorMsg);
				DialogResult = DialogResult.None;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProviderTargetFolder.SetError(tbTargetFolder, "");
				errorProviderTargetFolder.Dispose();
			}

			if (String.IsNullOrEmpty(appSettings.FileExtensionFilter) ||
				String.IsNullOrEmpty(appSettings.SourcePath) ||
				String.IsNullOrEmpty(appSettings.TargetPath) )
			{
				DialogResult = DialogResult.None;
			}

			if (DialogResult == DialogResult.None)
				return;
		} // END_METHOD
		

		#region [ DUPLICATE RADIO BUTTONS ]

		private void rbOverwrite_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DuplicateFileAction = DuplicateFileActionState.Overwrite.ToString("G");
		} // END_METHOD


		private void rbRename_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DuplicateFileAction = DuplicateFileActionState.Rename.ToString("G");
		} // END_METHOD


		private void rbSkip_CheckedChanged(object sender, EventArgs e)
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
		private void tbFileExtensions_TextChanged(object sender, EventArgs e)
		{
			appSettings.FileExtensionFilter = tbFileExtensions.Text;
		} // END_METHOD


		private void tbFileExtensions_Validated(object sender, EventArgs e)
		{
			errorProviderFileExtensions.SetError(tbFileExtensions, "");
			errorProviderFileExtensions.Dispose();
		}


		private void tbFileExtensions_Validating(object sender, CancelEventArgs e)
		{
			if (!IsValidFileExtensionFilters())
			{
				e.Cancel = true;
				tbFileExtensions.Select(0, tbFileExtensions.Text.Length);
				this.errorProviderFileExtensions.SetError(tbFileExtensions, "One or more extensions are not correct.");
			}
		} // END_METHOD


		// Clear File Extension
		private void btnFileExtensionsClear_Click(object sender, EventArgs e)
		{
			tbFileExtensions.Clear();
		} // END_METHOD


		// Load File Extensions...
		private void btnFileExtensionsFileSelect_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();

			
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
				tbFileExtensions.Text = sr.ReadToEnd();
				sr.Close();
			}
		} // END_METHOD
		
		#endregion

		
		// Duplicate Message Checkbox...
		private void cbHideDupeFileMessage_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.HideDupeMessage = cbHideDupeFileMessage.Checked;

			if (cbHideDupeFileMessage.Checked)
			{
				// Enable Duplicate Action radio buttons...
				grpDupeHandeling.Enabled = true;
				rbOverwrite.Enabled = true;
				rbRename.Enabled = true;
				rbSkip.Enabled = true;
				rbCancel.Enabled = true;
			}
			else
			{
				grpDupeHandeling.Enabled = false;
				// Disable Duplicate Action radio buttons...
				rbOverwrite.Enabled = false;
				rbRename.Enabled = false;
				rbSkip.Enabled = false;
				rbCancel.Enabled = false;
			}
		} // END_METHOD		
		
		
		// Splash Screen...
		private void cbDisableSplashScreen_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DisableSplashScreen = cbDisableSplashScreen.Checked;
		} // END_METHOD

		
		// Disable XMP..
		private void cbDisableXMLOptions_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DisableXMLOptions = cbDisableXMLOptions.Checked;
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
			List<string> filterList = MainForm.LoadFileExtensions(appSettings);

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
		private void tbSourceFolder_TextChanged(object sender, EventArgs e)
		{
			appSettings.SourcePath = tbSourceFolder.Text;
		} // END_METHOD

		
		// Source Path - Validating...
		private void tbSourceFolder_Validating(object sender, CancelEventArgs e)
		{
			if (!ValidatePath(tbSourceFolder.Text))
			{
				e.Cancel = true;
				tbSourceFolder.Select(0, tbSourceFolder.Text.Length);
				this.errorProviderSourceFolder.SetError(tbSourceFolder, errorMsg);
			}
		} // END_METHOD

		
		// Source Path - Validated...
		private void tbSourceFolder_Validated(object sender, EventArgs e)
		{
			errorProviderSourceFolder.SetError(tbSourceFolder, "");
			errorProviderSourceFolder.Dispose();
		} // END_METHOD

	
		// Source Path - Clear...
		//private void button8_Click(object sender, EventArgs e)
		private void btnSourceFolderClear_Click(object sender, EventArgs e)
		{
			tbSourceFolder.Clear();
		} // END_METHOD

		
		// Source Path Browser...
		private void btnSourceFolderBrowse_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();

			if (fbd.ShowDialog() == DialogResult.OK)
			{
				tbSourceFolder.Text = appSettings.SourcePath = fbd.SelectedPath;

				// Clear the ErrorProvider of errors if present...
				errorProviderSourceFolder.SetError(tbSourceFolder, "");
				errorProviderSourceFolder.Dispose();
			}
		} // END_METHOD
		
		#endregion


		#region [ TARGET PATH ]

		// Target Path - Changed
		private void tbTargetFolder_TextChanged(object sender, EventArgs e)
		{
			appSettings.TargetPath = tbTargetFolder.Text;
		} // END_METHOD

		
		// Target Path - Validating...
		private void tbTargetFolder_Validating(object sender, CancelEventArgs e)
		{
			if (!ValidatePath(tbTargetFolder.Text))
			{
				e.Cancel = true;
				tbTargetFolder.Select(0, tbTargetFolder.Text.Length);
				this.errorProviderTargetFolder.SetError(tbTargetFolder, errorMsg);
			}
		} // END_METHOD

		
		// Target Path - Validated...
		private void tbTargetFolder_Validated(object sender, EventArgs e)
		{
			errorProviderTargetFolder.SetError(tbSourceFolder, "");
			errorProviderTargetFolder.Dispose();
		} // END_METHOD

		
		// Target Path - Clear...
		private void btnTargetClear_Click(object sender, EventArgs e)
		{
			tbTargetFolder.Clear();
		} // END_METHOD

		
		// Target Path Browser...
		private void btnTargetFolderBrowse_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				tbTargetFolder.Text = appSettings.TargetPath = fbd.SelectedPath;

				// Clear the ErrorProvider of errors if present...
				errorProviderTargetFolder.SetError(tbTargetFolder, "");
				errorProviderTargetFolder.Dispose();
			}
		}
		
		#endregion

	} // END_CLASS

} // END_NAMESPACE
