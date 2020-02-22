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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlPrep
{
    /// <summary>
    /// Gets or sets the type of output to produce. 
    /// </summary>
    public enum OutputType
    {
        /// <summary>
        /// Make a string object
        /// </summary>
        String = 0,
        /// <summary>
        /// Make a c# stringbuilder object
        /// </summary>
        StringBuilder = 1
    }

    /// <summary>
    /// Lists the types of tasks this can be. --Will Kraft (12/24/2019).
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        /// Default tab state, before any task has been executed. --Will Kraft (12/24/2019).
        /// </summary>
        Default = 0,

        /// <summary>
        /// Indicates this was a Prepare-related task. --Will Kraft (12/24/2019).
        /// </summary>
        Prepare = 1,

        /// <summary>
        /// Indicates this was a Strip-related task. --Will Kraft (12/24/2019).
        /// </summary>
        Strip = 2

    }

    /// <summary>
    /// Getsthe possible positions for an EditorSingle object. --Will Kraft (2/22/2020).
    /// </summary>
    public enum EditorSinglePosition
    {
        Upper = 0,
        Lower = 1
    }
}
