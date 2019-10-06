
using System;
using System.Drawing;

namespace SqlPrep
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
	

	}
	
	/// <summary>
	/// Custom event handler for getting data from a Converter object back out to the main form. --Will Kraft (10/5/19).
	/// </summary>
	/// <param name="sender">Sender object</param>
	/// <param name="e">Arguments containing Converter-related data</param>
	public delegate void ConvertEventHandler(object sender, ConverterEventArgs e);
}
