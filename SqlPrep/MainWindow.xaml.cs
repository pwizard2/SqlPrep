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
using SqlPrep.Worker;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SqlPrep
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Gets or sets whether the history should be deleted when the program exits. --Will Kraft (12/29/2019).
        /// </summary>
        bool DeleteHistory { get; set; }

        bool DevMode
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        TabItem CurrentTab => (TabItem)Tabs.SelectedItem;

        EditorDuo CurrentEditor => (EditorDuo)CurrentTab.Content;

        int TabNumber { get; set; }



        public MainWindow()
        {
            InitializeComponent();

            var _newTabCmd = new RoutedCommand();
            _newTabCmd.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(_newTabCmd, NewTabClick));

            var _stripCmd = new RoutedCommand();
            _stripCmd.InputGestures.Add(new KeyGesture(Key.F3));
            CommandBindings.Add(new CommandBinding(_stripCmd, StripClick));

            var _prepCmd = new RoutedCommand();
            _prepCmd.InputGestures.Add(new KeyGesture(Key.F2));
            CommandBindings.Add(new CommandBinding(_prepCmd, PrepareClick));

            var _clCmd = new RoutedCommand();
            _clCmd.InputGestures.Add(new KeyGesture(Key.L, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(_clCmd, CopyLowerClick));

            var _cuCmd = new RoutedCommand();
            _cuCmd.InputGestures.Add(new KeyGesture(Key.U, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(_cuCmd, CopyUpperClick));

            var _closeTcmd = new RoutedCommand();
            _closeTcmd.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(_closeTcmd, CloseCurrentTabClick));

            var _pasteTcmd = new RoutedCommand();
            _pasteTcmd.InputGestures.Add(new KeyGesture(Key.V, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(_pasteTcmd, PasteClick));

            var _xml = new XmlDocumentHistory();
            _xml.RegenerateTab += _xml_RegenerateTab;

            if (_xml.HistoryTabCount > 0)
            {
                _xml.RecoverTabs();
            }
            else // Start empty session...
            {
                AddTab();
                CurrentEditor.InputFocus();
            }
        }


        private void _xml_RegenerateTab(object sender, TabRestorationEventArgs e)
        {
            RestoreTab(e);
        }

        /// <summary>
        /// Restore a saved tab from a previous session's stored XML data. This is usually triggered by 
        /// an event from the XMLDocumentHistory class during program load. --Will Kraft (12/29/2019).
        /// </summary>       
        /// <param name="e">Tab data (from XML) to restore.</param>
        void RestoreTab(TabRestorationEventArgs e)
        {
            Brush OutputBG = SystemColors.HighlightBrush;
            Brush OutputSelection = SystemColors.WindowBrush;
            Brush TabTextColor = SystemColors.ControlTextBrush;

            switch (e.Task)
            {
                default:
                case TaskType.Default: // This isn't supposed to happen but if it ever does, default system colors are good enough.
                    break;

                case TaskType.Prepare:
                    TabTextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1b80fa"));
                    OutputBG = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d9edff"));
                    OutputSelection = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#486394"));
                    break;

                case TaskType.Strip:
                    TabTextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fe5000")); // old color: #fa781b
                    OutputBG = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fff3cf"));
                    OutputSelection = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9c4c24"));
                    break;
            }


            var n = new EditorDuo
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Name = $"Editor{TabNumber}",
                ID = e.TabID,
                Processed = true,
                Task = e.Task,
                LowerText = e.LowerText,
                UpperText = e.UpperText,
                Args = e.PrepareArgs,
                LowerColor = OutputBG,
                LowerSelect = OutputSelection,
                TabName = e.TabName
            };

            var t = new TabItem
            {
                Header = new HeaderedContentControl
                {
                    Header = e.TabName,
                    Foreground = TabTextColor
                },

                Content = n
            };

            Tabs.Items.Add(t);

            Tabs.SelectedIndex = Tabs.Items.Count - 1;
            TabNumber++;
        }

        void AddTab()
        {

            var n = new EditorDuo
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Name = $"Editor{TabNumber}",
                ID = Guid.NewGuid(),
                Processed = false,
                Task = TaskType.Default
            };

            var t = new TabItem
            {
                Header = new HeaderedContentControl
                {
                    Header = $"Query {TabNumber + 1}"
                },

                Content = n
            };

            Tabs.Items.Add(t);


            Tabs.SelectedIndex = Tabs.Items.Count - 1;
            TabNumber++;
        }

        void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void NewTabClick(object sender, RoutedEventArgs e)
        {
            AddTab();
        }

        void PrepareClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentEditor.UpperText))
                    throw new Exception("There is no Sql to convert.");

                if (CurrentEditor.Task == TaskType.Strip)
                    throw new Exception("You can't prepare a query that has been stripped unless you copy-and-paste the output into a new tab.");

                var _recycleArgsPrompt = MessageBoxResult.None;

                if (CurrentEditor.Args != null)
                    _recycleArgsPrompt = MessageBox.Show("Do you want to re-apply the most recent settings to this query?", "SqlPrep", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);

                // Give the user a chance to bail without affecting the processed query. --Will Kraft (12/31/2019).
                if (_recycleArgsPrompt == MessageBoxResult.Cancel & CurrentEditor.Args != null)
                    return;

                if (_recycleArgsPrompt == MessageBoxResult.Yes && CurrentEditor.Args != null)
                {
                    // Sender is not really important here, all we want are the stored args. --Will Kraft (11/30/2019).
                    P_Prepared(this, CurrentEditor.Args);
                }
                else
                {
                    var p = new Prepare
                    {
                        Owner = this
                    };
                    p.Prepared += P_Prepared;
                    p.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void P_Prepared(object sender, Delegates.PrepareEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                    return;

                var _p = new SqlPrep.Worker.Prepare(e, CurrentEditor.UpperText);
                _p.TaskDone += OnTaskDone;
                var _output = _p.Run(out string prepError);

                if (!string.IsNullOrEmpty(prepError))
                    throw new Exception(prepError);

                CurrentEditor.LowerText = _output;
                CurrentEditor.Args = e;
            }
            catch (Exception ex)
            {
                MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnTaskDone(object sender, Delegates.ConverterEventArgs e)
        {
            if (!e.Successful)
                return;

            CurrentTab.Header = new HeaderedContentControl
            {
                Header = e.TabName,
                Foreground = e.TabTextColor
            };

            CurrentEditor.LowerColor = e.OutputBG;
            CurrentEditor.LowerSelect = e.OutputSelection;
            CurrentEditor.Processed = true;
            CurrentEditor.Task = e.Task;
            CurrentEditor.TabName = e.TabName;

            // Non-prepare tasks don't produce args but we still need to store something for that
            // in the XML so we can regen the tab on the next run. --Will Kraft (12/29/2019).
            if (e.Task != TaskType.Prepare)
                CurrentEditor.Args = new Delegates.PrepareEventArgs
                {
                    Cancelled = false,
                    UseAppendLine = false,
                    LeftPadding = 0,
                    Object = OutputType.String,
                    UseVar = true,
                    VariableName = string.Empty
                };
        }

        void StripClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentEditor.Task == TaskType.Prepare)
                    throw new Exception("You can't strip a query that has already been prepared unless you copy-and-paste the output into a new tab.");

                var _s = new Strip(CurrentEditor.UpperText);
                _s.TaskDone += OnTaskDone;
                var _output = _s.Run(out string _convertErr);

                CurrentEditor.LowerText = string.IsNullOrEmpty(_convertErr) ? _output : throw new Exception(_convertErr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void CopyUpperClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(CurrentEditor.UpperText);
            }
            catch
            {
                MessageBox.Show("No text to copy!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void CopyLowerClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(CurrentEditor.LowerText);
            }
            catch
            {
                MessageBox.Show("No text to copy!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void PasteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentEditor.UpperText = Clipboard.GetText();
            }
            catch
            {
                MessageBox.Show("No valid text to paste!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void ClearClick(object sender, RoutedEventArgs e)
        {

        }

        private void CloseCurrentTabClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Tabs.Items.Remove(CurrentTab);

                if (Tabs.Items.Count == 0)
                {
                    TabNumber = 0;
                    AddTab();
                }
                else
                {
                    Tabs.SelectedIndex = Tabs.Items.Count;
                }
            }
            catch
            {

            }

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var _a = new About()
            {
                Owner = this
            };

            _a.ShowDialog();


        }

        /// <summary>
        /// Gets the number of tabs that have been processed. --Will Kraft (12/24/2019).
        /// </summary>
        int ProcessedTabs
        {
            get
            {
                try
                {
                    int value = 0;

                    foreach (TabItem i in Tabs.Items)
                    {
                        if (((EditorDuo)i.Content).Processed)
                            ++value;
                    }

                    return value;
                }
                catch
                {
                    return 0;
                }
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (ProcessedTabs > 0)
            //    e.Cancel = MessageBox.Show("Do you really want to exit? All processed queries will be discarded when the program closes.", "SqlPrep", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.No;


        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var _h = new XmlDocumentHistory();

            if (ProcessedTabs > 0 && !DeleteHistory)
            {
                _h.SaveTabstoXml(Tabs.Items);
            }
            else
            {
                _h.DeleteHistory();
            }



        }

        private void MnuDeleteHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProcessedTabs == 0 || !XmlDocumentHistory.HistoryExists)
                    throw new Exception("There is no processed query history to delete.");


                if (MessageBox.Show("Do you really want to discard all processed queries? Clicking \"Yes\" will immediately end the current session so the history can be deleted.", "SqlPrep", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    DeleteHistory = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(DevMode ? ex.ToString() : ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
