using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;


namespace Push
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Hydrate the Source and Target Listboxes
            LoadSource();
            LoadTarget();
        } // END_METHOD

        // Load Target ListView...
        private bool LoadTarget()
        {
            // Fetch all of the files in the source filder...
            string targetPath = @"T:\";  //string targetPath = @"C:\DEV_TARGET";

            if (!Directory.Exists(targetPath))
            {
                listBox1.Items.Add("The Target path do not exist!");
                return false;
            }


            string[] filesStrArray = Directory.GetFiles(targetPath);

            listView2.Items.Clear();

            // File extension types
            ArrayList FileExtensionArrayList = new ArrayList() { "TIFF", "TIF", "JPGE", "JPG" };

            ArrayList fileSourceArrayList = new ArrayList();

            foreach (string ext in FileExtensionArrayList)
            {
                // Create extension...
                string fileExtension = "*." + ext;
                string[] fileSourceStrArray = Directory.GetFiles(targetPath, fileExtension);

                if (fileSourceStrArray.Length <= 0) continue;

                foreach (string s in fileSourceStrArray) fileSourceArrayList.Add(s);

            } // END_FOREACH

            foreach (string s in fileSourceArrayList/*filesStrArray*/)
            {

                FileInfo targetFileInfo = new FileInfo(s);

                string friendlyFileSize = StrFormatByteSize(targetFileInfo.Length);
                DateTime fileDate = File.GetCreationTime(s);
                string friendlyFileType = GetFileTypeDescription(s);
                string fileName = Path.GetFileNameWithoutExtension(targetFileInfo.Name);

                ListViewItem item = new ListViewItem(fileName);
                item.SubItems.Add(friendlyFileType);
                item.SubItems.Add(friendlyFileSize);
                item.SubItems.Add(fileDate.ToString("MM/dd/yyyy h:mm tt"));
                // 
                ListViewItem[] itemArray = new ListViewItem[] { item };
                listView2.Items.Add(item);
            }

            return true;
        } // END_METHOD
        
        // Load Source ListView...
        private bool LoadSource()
        {
            
            // Fetch all of the files in the source filder...
            string sourcePath = @"S:\";
            
            if (!Directory.Exists(sourcePath))
            {
                listBox1.Items.Add("The Source path do not exist!");
                return false;
            }
            
            string[] filesStrArray = Directory.GetFiles(sourcePath);

            //listBox2.Items.Clear();

            listView1.Items.Clear();

            // File extension types
            ArrayList FileExtensionArrayList = new ArrayList() { "TIFF", "TIF", "JPGE", "JPG" };

            ArrayList fileSourceArrayList = new ArrayList();

            foreach (string ext in FileExtensionArrayList)
            {
                // Create extension...
                string fileExtension = "*." + ext;
                string[] fileSourceStrArray = Directory.GetFiles(sourcePath, fileExtension);

                if (fileSourceStrArray.Length <= 0) continue;
                
                foreach (string s in fileSourceStrArray) fileSourceArrayList.Add(s);
            } // END_FOREACH

            foreach (string s in fileSourceArrayList/*filesStrArray*/)
            {
                FileInfo sourceFileInfo = new FileInfo(s);
                string friendlyFileSize = StrFormatByteSize(sourceFileInfo.Length);
                DateTime fileDate = File.GetCreationTime(s);
                string friendlyFileType = GetFileTypeDescription(s);
                string fileName = Path.GetFileNameWithoutExtension(sourceFileInfo.Name);

                ListViewItem item = new ListViewItem(fileName);
                item.SubItems.Add(friendlyFileType); 
                item.SubItems.Add(friendlyFileSize);
                item.SubItems.Add(fileDate.ToString("MM/dd/yyyy h:mm tt"));
                // 
                ListViewItem[] itemArray = new ListViewItem[] {item};
                listView1.Items.Add(item);
            }

            return true;
        } // END_METHOD

        // Copy Files from Source folder to Target folder...
        private void button1_Click(object sender, EventArgs e)
        {
            // Decl...
            string srcfileName;
            string destFileName;
            ArrayList fileSourceArrayList = new ArrayList();
            
            // Paths...
            string sourcePath = @"S:\";  //string sourcePath = @"C:\DEV_SOURCE";
            string targetPath = @"T:\";  //string targetPath = @"C:\DEV_TARGET";
            //string targetPath = @"\\Ml\XP_TARGET";

            // Validation..
            if (!Directory.Exists(sourcePath) || !Directory.Exists(targetPath) )
            {
                listBox1.Items.Add("The Source or Target path do not exist!");
                return;
            }

            // Init Controls...
            listBox1.Items.Clear();

            // File extension types...
            ArrayList FileExtensionArrayList = new ArrayList() {"TIFF","TIF","JPGE","JPG"};
            
            // Build list of file to copy... 
            foreach(string ext in FileExtensionArrayList)
            {
                // Create extension...
                string fileExtension = "*." + ext;
                string[] fileSourceStrArray = Directory.GetFiles(sourcePath, fileExtension);

                if (fileSourceStrArray.Length <= 0) 
                    continue;
   
                foreach (string s in fileSourceStrArray) 
                    fileSourceArrayList.Add(s);
            } // END_FOREACH



            // Build a list of files on the target folder...
            string[] fileTargetStrArray = System.IO.Directory.GetFiles(targetPath);
            int dupeFileCount = 0;
            // OUTER LOOP -- Iterate over each file in the target list...
            foreach (string t in fileTargetStrArray)
            {
                FileInfo targetFileInfo = new FileInfo(t);

                // INNER_LOOP -- Iterate over each file in the source list...
                foreach (string s in fileSourceArrayList)
                {
                    FileInfo sourceFileInfo = new FileInfo(s);

                    // Compare the fileName.ext (ignore the path)...
                    if (!targetFileInfo.Name.Equals(sourceFileInfo.Name, StringComparison.Ordinal))
                    {
                        continue;
                    }
                    ++dupeFileCount;
                    break; // Exit innter loop...

                } // END_FOREACH_INNER
            } // END_FOREACH_OUTER


            if (dupeFileCount > 0)
            {
                // ADD TaskDialog HERE...

                PSTaskDialog.cTaskDialog.ForceEmulationMode = checkBox1.Checked;
                try { PSTaskDialog.cTaskDialog.EmulatedFormWidth = Convert.ToInt32(edWidth.Text); }
                catch (Exception) { PSTaskDialog.cTaskDialog.EmulatedFormWidth = 450; }

                DialogResult res =
                  PSTaskDialog.cTaskDialog.ShowTaskDialogBox(
                        this,
                        "Duplicate Files Found",
                        "There were {0} duplicate files found in the Target Folder.",
                        "What would you like to do?",
                        "Renamed files will have the format: original_File_Name(n).ext, where (n) is a nemeric value.  When multiple copies exist the latest duplicate will always have the highest value.\n\nThese settings may be modified in the Configuration Dialog.",
                    //"Optional footer text with an icon can be included",
                        string.Empty,
                        "Don't show me this message again",
                    //"Radio Option 1|Radio Option 2|Radio Option 3",
                        string.Empty,
                    //"Command &Button 1|Command Button 2|Command Button 3|Command Button 4|Command Button 5",
                        "Overwrite All Duplicates|Copy/Rename All Duplicates|Skip All Duplicates|Cancel Copy",
                    //...PSTaskDialog.eTaskDialogButtons.OKCancel,
                        PSTaskDialog.eTaskDialogButtons.None,
                        PSTaskDialog.eSysIcons.Information,
                        PSTaskDialog.eSysIcons.Warning);
//                UpdateResult(res);

                    lbResult.Text = "Result : " + Enum.GetName(typeof(DialogResult), res) + Environment.NewLine +
                    "RadioButtonIndex : " + PSTaskDialog.cTaskDialog.RadioButtonResult.ToString() + Environment.NewLine +
                    "CommandButtonIndex : " + PSTaskDialog.cTaskDialog.CommandButtonResult.ToString() + Environment.NewLine +
                    "Verify CheckBox : " + (PSTaskDialog.cTaskDialog.VerificationChecked ? "true" : "false");

                    // Force exit... << DEBUG ONLY >>
                    return;

                //string message = string.Format("There are {0} duplicate files in the target folder. \n\n", dupeFileCount);
                //message += "Select Cancel to stop copy";

                //string caption = "Duplicates Found";
                //MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
                ////...MessageBoxButtons buttons = MessageBoxButtons.YesNo; 
                //DialogResult result;

                //// Displays the MessageBox.
                //result = MessageBox.Show(message, caption, buttons);

                //if (result == System.Windows.Forms.DialogResult.Cancel)
                //    return;
            }










            // Copy files...
            foreach (string s in fileSourceArrayList )
            {
                srcfileName = Path.GetFileName(s);
                destFileName = Path.Combine(targetPath, srcfileName);
                
                File.Copy(s, destFileName, true);

                // Update UI...
                listBox1.Items.Add("Copying " + srcfileName + " to " + destFileName);
                listBox1.Update();
            }

            /*----------------------------------------------------------------- 
             * If we get here, all of the files have been copied from the source folder to 
             * the target folder.
             * 
             * Now verify and remove each file has been copied.
             *----------------------------------------------------------------*/

            // Build a list of files on the target folder...
            //string[] fileTargetStrArray = System.IO.Directory.GetFiles(targetPath);

            // OUTER LOOP -- Iterate over each file in the target list...
            foreach (string t in fileTargetStrArray)
            {
                FileInfo targetFileInfo = new FileInfo(t);

                // INNER_LOOP -- Iterate over each file in the source list...
                foreach (string s in fileSourceArrayList)
                {
                    FileInfo sourceFileInfo = new FileInfo(s);

                    // Compare the fileName.ext (ignore the path)...
                    if (!targetFileInfo.Name.Equals(sourceFileInfo.Name, StringComparison.Ordinal))
                        continue;

                    // Compare size and create date...
                    // TODO: Try using CRC Checksum later...
                    if (sourceFileInfo.Length != targetFileInfo.Length)
                        break;

                    //---------------------------------------------------------
                    // If we get here, the files match...

                    // Delete the source file...
                    File.Delete(s);

                    // Update UI...
                    listBox1.Items.Add("CleanUp: Deleting " + s);
                    listBox1.Update();
                    break; // Exit innter loop...
                
                } // END_FOREACH_INNER
            } // END_FOREACH_OUTER

            listBox1.Items.Add("Copy Complete");
            listBox1.Update();

            // Update Source & Target Listboxes...
            LoadSource();
            LoadTarget();

        } // END_METHOD

        // DEVELOPMENT ONLY -- Reset Application...
        private void button2_Click(object sender, EventArgs e){

            //-----------------------------------------------------------------
            //  Clear the Target Folder
            
            //... NOTE: This value MUST be fetched from the configuration data...
            string targetPath = @"T:\";
            
            string[] filesStrArray = Directory.GetFiles(targetPath);
            foreach (string s in filesStrArray) 
                File.Delete(s);

            //-----------------------------------------------------------------
            //  Clear the Source Folder

            //... NOTE: This value MUST be fetched from the configuration data...
            string sourcePath = @"S:\";

            filesStrArray = Directory.GetFiles(sourcePath);
            foreach (string t in filesStrArray) 
                File.Delete(t);

            //-----------------------------------------------------------------
            //  Initialize the Source Folder with Test Data

            string testDataPath = @"C:\DEV_TESTDATA\Pictures";


            // File extension types
            ArrayList FileExtensionArrayList = new ArrayList() { "TIFF", "TIF", "JPGE", "JPG" };

            if (!Directory.Exists(sourcePath) || !Directory.Exists(targetPath))
            {
                listBox1.Items.Add("The Source or Target path do not exist!");
                return;
            }

            ArrayList fileTestDataArrayList = new ArrayList();

            foreach (string ext in FileExtensionArrayList)
            {
                // Create extension...
                string fileExtension = "*." + ext;
                string[] fileTestDataStrArray = Directory.GetFiles(testDataPath, fileExtension);

                if (fileTestDataStrArray.Length <= 0) 
                    continue;

                foreach (string s in fileTestDataStrArray) 
                    fileTestDataArrayList.Add(s);
            } // END_FOREACH

            string testDataFileName;
            string destFileName;
            foreach (string s in fileTestDataArrayList)
            {
                // Use static Path methods to extract only the file name from the path.
                testDataFileName = Path.GetFileName(s);
                destFileName = Path.Combine(sourcePath, testDataFileName);
                File.Copy(s, destFileName, true);
            }

            // Hydrate the Source and Target Listboxes
            LoadSource();

            LoadTarget();
        }

        // Empty... 
        private void Form1_Load(object sender, EventArgs e)
        {

        } // END_METHOD

        #region Constants

        private const uint FILE_ATTRIBUTE_READONLY = 0x00000001;
        private const uint FILE_ATTRIBUTE_HIDDEN = 0x00000002;
        private const uint FILE_ATTRIBUTE_SYSTEM = 0x00000004;
        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        private const uint FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
        private const uint FILE_ATTRIBUTE_DEVICE = 0x00000040;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
        private const uint FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
        private const uint FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
        private const uint FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
        private const uint FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
        private const uint FILE_ATTRIBUTE_OFFLINE = 0x00001000;
        private const uint FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
        private const uint FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
        private const uint FILE_ATTRIBUTE_VIRTUAL = 0x00010000;

        private const uint SHGFI_ICON = 0x000000100;     // get icon
        private const uint SHGFI_DISPLAYNAME = 0x000000200;     // get display name
        private const uint SHGFI_TYPENAME = 0x000000400;     // get type name
        private const uint SHGFI_ATTRIBUTES = 0x000000800;     // get attributes
        private const uint SHGFI_ICONLOCATION = 0x000001000;     // get icon location
        private const uint SHGFI_EXETYPE = 0x000002000;     // return exe type
        private const uint SHGFI_SYSICONINDEX = 0x000004000;     // get system icon index
        private const uint SHGFI_LINKOVERLAY = 0x000008000;     // put a link overlay on icon
        private const uint SHGFI_SELECTED = 0x000010000;     // show icon in selected state
        private const uint SHGFI_ATTR_SPECIFIED = 0x000020000;     // get only specified attributes
        private const uint SHGFI_LARGEICON = 0x000000000;     // get large icon
        private const uint SHGFI_SMALLICON = 0x000000001;     // get small icon
        private const uint SHGFI_OPENICON = 0x000000002;     // get open icon
        private const uint SHGFI_SHELLICONSIZE = 0x000000004;     // get shell size icon
        private const uint SHGFI_PIDL = 0x000000008;     // pszPath is a pidl
        private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;     // use passed dwFileAttribute
       
        #endregion

        public static string GetFileTypeDescription(string fileNameOrExtension)
        {
            SHFILEINFO shfi;
            if (IntPtr.Zero != SHGetFileInfo(
                                fileNameOrExtension,
                                FILE_ATTRIBUTE_NORMAL,
                                out shfi,
                                (uint)Marshal.SizeOf(typeof(SHFILEINFO)),
                                SHGFI_USEFILEATTRIBUTES | SHGFI_TYPENAME))
            {
                return shfi.szTypeName;
            }
            return null;
        }

        [DllImport("shell32")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, uint flags);

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        
        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(
                long fileSize
                , [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer
                , int bufferSize);

        /// <summary>
        /// Converts a numeric value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
        /// </summary>
        /// <param name="filelength">The numeric value to be converted.</param>
        /// <returns>the converted string</returns>
        public static string StrFormatByteSize(long filesize)
        {
            StringBuilder sb = new StringBuilder(11);
            StrFormatByteSize(filesize, sb, sb.Capacity);
            return sb.ToString();
        }

        private void TestOnly_Click(object sender, EventArgs e)
        {
            PSTaskDialog.cTaskDialog.ForceEmulationMode = checkBox1.Checked;
            try { PSTaskDialog.cTaskDialog.EmulatedFormWidth = Convert.ToInt32(edWidth.Text); }
            catch (Exception) { PSTaskDialog.cTaskDialog.EmulatedFormWidth = 450; }

            DialogResult res =
              PSTaskDialog.cTaskDialog.ShowTaskDialogBox(
                    this,
                    "Duplicate Files Found",
                    "There were {0} duplicate files found in the Target Folder.",
                    "What would you like to do?",
                    "Renamed files will have the format: original_File_Name(n).ext, where (n) is a nemeric value.  When multiple copies exist the latest duplicate will always have the highest value.\n\nThese settings may be modified in the Configuration Dialog.",
                    //"Optional footer text with an icon can be included",
                    string.Empty,
                    "Don't show me this message again",
                    //"Radio Option 1|Radio Option 2|Radio Option 3",
                    string.Empty,
                    //"Command &Button 1|Command Button 2|Command Button 3|Command Button 4|Command Button 5",
                    "Overwrite All Duplicates|Copy/Rename All Duplicates|Skip All Duplicates|Cancel Copy",
                    //...PSTaskDialog.eTaskDialogButtons.OKCancel,
                    PSTaskDialog.eTaskDialogButtons.None,
                    PSTaskDialog.eSysIcons.Information,
                    PSTaskDialog.eSysIcons.Warning);
                    UpdateResult(res);  
        }
        
        //--------------------------------------------------------------------------------
        void UpdateResult(DialogResult res)
        {
            lbResult.Text = "Result : " + Enum.GetName(typeof(DialogResult), res) + Environment.NewLine +
                            "RadioButtonIndex : " + PSTaskDialog.cTaskDialog.RadioButtonResult.ToString() + Environment.NewLine +
                            "CommandButtonIndex : " + PSTaskDialog.cTaskDialog.CommandButtonResult.ToString() + Environment.NewLine +
                            "Verify CheckBox : " + (PSTaskDialog.cTaskDialog.VerificationChecked ? "true" : "false");
        }
    }
}
