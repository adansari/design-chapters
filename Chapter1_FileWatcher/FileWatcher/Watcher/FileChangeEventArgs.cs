using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher.Watcher
{
    public class FileChangeEventArgs
    {
        public string FilePath { get; set; }
        public string FileContent { get; set; }

        public DateTime LastModifiedDateTime { get; set; }

        public bool IsError { get; set; } = false;

        public Exception Error { get; set; }
    }
}
