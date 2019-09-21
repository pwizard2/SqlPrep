/*
 * Created by SharpDevelop.
 * User: Will Kraft
 * Date: 9/21/2019
 * Time: 3:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Linq;

namespace SqlPrep
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		string Input
		{
			get{
				return txtInput.Text;
			}
			set{
				txtInput.Text=value;
			}
		}
		
		string Output
		{
			get{
				return txtOutput.Text;
			}
			set{
				txtOutput.Text=value;
			}
		}
		
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void MainFormLoad(object sender, EventArgs e)
		{
	
		}
		void TextBox2TextChanged(object sender, EventArgs e)
		{
	
		}
		void StripParenthesisToolStripMenuItemClick(object sender, EventArgs e)
		{
	
		}
		void QuitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}
		
		void PrepareToolStripMenuItemClick(object sender, EventArgs e)
		{
			Prepare();
		}
		
		/// <summary>
		/// Add c# line Formatting.
		/// </summary>
		void Prepare(){
			
			try{
				
				var _varName=string.Empty;
				
				if(string.IsNullOrEmpty(Input))
					throw new Exception("Nothing to convert");
				   
				
				
				var sb=new StringBuilder();
				
				var _varDlg=new dlgVariable();
				_varDlg.ShowDialog();
				
				_varName=string.IsNullOrEmpty(_varDlg.VariableName) ? "_query" : _varDlg.VariableName;
				
				var lineCount=Input.Length - Input.Replace(Environment.NewLine, string.Empty).Length;
	
				using(var tr=new StringReader(Input))
				{										

					var _data=string.Empty;
					
					var _currentLineNum=0;


					do{
						
						_data=tr.ReadLine();
						
						if(!string.IsNullOrEmpty(_data)){
							_data=_data.TrimEnd();
						}
						else{
							continue;
						}
							
						
						if(_currentLineNum==0){
							var _nextLine=new StringBuilder();
							
							_nextLine.Append("var ");						
							_nextLine.Append(_varName);						                 
							_nextLine.Append(" = \" ");
							_nextLine.Append(_data);
							_nextLine.Append("\"");
							
							sb.AppendLine(_nextLine.ToString());
						}						
						else{
							
							var _nextLine=new StringBuilder();
							
							for(int i=0; i < _varName.Length + 5; i++)
							    _nextLine.Append(" ");
							    
							_nextLine.Append("+ \" ");
							
							_nextLine.Append(_data);
							_nextLine.Append("\"");
							
							sb.AppendLine(_nextLine.ToString());
						}
						
						_currentLineNum++;
					}
					while(_data != null);
	                   

						var _lastLine=new StringBuilder();
							
							for(int i=0; i < _varName.Length + 5; i++)
							    _lastLine.Append(" ");
							
							_lastLine.Append(";");
							
							sb.Append(_lastLine.ToString());
					
					Output=sb.ToString();
				}
			}
			catch(Exception ex){
				
				MessageBox.Show(ex.ToString());
			}
		}
		
		/// <summary>
		/// Remove the C# line formatting. --Will Kraft (9/21/19).
		/// </summary>
		void Strip(){
			try{
				
			}
			catch(Exception ex){
				
			}
		}
	}
}
