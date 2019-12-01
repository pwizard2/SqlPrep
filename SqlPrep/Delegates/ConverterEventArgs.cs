/*
   This file is part of SqlPrep.
   Copyright © 2019, Will Kraft <pwizard@gmail.com>

    SqlPrep is free software: you can redistribute it and/or modify
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
using Brush = System.Windows.Media.Brush;

namespace SqlPrep.Delegates
{
    /// <summary>
    /// Description of ConvertEventArgs.
    /// </summary>
    public class ConverterEventArgs : EventArgs
	{
		public ConverterEventArgs()
		{
		}
		
		/// <summary>
		/// Gets or sets the name for the cuirrent tabs based on conversion data. --Will Kraft (10/5/19).
		/// </summary>
		public string TabName{get;set;}
	
        /// <summary>
        /// Gets or sets if the task was successful. --Will Kraft (10/27/19).
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// Gets or sets the text color the tab should be after conversion. --Will Kraft (10/27/19).
        /// </summary>
        public Brush TabTextColor { get; set; }

        /// <summary>
        /// Gets or sets the background color for the ouput panel. --Will Kraft (11/17/19).
        /// </summary>
        public Brush OutputBG { get; set; }

        /// <summary>
        /// Gets or sets the selection background color for the ouput panel. --Will Kraft (11/17/19).
        /// </summary>
        public Brush OutputSelection { get; set; }
    }
	
	/// <summary>
	/// Custom event handler for getting data from a Converter object back out to the main form. 
    /// --Will Kraft (10/5/19).
	/// </summary>
	/// <param name="sender">Sender object</param>
	/// <param name="e">Arguments containing Converter-related data</param>
	public delegate void ConvertEventHandler(object sender, ConverterEventArgs e);
}
