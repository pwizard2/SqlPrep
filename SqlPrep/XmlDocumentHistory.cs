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

using SqlPrep.Delegates;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.XPath;

namespace SqlPrep
{
    /// <summary>
    /// This class controls everything to do with the saved XML history file for processed queries. --Will Kraft (12/24/2019).
    /// </summary>
    public class XmlDocumentHistory
    {
        /// <summary>
        /// Event that fires when XML data needs to be regenerated into a new tab for the current session. --Will Kraft (12/29/2019).
        /// </summary>
        public event RestoreTabEventHandler RegenerateTab;

        readonly static string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SqlPrep");

        /// <summary>
        /// Gets whether the history file exists. --Will Kraft (12/31/2019).
        /// </summary>
        public static bool HistoryExists => File.Exists(Path.Combine(configPath, "TabHistory.xml"));

        /// <summary>
        /// This class controls everything to do with the saved XML history file for processed queries. --Will Kraft (12/24/2019).
        /// </summary>
        public XmlDocumentHistory()
        {
            if (!Directory.Exists(configPath))
                Directory.CreateDirectory(configPath);
        }

        /// <summary>
        /// Force-delete the saved query XML file. --Will Kraft (12/29/2019).
        /// </summary>
        /// <returns></returns>
        public bool DeleteHistory()
        {
            try
            {
                if (HistoryExists)
                    File.Delete(Path.Combine(configPath, "TabHistory.xml"));

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to delete query history: {ex.Message}", "SqlPrep", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Gets the number of saved tabs in the history file. --Will Kraft (12/29/2019).
        /// </summary>
        public int HistoryTabCount
        {
            get
            {
                try
                {
                    XmlDocument settings = new XmlDocument();
                    settings.Load(Path.Combine(configPath, "TabHistory.xml"));

                    XmlNode root = settings.SelectSingleNode("Tabs");

                    XmlNodeList _editors = root.SelectNodes("EditorDuo");
                    return _editors.Count;
                }
                catch
                {
                    return 0;
                }
            }
        }



        /// <summary>
        /// Turn the contents of the XML file into tab restoration events for the main form to handle. --Will Kraft (2/12/2020).
        /// </summary>
        public void RecoverTabs()
        {
            RecoverTabs(Path.Combine(configPath, "TabHistory.xml"));
        }

        /// <summary>
        /// Turn the contents of the XML file into tab restoration events for the main form to handle. --Will Kraft (12/29/2019).
        /// </summary>
        public void RecoverTabs(string path)
        {
            try
            {
                XmlDocument settings = new XmlDocument();
                settings.Load(path);

                XmlNode root = settings.SelectSingleNode("Tabs");

                // rudimentary check for now, better one to come later. --Will Kraft (2/12/2020).
                if (root.SelectNodes("EditorDuo") == null)
                    throw new NullReferenceException();

                XmlNodeList _editors = root.SelectNodes("EditorDuo");

                foreach (XmlNode e in _editors)
                {
                    try
                    {
                        bool.TryParse(e.SelectSingleNode("Implicit").InnerText, out bool useImplicit);
                        bool.TryParse(e.SelectSingleNode("AppendLine").InnerText, out bool useAppendL);
                        int.TryParse(e.SelectSingleNode("Task").InnerText, out int _taskType);
                        int.TryParse(e.SelectSingleNode("LeftPadding").InnerText, out int _lPad);
                        int.TryParse(e.SelectSingleNode("Output").InnerText, out int _outputT);
                        Guid.TryParse(e.Attributes.GetNamedItem("GUID").InnerText, out Guid _tabGuid);

                        RegenerateTab?.Invoke(this, new TabRestorationEventArgs
                        {
                            TabID = _tabGuid,
                            UpperText = e.SelectSingleNode("Upper").InnerText,
                            LowerText = e.SelectSingleNode("Lower").InnerText,
                            TabName = e.SelectSingleNode("TabName").InnerText,
                            Task = (TaskType)_taskType,
                            PrepareArgs = new PrepareEventArgs
                            {
                                VariableName = e.SelectSingleNode("Variable").InnerText,
                                UseVar = useImplicit,
                                UseAppendLine = useAppendL,
                                Cancelled = false,
                                LeftPadding = _lPad,
                                Object = (OutputType)_outputT,
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to load query history: {ex.Message}", "SqlPrep", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("This XML file is not compatible with SqlPrep.", "SqlPrep", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Save the current tabs and their contents/settings to an Xml document. --Will Kraft (2/12/2020).
        /// </summary>
        /// <param name="tabs">Current tab collection.</param>
        public void SaveTabstoXml(ItemCollection tabs)
        {
            SaveTabstoXml(tabs, Path.Combine(configPath, "TabHistory.xml"));
        }

        /// <summary>
        /// Save the current tabs and their contents/settings to an Xml document. --Will Kraft (12/24/2019).
        /// </summary>
        /// <param name="tabs">Current tab collection.</param>
        /// <param name="path"> Output path</param>
        public void SaveTabstoXml(ItemCollection tabs, string path)
        {
            try
            {
                XmlDocument settings = new XmlDocument();

                var _root = settings.CreateElement("Tabs");
                settings.AppendChild(_root);

                foreach (TabItem i in tabs)
                {
                    var _editor = (EditorDuo)i.Content;

                    if (!_editor.Processed)
                        continue;

                    var _tabNode = settings.CreateElement("EditorDuo");

                    var _guid = settings.CreateAttribute("GUID");
                    _guid.InnerText = _editor.ID.ToString();
                    _tabNode.Attributes.Append(_guid);

                    var _upperText = settings.CreateElement("Upper");
                    _upperText.InnerText = _editor.UpperText;
                    _tabNode.AppendChild(_upperText);

                    var _lowerText = settings.CreateElement("Lower");
                    _lowerText.InnerText = _editor.LowerText;
                    _tabNode.AppendChild(_lowerText);

                    var _taskType = settings.CreateElement("Task");
                    _taskType.InnerText = ((int)_editor.Task).ToString();
                    _tabNode.AppendChild(_taskType);

                    var _tabName = settings.CreateElement("TabName");
                    _tabName.InnerText = _editor.TabName;

                    var _linespacing = settings.CreateElement("LeftPadding");
                    _linespacing.InnerText = _editor.Args.LeftPadding.ToString();
                    _tabNode.AppendChild(_linespacing);

                    var _implicit = settings.CreateElement("Implicit");
                    _implicit.InnerText = _editor.Args.UseVar.ToString();
                    _tabNode.AppendChild(_implicit);

                    var _appendl = settings.CreateElement("AppendLine");
                    _appendl.InnerText = _editor.Args.UseAppendLine.ToString();
                    _tabNode.AppendChild(_appendl);

                    var _varN = settings.CreateElement("Variable");
                    _varN.InnerText = _editor.Args.VariableName.ToString();
                    _tabNode.AppendChild(_varN);

                    var _out = settings.CreateElement("Output");
                    _out.InnerText = ((int)_editor.Args.Object).ToString();
                    _tabNode.AppendChild(_out);

                    _tabNode.AppendChild(_tabName);

                    _root.AppendChild(_tabNode);
                    settings.Save(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to save query history: {ex.Message}", "SqlPrep", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
