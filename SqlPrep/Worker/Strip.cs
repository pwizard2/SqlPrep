﻿using SqlPrep.Delegates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SqlPrep.Worker
{
    public class Strip : BaseTask
    {
        readonly string sbPattern = @"^\s*(var|stringbuilder)\s+\w+\s*\=\s*new\s+StringBuilder\(\)\;\s*";

        /// <summary>
        /// Task that strips string formatting from a snippet and returns pure SQL. --Will Kraft (10/27/19).
        /// </summary>
        /// <param name="_content"></param>
        public Strip(string _content) : base(_content)
        { }

        public override string HandleString(out string error)
        {
            error = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(Input))
                    throw new Exception("No text to process.");

                var lineCount = Input.Length - Input.Replace(Environment.NewLine, string.Empty).Length;
                var sb = new StringBuilder();

                using (var tr = new StringReader(Input))
                {
                    var _data = string.Empty;
                    var lead = 0;

                    do
                    {
                        _data = tr.ReadLine();

                        if (CurrentLineNum == 0)
                        {
                            if (!Regex.IsMatch(_data.TrimStart(), @"^\s*(var|string)\s+.+=\s+\"""))
                                throw new Exception("Invalid input string detected. This method only accepts valid C# string variable statements beginning with \"var\" or \"string\".");

                            lead = _data.IndexOf("=", StringComparison.InvariantCulture);

                            TabLabel = _data.Substring(0, lead).Replace("string", string.Empty).Replace("var", string.Empty).Trim();
                        }

                        if (!string.IsNullOrEmpty(_data))
                        {
                            if (_data.Contains("+ \""))
                            {
                                var _cleaned = _data.Substring(_data.IndexOf("+", StringComparison.InvariantCulture) + 3);

                                _cleaned = _cleaned.Contains("\";") ? _cleaned.Substring(0, _cleaned.Length - 2)
                                    : _cleaned.Substring(0, _cleaned.Length - 1);

                                if (!Regex.IsMatch(_cleaned, @"\s+\;") && !string.IsNullOrEmpty(_cleaned))
                                    sb.AppendLine(_cleaned);
                            }
                            else if (_data.Contains("= \""))
                            {

                                var _cleaned = _data.Substring(_data.IndexOf("=", StringComparison.InvariantCulture) + 3);

                                _cleaned = _cleaned.Substring(0, _cleaned.Length - 1);

                                if (!Regex.IsMatch(_cleaned, @"\s+\;"))
                                    sb.AppendLine(_cleaned);
                            }
                            else
                            {
                                if (!Regex.IsMatch(_data, @"\s+\;"))
                                    sb.AppendLine(_data);
                            }
                        }

                        CurrentLineNum++;

                    } while (_data != null);

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {

                error = DevMode ? ex.ToString() : ex.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// Extract the raw SQL from a stringbuilder list. --Will Kraft (11/28/19).
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public override string HandleStringBuilder(out string error)
        {
            error = string.Empty;
            try
            {
                var VariableFinder = Regex.Match(Input, @"\s*(\w+)\s*\=\s*new\s+StringBuilder\(\)\;\s*", RegexOptions.IgnoreCase);

                var _data = string.Empty;

                var varName = VariableFinder.Groups[1].Value;
                TabLabel = varName;

                if (string.IsNullOrEmpty(varName))
                    throw new Exception("Unable to isolate variable name from stringbuilder.");

                var sb = new StringBuilder();

                string sbLinePattern = "\\.(?:Append|AppendLine)\\(\"(.+)\"\\)\\;$";

                using (var tr = new StringReader(Input))
                {
                    do
                    {
                        _data = tr.ReadLine();

                        if (string.IsNullOrEmpty(_data))
                            break;

                        if (!Regex.IsMatch(_data, sbLinePattern))
                            continue;

                        // remove the variabvle name from the line to simplify regex.
                        var _data2 = _data.Replace(varName, string.Empty);

                        var nextlineFinder = Regex.Match(_data, sbLinePattern, RegexOptions.IgnoreCase);

                        if (!string.IsNullOrEmpty(nextlineFinder.Groups[1].Value))
                            sb.AppendLine(nextlineFinder.Groups[1].Value);
                    }
                    while (_data != null);
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                error = DevMode ? ex.ToString() : ex.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// Do a Strip operation. --Will Kraft (10/27/19).
        /// </summary>
        /// <param name="error">Error from task execution, if any.</param>
        /// <returns></returns>
        public override string Run(out string error)
        {
            error = string.Empty;

            try
            {
                if (Regex.IsMatch(Input, sbPattern, RegexOptions.IgnoreCase))
                {
                    return HandleStringBuilder(out error);
                }
                else
                {
                    return HandleString(out error);
                }

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
                    TabTextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fa781b")),
                    OutputBG = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fff3cf")),
                    OutputSelection = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9c4c24"))
                });
            }
        }
    }
}