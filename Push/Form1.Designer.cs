namespace Push
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.FName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView2 = new System.Windows.Forms.ListView();
            this.FNameTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TypeTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SizeTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DateTarget = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.duplicateGroup = new System.Windows.Forms.GroupBox();
            this.copyRename = new System.Windows.Forms.RadioButton();
            this.overWrite = new System.Windows.Forms.RadioButton();
            this.skip = new System.Windows.Forms.RadioButton();
            this.TestOnly = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.edWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbResult = new System.Windows.Forms.Label();
            this.duplicateGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Push";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(395, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(220, 82);
            this.listBox1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FName,
            this.Type,
            this.FSize,
            this.Date});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(12, 100);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(365, 317);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // FName
            // 
            this.FName.Text = "Name";
            this.FName.Width = 73;
            // 
            // Type
            // 
            this.Type.Text = "Type";
            this.Type.Width = 74;
            // 
            // FSize
            // 
            this.FSize.Text = "Size";
            // 
            // Date
            // 
            this.Date.Text = "Date";
            this.Date.Width = 118;
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FNameTarget,
            this.TypeTarget,
            this.SizeTarget,
            this.DateTarget});
            this.listView2.Location = new System.Drawing.Point(395, 100);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(356, 318);
            this.listView2.TabIndex = 6;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // FNameTarget
            // 
            this.FNameTarget.Text = "Name";
            this.FNameTarget.Width = 73;
            // 
            // TypeTarget
            // 
            this.TypeTarget.Text = "Type";
            this.TypeTarget.Width = 74;
            // 
            // SizeTarget
            // 
            this.SizeTarget.Text = "Size";
            // 
            // DateTarget
            // 
            this.DateTarget.Text = "Date";
            this.DateTarget.Width = 118;
            // 
            // duplicateGroup
            // 
            this.duplicateGroup.Controls.Add(this.copyRename);
            this.duplicateGroup.Controls.Add(this.overWrite);
            this.duplicateGroup.Controls.Add(this.skip);
            this.duplicateGroup.Location = new System.Drawing.Point(622, 13);
            this.duplicateGroup.Name = "duplicateGroup";
            this.duplicateGroup.Size = new System.Drawing.Size(129, 81);
            this.duplicateGroup.TabIndex = 7;
            this.duplicateGroup.TabStop = false;
            this.duplicateGroup.Text = "Duplicates";
            // 
            // copyRename
            // 
            this.copyRename.AutoSize = true;
            this.copyRename.Location = new System.Drawing.Point(10, 58);
            this.copyRename.Name = "copyRename";
            this.copyRename.Size = new System.Drawing.Size(94, 17);
            this.copyRename.TabIndex = 2;
            this.copyRename.Text = "Copy/Rename";
            this.copyRename.UseVisualStyleBackColor = true;
            // 
            // overWrite
            // 
            this.overWrite.AutoSize = true;
            this.overWrite.Checked = true;
            this.overWrite.Location = new System.Drawing.Point(10, 37);
            this.overWrite.Name = "overWrite";
            this.overWrite.Size = new System.Drawing.Size(70, 17);
            this.overWrite.TabIndex = 1;
            this.overWrite.TabStop = true;
            this.overWrite.Text = "Overwrite";
            this.overWrite.UseVisualStyleBackColor = true;
            // 
            // skip
            // 
            this.skip.AutoSize = true;
            this.skip.Location = new System.Drawing.Point(10, 16);
            this.skip.Name = "skip";
            this.skip.Size = new System.Drawing.Size(46, 17);
            this.skip.TabIndex = 0;
            this.skip.Text = "Skip";
            this.skip.UseVisualStyleBackColor = true;
            // 
            // TestOnly
            // 
            this.TestOnly.Location = new System.Drawing.Point(12, 42);
            this.TestOnly.Name = "TestOnly";
            this.TestOnly.Size = new System.Drawing.Size(133, 23);
            this.TestOnly.TabIndex = 8;
            this.TestOnly.Text = "Test-Only";
            this.TestOnly.UseVisualStyleBackColor = true;
            this.TestOnly.Click += new System.EventHandler(this.TestOnly_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(279, 71);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(102, 17);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Force Emulation";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // edWidth
            // 
            this.edWidth.Location = new System.Drawing.Point(279, 42);
            this.edWidth.Name = "edWidth";
            this.edWidth.Size = new System.Drawing.Size(100, 20);
            this.edWidth.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Emulation Form Width";
            // 
            // lbResult
            // 
            this.lbResult.Location = new System.Drawing.Point(151, 13);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(122, 75);
            this.lbResult.TabIndex = 13;
            this.lbResult.Text = "label2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 430);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edWidth);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.TestOnly);
            this.Controls.Add(this.duplicateGroup);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.duplicateGroup.ResumeLayout(false);
            this.duplicateGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader FName;
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader FSize;
        private System.Windows.Forms.ColumnHeader FNameTarget;
        private System.Windows.Forms.ColumnHeader TypeTarget;
        private System.Windows.Forms.ColumnHeader SizeTarget;
        private System.Windows.Forms.ColumnHeader DateTarget;
        private System.Windows.Forms.GroupBox duplicateGroup;
        private System.Windows.Forms.RadioButton copyRename;
        private System.Windows.Forms.RadioButton overWrite;
        private System.Windows.Forms.RadioButton skip;
        private System.Windows.Forms.Button TestOnly;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox edWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbResult;
    }
}

