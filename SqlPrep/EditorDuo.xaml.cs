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
                return txtUpper.Text;
            }
            set
            {
                txtUpper.Text = value;
            }
        }

        public Brush LowerColor
        {
            get
            {
                return txtLower.Background;
            }
            set
            {
                txtLower.Background = value;
            }
        }

        public Brush LowerSelect
        {
            get
            {
                return txtLower.SelectionBrush;
            }
            set
            {
                txtLower.SelectionBrush = value;
            }
        }


        public Guid ID { get; set; }

        public string LowerText
        {
            get
            {
                return txtLower.Text;
            }
            set
            {
                txtLower.Text = value;
            }
        }

        event EventHandler TabOpened;


        public EditorDuo()
        {
            InitializeComponent();

            TabOpened?.Invoke(this, EventArgs.Empty);
            InputFocus();
        }

        /// <summary>
        /// Manually set the focus to the input pane. --Will Kraft (11/3/19).
        /// </summary>
        public void InputFocus()
        {
            txtUpper.Focus();
        }

        /// <summary>
        /// Provide a place to save the most recent args in case we want to run the process 
        /// again on this tab, for instance, if we change the sql. --Will Kraft (11/30/2019).
        /// </summary>
        internal PrepareEventArgs Args { get; set; }

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
