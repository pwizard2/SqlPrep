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
using System.Text.RegularExpressions;

namespace SqlPrep
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		/// <summary>
		/// Gets whether this program is running in devmode. --Will Kraft (9/22/19).
		/// </summary>
		bool DevMode {
			get {
				#if DEBUG
				return true;				
				#else
				return false;				
				#endif
			}
			
		}
		
		/// <summary>
		/// Gets or sets the text from the upper pane. --Will Kraft (9/22/19).
		/// </summary>
		string Input {
			get {
				return txtInput.Text;
			}
			set {
				txtInput.Text = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the text from the lower pane. --Will Kraft (9/22/19).
		/// </summary>
		string Output {
			get {
				return txtOutput.Text;
			}
			set {
				txtOutput.Text = value;
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
		void Prepare()
		{
			
			try {
				
				var _varName = string.Empty;
				
				if (string.IsNullOrEmpty(Input))
					throw new Exception("No text to process.");
				   
				var sb = new StringBuilder();
				
				var _varDlg = new dlgVariable();
				_varDlg.ShowDialog();
				
				_varName = string.IsNullOrEmpty(_varDlg.VariableName) ? "_query" : _varDlg.VariableName;
				var _extraPad = _varDlg.LeftPadding;
				
				var lineCount = Input.Length - Input.Replace(Environment.NewLine, string.Empty).Length;
	
				using (var tr = new StringReader(Input)) {										

					var _data = string.Empty;
					
					var _currentLineNum = 0;


					do {
						
						_data = tr.ReadLine();
						
						if (!string.IsNullOrEmpty(_data)) {
							_data = _data.TrimEnd();
						} else {
							continue;
						}
							
						
						if (_currentLineNum == 0) {
							var _nextLine = new StringBuilder();
							
							_nextLine.Append("var ");						
							_nextLine.Append(_varName);						                 
							_nextLine.Append(" = \" ");
							_nextLine.Append(_data);
							_nextLine.Append("\"");
							
							sb.AppendLine(_nextLine.ToString());
						} else {
							
							var _nextLine = new StringBuilder();
							
							for (int i = 0; i < _varName.Length + 5 + _extraPad; i++)
								_nextLine.Append(" ");
							    
							_nextLine.Append("+ \" ");
							
							_nextLine.Append(_data);
							_nextLine.Append("\"");
							
							sb.AppendLine(_nextLine.ToString());
						}
						
						_currentLineNum++;
					} while(_data != null);
	                   

					var _lastLine = new StringBuilder();
							
					for (int i = 0; i < _varName.Length + 5 + _extraPad; i++)
						_lastLine.Append(" ");
							
					_lastLine.Append(";");
							
					sb.Append(_lastLine.ToString());
					
					Output = sb.ToString();
				}
			} catch (Exception ex) {
				
				MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		/// <summary>
		/// Remove the C# line formatting. --Will Kraft (9/21/19).
		/// </summary>
		void Strip()
		{			
			try {
				
				if (string.IsNullOrEmpty(Input))
					throw new Exception("No text to process.");
				
				var lineCount = Input.Length - Input.Replace(Environment.NewLine, string.Empty).Length;
				var sb = new StringBuilder();
				
				using (var tr = new StringReader(Input)) {										

					var _data = string.Empty;
					
					var _currentLineNum = 0;
					
					var lead = 0;
					
					do {
						
						
						_data = tr.ReadLine();
						
						if (_currentLineNum == 0) {
							lead = _data.IndexOf("=", StringComparison.InvariantCulture);
							
				
							if (!Regex.IsMatch(_data, @"^(var|string)\s+.+=\s\"""))
								throw new Exception("Invalid input string detected. This method only accepts valid C# string variable statements beginning with \"var\" or \"string\".");
						}

						if (!string.IsNullOrEmpty(_data)) {
							
							if (_data.Contains("+ \"")) {

								var _cleaned = _data.Substring(_data.IndexOf("+" , StringComparison.InvariantCulture)+3);
								//_cleaned = _cleaned.Replace("\"", string.Empty);
								_cleaned = _cleaned.Substring(0, _cleaned.Length - 1);
								
								if (!Regex.IsMatch(_cleaned, @"\s+\;"))
									sb.AppendLine(_cleaned);
								
							} 
							
							else if (_data.Contains("= \"")) {

								var _cleaned = _data.Substring(_data.IndexOf("=" , StringComparison.InvariantCulture)+3);
								//_cleaned = _cleaned.Replace("\"", string.Empty);
								_cleaned = _cleaned.Substring(0, _cleaned.Length - 1);
								
								if (!Regex.IsMatch(_cleaned, @"\s+\;"))
									sb.AppendLine(_cleaned);
								
							}
							else {
								
								if (!Regex.IsMatch(_data, @"\s+\;"))
									sb.AppendLine(_data);
							}
							
						}
						
							
						//sb.AppendLine(_cleaned);
						_currentLineNum++;
					} while(_data != null);
					
					Output = sb.ToString();
				
				}
				
			} catch (Exception ex) {
				
				MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		void ClearToolStripMenuItemClick(object sender, EventArgs e)
		{
			Input = string.Empty;
			Output = string.Empty;
			txtInput.Focus();
		}
		
		void StripToolStripMenuItemClick(object sender, EventArgs e)
		{
			Strip();
		}
		
		void UpperToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				Clipboard.SetText(Input);
			} catch {
				MessageBox.Show("No text to copy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		void LowerToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				Clipboard.SetText(Output);
			} catch {
				MessageBox.Show("No text to copy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			
		}
		
		void PasteToolStripMenuItemClick(object sender, EventArgs e)
		{
			Input = Clipboard.GetText();
		}
		
	}
}
