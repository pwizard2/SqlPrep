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
    /// <summary>
    /// Interface for all tasks. 
    /// </summary>
    interface ITask
    {
        event ConvertEventHandler TaskDone;

        /// <summary>
        /// Run the current task. 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        string Run(out string error);

        /// <summary>
        /// GHandle the current task for a string result.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        string HandleString(out string error);

        /// <summary>
        /// Handle the current task for a stringbuilder result
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        string HandleStringBuilder(out string error);
    }
}
