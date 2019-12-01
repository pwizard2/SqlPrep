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
using System.IO;
using System.Text;
using System.Windows.Media;

namespace SqlPrep.Worker
{
    public class Prepare : BaseTask
    {
        PrepareEventArgs Args { get; set; }

        public Prepare(PrepareEventArgs args, string _content) : base(_content)
        {
            Args = args;
        }

        public override string Run(out string error)
        {
            error = string.Empty;

            try
            {
                var _output = string.Empty;
                var _err = string.Empty;

                switch (Args.Object)
                {
                    default:
                    case OutputType.String:
                        _output = HandleString(out _err);
                        break;

                    case OutputType.StringBuilder:
                        _output = HandleStringBuilder(out _err);
                        break;
                }

                if (!string.IsNullOrEmpty(_err))
                    throw new Exception(_err);

                return _output;
            }
            catch (Exception ex)
            {
                error = DevMode ? ex.ToString() : ex.Message;
                return string.Empty;
            }
            finally
            {
                Done(new ConverterEventArgs
                {
                    TabName = TabLabel,
                    Successful = string.IsNullOrEmpty(error),
                    TabTextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1b80fa")),
                    OutputBG = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d9edff")),
                    OutputSelection = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#486394"))
                });
            }
        }

        public override string HandleStringBuilder(out string error)
        {
            error = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(Input))
                    throw new Exception("No text to process.");

                var sb = new StringBuilder();

                TabLabel = string.IsNullOrEmpty(Args.VariableName) ? "_query" : Args.VariableName;
                var _extraPad = Args.LeftPadding;
                var _varType = Args.UseVar ? "var" : "StringBuilder";
                _ = Input.Length - Input.Replace(Environment.NewLine, string.Empty).Length;


                var _firstLine = new StringBuilder();

                _firstLine.Append(_varType);
                _firstLine.Append(" ");
                _firstLine.Append(TabLabel);
                _firstLine.Append(" = new StringBuilder();");


                sb.AppendLine(_firstLine.ToString());

                using (var tr = new StringReader(Input))
                {
                    var _data = string.Empty;

                    do
                    {
                        _data = tr.ReadLine();

                        if (!string.IsNullOrEmpty(_data))
                        {
                            _data = _data.TrimEnd();
                        }
                        else
                        {
                            continue;
                        }

                        var _nextLine = new StringBuilder();

                        _nextLine.Append(Args.UseAppendLine ? $"{Args.VariableName}.AppendLine(\"" : $"{Args.VariableName}.Append(\"");

                        _nextLine.Append(_data);
                        _nextLine.Append("\");");

                        sb.AppendLine(_nextLine.ToString());

                        CurrentLineNum++;
                    }
                    while (_data != null);

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                error = DevMode ? ex.ToString() : ex.Message;
                return string.Empty;
            }
        }

        public override string HandleString(out string error)
        {
            error = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(Input))
                    throw new Exception("No text to process.");

                var sb = new StringBuilder();

                TabLabel = string.IsNullOrEmpty(Args.VariableName) ? "_query" : Args.VariableName;
                var _extraPad = Args.LeftPadding;
                var _varType = Args.UseVar ? "var" : "string";
                _ = Input.Length - Input.Replace(Environment.NewLine, string.Empty).Length;

                using (var tr = new StringReader(Input))
                {
                    var _data = string.Empty;

                    do
                    {
                        _data = tr.ReadLine();

                        if (!string.IsNullOrEmpty(_data))
                        {
                            _data = _data.TrimEnd();
                        }
                        else
                        {
                            continue;
                        }

                        if (CurrentLineNum == 0)
                        {
                            var _nextLine = new StringBuilder();

                            _nextLine.Append(_varType);
                            _nextLine.Append(" ");
                            _nextLine.Append(TabLabel);
                            _nextLine.Append(" = \" ");
                            _nextLine.Append(_data);
                            _nextLine.Append("\"");

                            sb.AppendLine(_nextLine.ToString());
                        }
                        else
                        {
                            // Prune out empty lines. --Will Kraft (11/30/2019).
                            if (string.IsNullOrEmpty(_data))
                                continue;

                            var _nextLine = new StringBuilder();

                            for (int i = 0; i < TabLabel.Length + (_varType.Length + 2) + _extraPad; i++)
                                _nextLine.Append(" ");

                            _nextLine.Append("+ \" ");

                            _nextLine.Append(_data);
                            _nextLine.Append("\"");

                            sb.AppendLine(_nextLine.ToString());
                        }

                        CurrentLineNum++;
                    }
                    while (_data != null);

                    var _lastLine = new StringBuilder();

                    for (int i = 0; i < TabLabel.Length + (_varType.Length + 2) + _extraPad; i++)
                        _lastLine.Append(" ");

                    _lastLine.Append(";");

                    sb.Append(_lastLine.ToString());

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                error = DevMode ? ex.ToString() : ex.Message;
                return string.Empty;
            }
        }
    }
}

