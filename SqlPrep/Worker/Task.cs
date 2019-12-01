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

namespace SqlPrep.Worker
{
    public abstract class BaseTask : ITask
    {

        protected internal string Input { get; set; }

        protected int CurrentLineNum;

        /// <summary>
        /// Gets or sets the text that should be shown on the current tab's label after a task is done.
        /// </summary>
        protected string TabLabel { get; set; }

        public bool DevMode { get; set; }

        public BaseTask(string _content)
        {
            Input = _content;
            CurrentLineNum = 0;
        }

        public event ConvertEventHandler TaskDone;

        protected void Done(ConverterEventArgs e)
        {
            TaskDone?.Invoke(this, e);
        }

        /// <summary>
        /// Run the current task (base). --Will Kraft (10/27/19).
        /// </summary>
        /// <param name="error">Error from task execution, if any.</param>
        /// <returns>processed string.</returns>
        public abstract string Run(out string error);

        /// <summary>
        /// Handle the current task for a string result. --Will Kraft (11/17/19).
        /// </summary>
        /// <param name="error">Error from task execution, if any.</param>
        /// <returns>processed string.</returns>
        public abstract string HandleString(out string error);


        /// <summary>
        /// Handle the current task for a string result. --Will Kraft (11/17/19).
        /// </summary>
        /// <param name="error">Error from task execution, if any.</param>
        /// <returns>processed string.</returns>
        public abstract string HandleStringBuilder(out string error);

    }
}
