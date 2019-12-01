
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
using System.Text.RegularExpressions;
using System.Windows;

namespace SqlPrep
{
    /// <summary>
    /// Interaction logic for Prepare.xaml
    /// </summary>
    public partial class Prepare : Window
    {

        public event PrepareEventHandler Prepared;

        public Prepare()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtVariable.Text))
                    throw new Exception("You must set a variable name.");

                if (txtVariable.Text.Contains(@"\s+"))
                    throw new Exception("Variable names cannot contain whitespace.");

                if (Regex.IsMatch(txtVariable.Text, "var", RegexOptions.IgnoreCase) || Regex.IsMatch(txtVariable.Text, "string", RegexOptions.IgnoreCase) || Regex.IsMatch(txtVariable.Text, "stringbuilder", RegexOptions.IgnoreCase))
                    throw new Exception("Invalid variable name.");

                Prepared?.Invoke(this, new PrepareEventArgs
                {
                    VariableName = txtVariable.Text,
                    UseVar = chkImplicit.IsChecked != default ? (bool)chkImplicit.IsChecked : true,
                    UseAppendLine= chkAppendLine.IsChecked != default ? (bool)chkAppendLine.IsChecked : false,
                    LeftPadding = txtPadding.Text.Length,
                    Cancelled = false,
                    Object = (bool)rString.IsChecked ? OutputType.String : OutputType.StringBuilder
                });

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Prepared?.Invoke(this, new PrepareEventArgs
            {
                Cancelled = true
            });

            DialogResult = false;
            Close();
        }

        private void SetAppendVis(object sender, RoutedEventArgs e)
        {
            try
            {
                chkAppendLine.Visibility = (bool)rStringBuilder.IsChecked ? Visibility.Visible : Visibility.Collapsed;

                padExplain.Visibility = (bool)rString.IsChecked ? Visibility.Visible : Visibility.Collapsed;
                padExplainImg.Visibility = (bool)rString.IsChecked ? Visibility.Visible : Visibility.Collapsed;
                txtPadding.Visibility = (bool)rString.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                chkAppendLine.Visibility = Visibility.Collapsed;
                padExplain.Visibility = Visibility.Visible;
                padExplainImg.Visibility = Visibility.Visible;
                txtPadding.Visibility = Visibility.Visible;
            }

        }
    }
}
