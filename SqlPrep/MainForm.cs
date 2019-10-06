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
using System.Linq;
using System.Collections.Generic;

namespace SqlPrep
{
	/// <summary>
	/// Main GUI.
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Gets or sets the number of pages in the history.
		/// </summary>
		int PageNumber{ get; set; }
		
		/// <summary>
		/// Gets or sets the converter object on the active tab. --Will Kraft (10/6/19).
		/// </summary>
		Converter Current{ get; set; }
		
		/// <summary>
		/// Gets or sets the tab name history for this session. --Will Kraft (10/6/19).
		/// </summary>
		List<string> History{ get; set; }
		
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
		/// Spawn a tab with a converter object. --Will Kraft (10/6/19).
		/// </summary>
		void AddTab()
		{
			
			var _con = new Converter();
			_con.Dock = DockStyle.Fill;
			_con.Name = "_con";
			_con.PrepareDone += _con_PrepareDone;
			_con.StripDone += _con_StripDone;	
			
			var _newPage = new TabPage();
			_newPage.Text = "Query " + (PageNumber + 1);
			_newPage.Controls.Add(_con);

			tabControl1.TabPages.Add(_newPage);
			PageNumber++;
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
			
			AddTab();
			Current = (Converter)tabControl1.TabPages[0].Controls["_con"];
			History = new List<string>();
				
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
	
		}
		
		void TextBox2TextChanged(object sender, EventArgs e)
		{
	
		}
		

		void QuitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}
		
		void ClearToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				Current.Input = string.Empty;
				Current.Output = string.Empty;

			} catch (Exception ex) {
				MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			
			
		}
		
		void StripToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				Current.Strip();
			} catch (Exception ex) {
				MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		

		void AddToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				AddTab();
				tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
				
			} catch (Exception ex) {
				MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		void TabControl1SelectedIndexChanged(object sender, EventArgs e)
		{
			try {
				Current = (Converter)tabControl1.SelectedTab.Controls["_con"];
				
				
			} catch (Exception ex) {
				MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		
		void PrepareToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				Current.Prepare();
			} catch (Exception ex) {
				MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		void PasteToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				Current.Input = Clipboard.GetText();
			} catch (Exception ex) {
				MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		void LowerToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				Clipboard.SetText(Current.Output);
			} catch {
				MessageBox.Show("No text to copy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			
		}
		
		void AboutToolStripMenuItemClick(object sender, EventArgs e)
		{
			var _about = new About();
			_about.ShowDialog();
		}
		
		void UpperToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				Clipboard.SetText(Current.Input);
			} catch {
				MessageBox.Show("No text to copy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			
		}
		
		void _con_PrepareDone(object sender, ConverterEventArgs e)
		{
			History.Add(e.TabName);
			int _count = History.Count(s => s == e.TabName);
			tabControl1.SelectedTab.Text = _count > 1 ? e.TabName + " (" + _count + ")" : e.TabName;
			
		}

		void _con_StripDone(object sender, ConverterEventArgs e)
		{
			History.Add(e.TabName);
			int _count = History.Count(s => s == e.TabName);
			tabControl1.SelectedTab.Text = _count > 1 ? e.TabName + " (" + _count + ")" : e.TabName;
			
		}
		
		void CloseCurrentToolStripMenuItemClick(object sender, EventArgs e)
		{
			try {
				
				if (tabControl1.TabPages.Count == 1) {
				
					PageNumber = 0;
					History.Clear();
					AddTab();
				}
		
				tabControl1.TabPages.Remove(tabControl1.SelectedTab);
				tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
				
			} catch {
				MessageBox.Show("No text to copy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		void CloseAllToolStripMenuItemClick(object sender, EventArgs e)
		{
		}
	}
}
