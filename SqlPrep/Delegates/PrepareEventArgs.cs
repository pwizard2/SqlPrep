using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlPrep;

namespace SqlPrep.Delegates
{
    public class PrepareEventArgs : EventArgs
    {
        public bool Cancelled { get; set; }

        public int LeftPadding { get; set; }

        public bool UseVar { get; set; }

        public bool UseAppendLine { get; set; }

        public string VariableName { get; set; }

        public OutputType Object { get; set; }
    }

    public delegate void PrepareEventHandler(object sender, PrepareEventArgs e);
}
