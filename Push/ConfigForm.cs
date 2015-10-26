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
		public AppSettings appSettings;
		public AppSettings originalAppSettings;

		// Form Construct...
		public ConfigForm()
		{
			InitializeComponent();
			this.AutoValidate  = AutoValidate.EnableAllowFocusChange;
		} // END_METHOD


		// Form Load...
		private void ConfigForm_Load(object sender, EventArgs e)
		{
			// Hydrate the controls with the current settings...
			cbHideDupeFileMessage.Checked = appSettings.HideDupeMessage.GetValueOrDefault(false);
			
			tbSourceFolder.Text = appSettings.SourcePath;
			tbTargetFolder.Text = appSettings.TargetPath;
			tbFileExtensions.Text = appSettings.FileExtensionFilter;
			cbDisableSplashScreen.Checked = appSettings.DisableSplashScreen.GetValueOrDefault(false);
			cbDisableXMLOptions.Checked = appSettings.DisableXMLOptions.GetValueOrDefault(false);
			
			// Save the original values in the appSettings...
			originalAppSettings = new AppSettings
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
			if (Helper.IsAppSettingsEmptyOrNull(appSettings))
			{
				FileInfo exePath = new FileInfo("Push.exe");
				appSettings.ExePath = exePath.DirectoryName;
			}
		} // END_METHOD

		
		// OK...
		private void btnOK_Click(object sender, EventArgs e)
		{
			ValidateConfigForm();
		} // END_METHOD


		// OK, Cancel & 'X' Event Handler...
		private void ConfigForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			if (DialogResult != DialogResult.Cancel)
			{
				//-------------------------------------------------------------
				// If we get here, the user did not select Cancel...

				appSettings.DisableSplashScreen = cbDisableSplashScreen.Checked;
				appSettings.DisableXMLOptions = cbDisableSplashScreen.Checked;
				appSettings.HideDupeMessage = cbHideDupeFileMessage.Checked;
				appSettings.ShowDetails = appSettings.ShowDetails.GetValueOrDefault(true);
				return;
			}

			//-----------------------------------------------------------------
			// If we get here, the user wants to discard all entered values...

			if (Helper.IsAppSettingsEmptyOrNull(originalAppSettings))
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
					// If we get here, the user wants to exit the configuration...
					return;
				}
				else
				{
					// If we get here, the user wants to finish configuration...
					e.Cancel = true;
					return;
				}
			} // END_IF_RESULT = CANCEL

			//-----------------------------------------------------------------
			// If were get here, the user wants to discard the settings they just
			//		made and restory the orginal settings...

			// Restore the original appSettings
			appSettings = originalAppSettings;
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

		
		private void rbCancel_CheckedChanged(object sender, EventArgs e)
		{
			appSettings.DuplicateFileAction = DuplicateFileActionState.Cancel.ToString("G");
		} // END_METHOD
	
		#endregion


		#region [ FILE EXTENSION FILTER ]

		// File Extension Changed
		private void tbFileExtensions_TextChanged(object sender, EventArgs e)
		{
			appSettings.FileExtensionFilter = tbFileExtensions.Text;
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


		#region [ SOURCE PATH ]

		// Source Path - Changed
		private void tbSourceFolder_TextChanged(object sender, EventArgs e)
		{
			appSettings.SourcePath = tbSourceFolder.Text;
		} // END_METHOD

		
		// Source Path - Clear...
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
