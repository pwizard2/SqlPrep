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

using System.Windows;

namespace SqlPrep
{
    /// <summary>
    /// Import dialog window. --Will Kraft (2/29/2020).
    /// </summary>
    public partial class ImportDialog : Window
    {
        public ImportDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets whether the append behavior should be used for the tab set that is to be 
        /// loaded. If false, the replace behavior is used instead. --Will Kraft (3/1/2020).
        /// </summary>
        public bool Append { get; set; }

        private void BtnAppend_Click(object sender, RoutedEventArgs e)
        {
            Append = true;
            Close();
        }

        private void BtnReplace_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
