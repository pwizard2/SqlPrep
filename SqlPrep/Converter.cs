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
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SqlPrep
{
	/// <summary>
	/// Description of Converter.
	/// </summary>
	public partial class Converter : UserControl
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
		/// Event that runs after a Prepare operation has successfully completed. --Will Kraft (10/5/19).
		/// </summary>
		public event ConvertEventHandler PrepareDone;
		
		/// <summary>
		/// Event that runs after a Strip operation has successfully completed. --Will Kraft (10/5/19).
		/// </summary>
		public event ConvertEventHandler StripDone;
		
		public Converter()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
					
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		/// <summary>
		/// Gets or sets the text from the upper pane. --Will Kraft (9/22/19).
		/// </summary>
		internal string Input {
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
		internal string Output {
			get {
				return txtOutput.Text;
			}
			set {
				txtOutput.Text = value;
			}
		}
		
		/// <summary>
		/// Add c# line Formatting.
		/// </summary>
		internal void Prepare()
		{
			
			try {
				
				var _varName = string.Empty;
				
				if (string.IsNullOrEmpty(Input))
					throw new Exception("No text to process.");
				   
				var sb = new StringBuilder();
				
				var _varDlg = new dlgVariable();
				var _result = _varDlg.ShowDialog();
				
				if (_result == DialogResult.Cancel)
					return;
				
				_varName = string.IsNullOrEmpty(_varDlg.VariableName) ? "_query" : _varDlg.VariableName;
				var _extraPad = _varDlg.LeftPadding;
				var _varType = _varDlg.UseVar ? "var" : "string";
				
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
							
							_nextLine.Append(_varType);
							_nextLine.Append(" ");						
							_nextLine.Append(_varName);						                 
							_nextLine.Append(" = \" ");
							_nextLine.Append(_data);
							_nextLine.Append("\"");
							
							sb.AppendLine(_nextLine.ToString());
						} else {
							
							var _nextLine = new StringBuilder();
							
							for (int i = 0; i < _varName.Length + (_varType.Length + 2) + _extraPad; i++)
								_nextLine.Append(" ");
							    
							_nextLine.Append("+ \" ");
							
							_nextLine.Append(_data);
							_nextLine.Append("\"");
							
							sb.AppendLine(_nextLine.ToString());
						}
						
						_currentLineNum++;
					} while(_data != null);
	                   

					var _lastLine = new StringBuilder();
							
					for (int i = 0; i < _varName.Length + (_varType.Length + 2) + _extraPad; i++)
						_lastLine.Append(" ");
							
					_lastLine.Append(";");
							
					sb.Append(_lastLine.ToString());
					
					Output = sb.ToString();
					
					if (PrepareDone != null)
						PrepareDone(this, new ConverterEventArgs {
							TabName = _varName
						});
				}
			} catch (Exception ex) {
				
				MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		/// <summary>
		/// Remove the C# line formatting. --Will Kraft (9/21/19).
		/// </summary>
		internal void Strip()
		{			
			try {
				
				if (string.IsNullOrEmpty(Input))
					throw new Exception("No text to process.");
				
				var lineCount = Input.Length - Input.Replace(Environment.NewLine, string.Empty).Length;
				var sb = new StringBuilder();
				
				using (var tr = new StringReader(Input)) {										

					var _data = string.Empty;
					var _label = string.Empty;
					
					var _currentLineNum = 0;
					
					var lead = 0;
					
					do {

						_data = tr.ReadLine();
						
						if (_currentLineNum == 0) {
							
							if (!Regex.IsMatch(_data.TrimStart(), @"^(var|string)\s+.+=\s\"""))
								throw new Exception("Invalid input string detected. This method only accepts valid C# string variable statements beginning with \"var\" or \"string\".");
							
							lead = _data.IndexOf("=", StringComparison.InvariantCulture);
							
							_label = _data.Substring(0, lead).Replace("string", string.Empty).Replace("var", string.Empty).Trim();
							
							
						}

						if (!string.IsNullOrEmpty(_data)) {
							
							if (_data.Contains("+ \"")) {

								var _cleaned = _data.Substring(_data.IndexOf("+", StringComparison.InvariantCulture) + 3);
								//_cleaned = _cleaned.Substring(0, _cleaned.Length - 1);
								
								_cleaned = _cleaned.Contains("\";") ? _cleaned.Substring(0, _cleaned.Length - 2) 
									: _cleaned.Substring(0, _cleaned.Length - 1);
								
								if (!Regex.IsMatch(_cleaned, @"\s+\;") && !string.IsNullOrEmpty(_cleaned))
									sb.AppendLine(_cleaned);	
							} else if (_data.Contains("= \"")) {

								var _cleaned = _data.Substring(_data.IndexOf("=", StringComparison.InvariantCulture) + 3);

								_cleaned = _cleaned.Substring(0, _cleaned.Length - 1);

								if (!Regex.IsMatch(_cleaned, @"\s+\;"))
									sb.AppendLine(_cleaned);								
							} else {
								
								if (!Regex.IsMatch(_data, @"\s+\;"))
									sb.AppendLine(_data);
							}							
						}
	
						_currentLineNum++;
						
					} while(_data != null);
					
					Output = sb.ToString();
				
					if (StripDone != null)
						StripDone(this, new ConverterEventArgs {
							TabName = _label
						});
				}
				
			} catch (Exception ex) {
				
				MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
