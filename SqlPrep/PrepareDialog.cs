/*
   This file is part of SqlPrep.

    Foobar is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    SqlPrep is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with SqlPrep.  If not, see <https://www.gnu.org/licenses/>.
 */
 
using System;
using System.Windows.Forms;

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
