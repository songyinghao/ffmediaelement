using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unosquare.Threading
{
    /// <summary>
    /// 线程EventArgs
    /// </summary>
    public class ThreadEventArgs : EventArgs
    {
        public Exception Exception { get; }

        public ThreadEventArgs(Exception ex)
        {
            Exception = ex;
        }
    }
}
