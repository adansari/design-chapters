using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher.Watcher
{
    public class FileWatcherService : IWatcherService
    {
        public event EventHandler<FileChangeEventArgs> FileChanged;

        private IFileSysRepo _repo;
        private CancellationTokenSource _cancelTheTask;
        private CancellationToken _cancellToken;
        private PeriodicTimer _timer;
        private TimeSpan _watchInterval;
        private TimeSpan? _stopAfter;
        private DateTime _lastFileModifiedDateTime;

        ///<inheritdoc/>
        public string FilePath
        {
            get { return _repo.FilePath; }
            set { _repo.FilePath = value; }
        }

        ///<inheritdoc/>
        public TimeSpan WatchInterval
        {
            get { return _watchInterval; }
            set { _watchInterval = value; }
        }


        ///<inheritdoc/>
        public TimeSpan? StopAfter
        {
            get { return _stopAfter; }
            set { _stopAfter = value; }
        }


        public FileWatcherService(IFileSysRepo repo)
        {
            _repo = repo;
            _cancelTheTask = new CancellationTokenSource();
            _cancellToken = _cancelTheTask.Token;
        }

        ///<inheritdoc/>
        public async Task DoWatchAsync()
        {
            // auto stop after given time span
            DateTime watchUntil = DateTime.MaxValue;
            if (_stopAfter != null) watchUntil = DateTime.Now.Add(_stopAfter.Value);

            // dispose off the timer when monitoring ends
            using (_timer = new PeriodicTimer(_watchInterval))
            {
                // track the file's last modified datetime for any change
                _lastFileModifiedDateTime = DateTime.MinValue;

                while (await _timer.WaitForNextTickAsync(_cancellToken))
                {
                    if (HasFileChanged())
                    {
                        _lastFileModifiedDateTime = _repo.GetLastModifiedDateTime();
                        var args = new FileChangeEventArgs();
                        try
                        {
                            args.FileContent = await _repo.ReadFileContentAsync(_cancellToken);// get the latest file content
                            args.LastModifiedDateTime = _lastFileModifiedDateTime; // get the latest modified date
                            args.FilePath = _repo.FilePath;

                            // stop monitoring if Cancel  
                            if (_cancellToken.IsCancellationRequested)
                                break;
                        }
                        catch (Exception ex)
                        {
                            args.FileContent = null;
                            args.LastModifiedDateTime = _lastFileModifiedDateTime;
                            args.IsError = true;
                            args.Error = ex;
                        }

                        RaiseFileChangeEvent(args); // raise the file change event
                    }

                    if (_stopAfter != null)
                    {
                        // auto stop after given time span
                        if (DateTime.Now > watchUntil) break;
                    }
                }
            }
        }

        protected virtual bool HasFileChanged()
        {
            return _repo.GetLastModifiedDateTime() > _lastFileModifiedDateTime;
        }

        ///<inheritdoc/>
        public void Cancel()
        {
            // Cancel the thread by notifying through the Task Cancellation token
            _cancelTheTask.Cancel();
        }

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaiseFileChangeEvent(FileChangeEventArgs eventArgs)
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            FileChanged?.Invoke(this, eventArgs);
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

