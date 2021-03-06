﻿/*
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SqlPrep
{
    /// <summary>
    /// Contains a single editor for an upper or lower position. 
    /// </summary>
    public partial class EditorSingle : UserControl
    {
        public EditorSingle()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Manually set the focus to the input pane. --Will Kraft (11/3/19).
        /// </summary>
        public void InputFocus()
        {
            txtEditor.Focus();
        }

        /// <summary>
        /// Gets or sets the position for this object in the current EditorDuo. --Will Kraft (2/22/2020).
        /// </summary>
        public EditorSinglePosition Position { get; set; }

        /// <summary>
        /// Gets or sets the type of task this tab has completed. --Will Kraft (12/24/2019).
        /// </summary>
        public TaskType Task { get; set; }

        public string Text
        {
            get
            {
                return txtEditor.Text;
            }
            set
            {
                txtEditor.Text = value;
            }
        }


        /// <summary>
        /// Gets or sets the background color for the editor. --Will Kraft (2/22/2020).
        /// </summary>
        public Brush EditorBackground
        {
            get
            {
                return txtEditor.Background;
            }
            set
            {
                txtEditor.Background = value;
            }
        }

        /// <summary>
        /// Gets or sets the text selection color for the editor. --Will Kraft (2/22/2020).
        /// </summary>
        public Brush EditorSelectColor
        {
            get
            {
                return txtEditor.SelectionBrush;
            }
            set
            {
                txtEditor.SelectionBrush = value;
            }
        }

        /// <summary>
        /// Gets or sets the text selection color for the operation type. --Will Kraft (2/22/2020).
        /// </summary>
        public Brush OpStatusProcessedColor
        {
            get
            {
                return OperationType.Foreground;
            }
            set
            {
                OperationType.Foreground = value;
            }
        }

        /// <summary>
        /// Gets or sets whether this editor has been processed yet.
        /// </summary>
        public bool Processed { get; set; }

        public void SetTaskType()
        {

            switch (Task)
            {
                default:
                    OperationType.Text = "None";
                    break;


                case TaskType.Prepare:
                    OperationType.Text = "Prepare";
                    break;

                case TaskType.Strip:
                    OperationType.Text = "Strip";
                    break;
            }

        }


        /// <summary>
        /// Set the date stamp for this editor's status bar. --Will Kraft (2/22/2020).
        /// </summary>
        public void SetDateStamp()
        {
            if (!Processed && Position == EditorSinglePosition.Lower)
            {
                DateStamp.Visibility = Visibility.Collapsed;
                Sep2.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                DateStamp.Visibility = Visibility.Visible;
                Sep2.Visibility = Visibility.Visible;

                switch (Position)
                {
                    default:
                    case EditorSinglePosition.Upper:
                        DateStamp.Text = CreationDate == default || CreationDate == DateTime.MinValue ? "Created: Unknown" : $"Created: {((DateTime)CreationDate).ToString()}";
                        break;

                    case EditorSinglePosition.Lower:
                        DateStamp.Text = ProcessedDate == default || ProcessedDate == DateTime.MinValue ? "Last Processed: Unknown" : $"Last Processed: {((DateTime)ProcessedDate).ToString()}";
                        break;
                }
            }
        }


        /// <summary>
        /// Gets or sets the time this editor was created. Only upper editors should use 
        /// this, set default otherwise. --Will Kraft (2/22/2020).
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the time this editor was last processed. Only lower editors should 
        /// use this, set default otherwise. --Will Kraft (2/22/2020).
        /// </summary>
        public DateTime? ProcessedDate { get; set; }

        private void TxtEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextEntered();
        }

        private void TxtEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextEntered();
        }

        void TextEntered()
        {

            var r = txtEditor.GetLineIndexFromCharacterIndex(txtEditor.CaretIndex);
            txtRow.Text = $"Row: {(r < 0 ? 0 : r + 1)}";
            txtCol.Text = $"Col: {txtEditor.CaretIndex - txtEditor.GetCharacterIndexFromLineIndex(r) + 1}";
        }

        private void txtEditor_KeyDown(object sender, KeyEventArgs e)
        {
            TextEntered();
        }
    }
}
