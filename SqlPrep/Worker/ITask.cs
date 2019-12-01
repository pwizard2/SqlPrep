using SqlPrep.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
