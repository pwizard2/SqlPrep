/*
 * Created by SharpDevelop.
 * User: Will Kraft
 * Date: 9/21/2019
 * Time: 4:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SqlPrep
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class dlgVariable : Form
	{
		/// <summary>
		/// This is the variable name. --Will Kraft (9/23/19).
		/// </summary>
		internal string VariableName{ get; set; }
		
		/// <summary>
		/// Gets or sets the amount of whitespace to pad the prepared string with. --Will Kraft (9/28/19).
		/// </summary>
		internal int LeftPadding { get; set; }
		
		/// <summary>
		/// Gets or sets if the prepare operation should use "var" instead of "string". --Will Kraft (9/28/19).
		/// </summary>
		internal bool UseVar{get;set;}
		

		
		public dlgVariable()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
		
			var _pad = textBox2.Text ?? string.Empty;
			VariableName = textBox1.Text;
			LeftPadding = string.IsNullOrEmpty(_pad) ? 0 : _pad.Length;
			UseVar=chkUseVar.Checked;
			DialogResult=DialogResult.OK;
			Close();
		}
		void Label2Click(object sender, EventArgs e)
		{
	
		}
		void BtnCancelClick(object sender, EventArgs e)
		{
			DialogResult=DialogResult.Cancel;
			Close();
		}
		
		
	}
}
