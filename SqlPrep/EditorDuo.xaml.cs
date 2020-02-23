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

using SqlPrep.Delegates;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SqlPrep
{
    /// <summary>
    /// Interaction logic for EditorDuo.xaml
    /// </summary>
    public partial class EditorDuo : UserControl
    {
        public string UpperText
        {
            get
            {
                return Upper.Text;
            }
            set
            {
                Upper.Text = value;
            }
        }

        public Brush LowerColor
        {
            get
            {
                return Lower.EditorBackground;
            }
            set
            {
                Lower.EditorBackground = value;
            }
        }

        public Brush LowerSelect
        {
            get
            {
                return Lower.EditorSelectColor;
            }
            set
            {
                Lower.EditorSelectColor = value;
            }
        }


        public Guid ID { get; set; }

        public string LowerText
        {
            get
            {
                return Lower.Text;
            }
            set
            {
                Lower.Text = value;
            }
        }

        event EventHandler TabOpened;

        /// <summary>
        /// Gets or sets whether this tab has had a task run on it yet. --Will Kraft (12/24/2019).
        /// </summary>
        public bool Processed { get; set; }

        public EditorDuo()
        {
            InitializeComponent();
            Upper.Processed = false;
            Lower.Processed = false;

            Upper.CreationDate = default;
            Lower.CreationDate = default;


            Upper.ProcessedDate = default;
            Lower.ProcessedDate = default;

            Upper.SetDateStamp();
            Lower.SetDateStamp();

            TabOpened?.Invoke(this, EventArgs.Empty);
            InputFocus();
        }

        /// <summary>
        /// Manually set the focus to the input pane. --Will Kraft (11/3/19).
        /// </summary>
        public void InputFocus()
        {
            Upper.InputFocus();
        }

        /// <summary>
        /// Provide a place to save the most recent args in case we want to run the process 
        /// again on this tab, for instance, if we change the sql. --Will Kraft (11/30/2019).
        /// </summary>
        internal PrepareEventArgs Args { get; set; }

        /// <summary>
        /// Gets or sets the type of task this tab has completed. --Will Kraft (12/24/2019).
        /// </summary>
        public TaskType Task { get; set; }

        /// <summary>
        /// Store the tab name for the XML file. --Will Kraft (12/24/2019).
        /// </summary>
        public string TabName { get; set; }

        private void txtUpper_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = true;
        }

        private void txtLower_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // e.Handled = true;
        }
    }
}
