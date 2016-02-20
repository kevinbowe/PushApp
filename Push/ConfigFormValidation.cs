using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
//---
using System.Text.RegularExpressions;


namespace Push
{
	public partial class ConfigForm : Form
	{
		public bool ValidateConfigForm()
		{
			bool isValid = true;

			#region [ CHECK FILE EXTENSION FILTERS ]
			if (!IsValidFileExtensionFilters())
			{
				tbFileExtensions.Select(0, tbFileExtensions.Text.Length);
				errorProviderFileExtensions.SetError(tbFileExtensions, "Valid file extension filter is required");
				DialogResult = DialogResult.None;
				isValid = false;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProviderFileExtensions.SetError(tbFileExtensions, "");
				errorProviderFileExtensions.Dispose();
			}
			#endregion

			#region [ CHECK DUPLICATE FILE ACTION ]
			if (!rbOverwrite.Checked && !rbRename.Checked
						&& !rbSkip.Checked && !rbCancel.Checked)
			{
				errorProviderDuplicateHandeling.SetError(rbOverwrite, "Please choose a Duplicate Action");
				DialogResult = DialogResult.None;
				isValid = false;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProviderDuplicateHandeling.Clear();
			}
			#endregion

			#region [ CHECK SOURCE FOLDER PATH ]
			if (!ValidatePath(tbSourceFolder.Text))
			{
				tbSourceFolder.Select(0, tbSourceFolder.Text.Length);
				errorProviderSourceFolder.SetError(tbSourceFolder, errorMsg);
				DialogResult = DialogResult.None;
				isValid = false;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProviderSourceFolder.SetError(tbSourceFolder, "");
				errorProviderSourceFolder.Dispose();
			}
			#endregion

			#region [ CHECK TARGET FOLDER PATH ]
			if (!ValidatePath(tbTargetFolder.Text))
			{
				tbSourceFolder.Select(0, tbTargetFolder.Text.Length);
				errorProviderTargetFolder.SetError(tbTargetFolder, errorMsg);
				DialogResult = DialogResult.None; 
				isValid = false;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProviderTargetFolder.SetError(tbTargetFolder, "");
				errorProviderTargetFolder.Dispose();
			}
			#endregion 

			#region [ COMPARE SOURCE AND TARGET FOLDER PATHS ]
			if (DuplicateOutputFolders(tbTargetFolder.Text, tbSourceFolder.Text))
			{
				tbTargetFolder.Select(0, tbTargetFolder.Text.Length);
				errorProviderTargetFolder.SetError(tbTargetFolder, errorMsg);
				//--
				tbSourceFolder.Select(0, tbSourceFolder.Text.Length);
				errorProviderSourceFolder.SetError(tbSourceFolder, errorMsg);
				//--
				DialogResult = DialogResult.None;
				isValid = false;
			}
			else
			{
				// If all conditions have been met, clear the ErrorProvider of errors.
				errorProviderTargetFolder.SetError(tbTargetFolder, "");
				errorProviderTargetFolder.Dispose();
				//--
				errorProviderSourceFolder.SetError(tbSourceFolder, "");
				errorProviderSourceFolder.Dispose();
			}
			#endregion 

			if (Helper.AppSettingsEmptyOrNull(appSettings))
			{
				DialogResult = DialogResult.None;
				isValid = false;
			}
			return isValid;
		}

		private bool DuplicateOutputFolders(string TargetFolder, string SourceFolder)
		{
			return SourceFolder.Trim().Equals(TargetFolder.Trim(), StringComparison.OrdinalIgnoreCase);
		} // END_METHOD

		#region [ FILE EXTENSIONS ]

		// File Extension...
		private void tbFileExtensions_Validated(object sender, EventArgs e)
		{
			errorProviderFileExtensions.SetError(tbFileExtensions, "");
			errorProviderFileExtensions.Dispose();
		} // END_METHOD

		private void tbFileExtensions_Validating(object sender, CancelEventArgs e)
		{
			if (!IsValidFileExtensionFilters())
			{
				e.Cancel = true;
				tbFileExtensions.Select(0, tbFileExtensions.Text.Length);
				this.errorProviderFileExtensions.SetError(tbFileExtensions, "One or more extensions are not correct.");
			}
		} // END_METHOD

		private bool IsValidFileExtensionFilters()
		{
			if (string.IsNullOrEmpty(appSettings.FileExtensionFilter))
				return false;

			// Parse the File Extension Filter string...
			List<string> filterList = Helper.LoadFileExtensions(appSettings);

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
		
		// Path Common
		string errorMsg = "Invalid Path";

		private bool ValidatePath(string path)
		{
			return Directory.Exists(path);
		} // END_METHOD

		#region [ SOURCE FOLDER ]

		// Source Path
		private void tbSourceFolder_Validating(object sender, CancelEventArgs e)
		{
			if (!ValidatePath(tbSourceFolder.Text))
			{
				e.Cancel = true;
				tbSourceFolder.Select(0, tbSourceFolder.Text.Length);
				this.errorProviderSourceFolder.SetError(tbSourceFolder, errorMsg);
			}

			// Compare Source and Target folders...
			if (DuplicateOutputFolders(tbSourceFolder.Text, tbTargetFolder.Text))
			{
				tbSourceFolder.Select(0, tbSourceFolder.Text.Length);
				this.errorProviderSourceFolder.SetError(tbSourceFolder, errorMsg);
				//--
				tbTargetFolder.Select(0, tbTargetFolder.Text.Length);
				this.errorProviderTargetFolder.SetError(tbTargetFolder, errorMsg);
				//--
				e.Cancel = true;
			}
		} // END_METHOD

		// Source Path
		private void tbSourceFolder_Validated(object sender, EventArgs e)
		{
			errorProviderSourceFolder.SetError(tbSourceFolder, "");
			errorProviderSourceFolder.Dispose();
		} // END_METHOD
		
		#endregion		

		#region [ TARGET FOLDER ]

		// Target Path
		private void tbTargetFolder_Validating(object sender, CancelEventArgs e)
		{
			if (!ValidatePath(tbTargetFolder.Text))
			{
				e.Cancel = true;
				tbTargetFolder.Select(0, tbTargetFolder.Text.Length);
				this.errorProviderTargetFolder.SetError(tbTargetFolder, errorMsg);
			}
			// Compare Source and Target folders...
			if (DuplicateOutputFolders(tbSourceFolder.Text, tbTargetFolder.Text))
			{
				tbSourceFolder.Select(0, tbSourceFolder.Text.Length);
				this.errorProviderSourceFolder.SetError(tbSourceFolder, errorMsg);
				//--
				tbTargetFolder.Select(0, tbTargetFolder.Text.Length);
				this.errorProviderTargetFolder.SetError(tbTargetFolder, errorMsg);
				//--
				e.Cancel = true;
			}
		} // END_METHOD

		// Target Path
		private void tbTargetFolder_Validated(object sender, EventArgs e)
		{
			errorProviderTargetFolder.SetError(tbSourceFolder, "");
			errorProviderTargetFolder.Dispose();
		} // END_METHOD
		
		#endregion
	}
}
