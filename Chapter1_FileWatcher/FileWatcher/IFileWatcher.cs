using FileWatcher.Watcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    /// <summary>
    /// Defines the Watch Service states and operations
    /// </summary>
    public interface IWatcherService
    {
        /// <summary>
        /// Get or set the File path to watch
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// Get or set the watching interval
        /// </summary>
        TimeSpan WatchInterval { get; set; }

        /// <summary>
        /// Optionally, get or set when watching should auto-stop
        /// </summary>
        TimeSpan? StopAfter { get; set; }

        /// <summary>
        /// Handler for file change event
        /// </summary>
        event EventHandler<FileChangeEventArgs> FileChanged;

        /// <summary>
        /// Starts watching
        /// </summary>
        /// <returns></returns>
        Task DoWatchAsync();

        /// <summary>
        /// Cancels the watching
        /// </summary>
        void Cancel();

        /// <summary>
        /// Dispose the resources
        /// </summary>
        void Dispose();
    }
}
