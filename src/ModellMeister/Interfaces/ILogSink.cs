using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellMeister.Interfaces
{
    /// <summary>
    /// The interface being used as a log sink
    /// </summary>
    public interface ILogSink
    {
        /// <summary>
        /// Clears all log messages
        /// </summary>
        void ClearLogMessages();

        /// <summary>
        /// Adds message to the log
        /// </summary>
        /// <param name="message">Log to be added</param>
        void AddMessageToLog(string message);
    }
}
