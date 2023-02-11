using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    /// <summary>
    /// Defines the File IO operations
    /// </summary>
    public interface IFileSysRepo
    {
        /// <summary>
        /// Get or set the File path to read from 
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// Reads the file text
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the async operation</param>
        /// <returns></returns>
        Task<string> ReadFileContentAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the last modified date of file
        /// </summary>
        /// <returns></returns>
        DateTime GetLastModifiedDateTime();
    }
}
