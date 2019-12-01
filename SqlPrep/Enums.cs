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
}
