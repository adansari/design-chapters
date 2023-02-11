using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher.FileSysRepo
{
    public class FileRepository : IFileSysRepo
    {
        private string _filePath = string.Empty;

        ///<inheritdoc/>
        public virtual string FilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_filePath)) throw new ArgumentNullException("FilePath");
                return _filePath;
            }

            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("FilePath");
                _filePath = value;
            }
        }

        ///<inheritdoc/>
        public virtual async Task<string> ReadFileContentAsync(CancellationToken cancellationToken = default)
        {
            if (!FileExists())
                throw new FileNotFoundException($"{FilePath} is not found at: {Environment.CurrentDirectory}");

            // read file content
            var fileContent = await ReadFileAsync();

            // if file content is empty throw an error
            if (string.IsNullOrEmpty(fileContent))
                throw new IOException($"{FilePath} is empty");

            return fileContent;
        }

        // Virtual wrapper for File.GetLastWriteTime(...) so that it can be mocked in the unit tests
        ///<inheritdoc/>
        public virtual DateTime GetLastModifiedDateTime()
        {
            return File.GetLastWriteTime(FilePath);
        }

        // Virtual wrapper for File.ReadAllTextAsync(...) so that it can be mocked in the unit tests
        protected virtual async Task<string> ReadFileAsync(CancellationToken cancellationToken = default)
        {
            return await File.ReadAllTextAsync(FilePath, cancellationToken);
        }

        // Virtual wrapper for File.Exists(...) so that it can be mocked easily in the unit tests
        protected virtual bool FileExists()
        {
            return File.Exists(FilePath);
        }
    }
}
