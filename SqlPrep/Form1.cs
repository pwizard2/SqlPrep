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

namespace SqlPrep
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class dlgVariable : Form
	{
		internal string VariableName{get;set;}
		
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
		
			VariableName=textBox1.Text;
			Close();
		}
	}
}
