using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            
            SetRowsandCols();
        }

        private void txtEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        void SetRowsandCols()
        {
            //var r = txtEditor.GetLineIndexFromCharacterIndex(txtEditor.CaretIndex);
            //txtRow.Text = $"Row: {(r < 0 ? 0 : r + 1)}";

            //txtCol.Text = $"Col: {(txtEditor.GetCharacterIndexFromLineIndex(r) < 0 ? 0 : txtEditor.GetCharacterIndexFromLineIndex(r) + 1)}";

            var r = txtEditor.GetLineIndexFromCharacterIndex(txtEditor.CaretIndex);
            txtRow.Text = $"Row: {(r < 0 ? 0 : r + 1)}";

            txtCol.Text = $"Col: {(txtEditor.CaretIndex < 0 ? 0 : txtEditor.CaretIndex)}";
        }

        private void txtEditor_KeyDown(object sender, KeyEventArgs e)
        {
            SetRowsandCols();
        }
    }
}
