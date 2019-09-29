﻿/*
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
using System.Reflection;
using System.Diagnostics;

namespace SqlPrep
{
	/// <summary>
	/// Description of About.
	/// </summary>
	public partial class About : Form
	{
		public About()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			var _assembly=Assembly.GetExecutingAssembly();

			label1.Text="SqlPrep v." + FileVersionInfo.GetVersionInfo(_assembly.Location).FileVersion;
			button1.Focus();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
