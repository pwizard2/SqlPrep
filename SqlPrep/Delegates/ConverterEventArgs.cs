
using System;
using System.Drawing;
using System.Windows.Media;
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
