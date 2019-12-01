using SqlPrep.Delegates;
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
