﻿using SqlPrep.Worker;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SqlPrep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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



            AddTab();

            CurrentEditor.InputFocus();


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
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        void AddTab()
        {

            var n = new EditorDuo
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Name = $"Editor",
                ID = Guid.NewGuid()
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

        void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

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

                var _recycleArgs = false;

                if (CurrentEditor.Args != null)
                    _recycleArgs = MessageBox.Show("Do you want to re-apply the most recent settings to this query?", "SqlPrep", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes;

                if (_recycleArgs && CurrentEditor.Args != null)
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
        }

        void StripClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine($"ID: {CurrentEditor.ID}");

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
    }
}