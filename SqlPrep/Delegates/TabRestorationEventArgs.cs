/*
   This file is part of SqlPrep.
   Copyright © 2020, Will Kraft <pwizard@gmail.com>

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

namespace SqlPrep.Delegates
{
    /// <summary>
    /// Event data for tab restoration. --Will Kraft (12/29/2019).
    /// </summary>
    public class TabRestorationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the upper query for the editor. --Will Kraft (12/29/2019).
        /// </summary>
        public string UpperText { get; set; }
        
        /// <summary>
        /// Gets or sets the lower query for the editor. --Will Kraft (12/29/019).
        /// </summary>
        public string LowerText { get; set; }

        /// <summary>
        /// Gets or sets the name for this tab. --Will Kraft (12/29/2019).
        /// </summary>
        public string TabName { get; set; }

        /// <summary>
        /// Gets or sets the original GUID for this tab.
        /// </summary>
        public Guid TabID { get; set; }

        /// <summary>
        /// Gets or sets what kind of task this is. --Will Kraft (12/29/2019).
        /// </summary>
        public TaskType Task { get; set; }

        /// <summary>
        /// Simulated preparation args for this restored tab. --Will Kraft (12/29/2019).
        /// </summary>
        public PrepareEventArgs PrepareArgs { get; set; }

    }

    /// <summary>
    /// Event that triggers a tab regen from saved XML data. --Will Kraft (12/29/2019).
    /// </summary>
    /// <param name="sender">Sender class</param>
    /// <param name="e">Container for XML data.</param>
    public delegate void RestoreTabEventHandler(object sender, TabRestorationEventArgs e);
}
