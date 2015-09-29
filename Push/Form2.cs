using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Push
{
	public partial class Form2 : Form
	{

		public CheckBox cb1 { get; set; }
		public CheckBox cb2 { get; set; }
		public CheckBox cb3 { get; set; }
		public TextBox SourcePath { get; set; }		
		
		
		public Form2()
		{
			InitializeComponent();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			cb1 = checkBox1;
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			cb2 = checkBox2;
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			cb3 = checkBox3;
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			cb1 = checkBox1;
			cb2 = checkBox2;
			cb3 = checkBox3; 
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



	}
}
