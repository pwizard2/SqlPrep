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

using System;
using System.IO;
using System.Windows.Controls;
using System.Xml;

namespace SqlPrep
{
    public class XmlDocumentHistory
    {

        readonly string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SqlPrep");

        public XmlDocumentHistory()
        {
            if (!Directory.Exists(configPath))
                Directory.CreateDirectory(configPath);
        }

        /// <summary>
        /// Save the current tabs and their contents/settings to an Xml document. --Will Kraft (12/24/2019).
        /// </summary>
        /// <param name="tabs">Current tab collection.</param>
        internal void SaveTabstoXml(ItemCollection tabs)
        {
            XmlDocument settings = new XmlDocument();

            var _root = settings.CreateElement("Tabs");
            settings.AppendChild(_root);

            foreach (TabItem i in tabs)
            {
                var _editor = (EditorDuo)i.Content;

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
                
                var _appendl= settings.CreateElement("AppendLine");
                _appendl.InnerText = _editor.Args.UseAppendLine.ToString();
                _tabNode.AppendChild(_appendl);   
                
                var _varN= settings.CreateElement("Variable");
                _varN.InnerText = _editor.Args.VariableName.ToString();
                _tabNode.AppendChild(_varN);
                
                var _out= settings.CreateElement("Output");
                _out.InnerText = ((int)_editor.Args.Object).ToString();
                _tabNode.AppendChild(_out);

                _tabNode.AppendChild(_tabName);

                _root.AppendChild(_tabNode);
            }


            settings.Save(Path.Combine(configPath, "TabHistory.xml"));
        }
    }
}
