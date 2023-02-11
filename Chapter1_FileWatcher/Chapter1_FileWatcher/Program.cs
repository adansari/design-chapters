
using FileWatcher;
using FileWatcher.FileSysRepo;
using FileWatcher.Watcher;

class Program
{

    static async Task Main(string[] args)
    {
        IWatcherService watcher = new FileWatcherService(new FileRepository())
        {
            FilePath = "test.txt",
            WatchInterval = new TimeSpan(0, 0, 5)
        };

        watcher.FileChanged += Watcher_FileChanged;

        try
        {
            Console.WriteLine($"Watching file ...  {watcher.FilePath}");
            await watcher.DoWatchAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    static void Watcher_FileChanged(object? sender, FileChangeEventArgs e)
    {
        if (e.IsError) throw e.Error;

        Console.WriteLine($"file: {e.FilePath} has changed!!");
        Console.WriteLine($"fileContent:");
        Console.WriteLine(e.FileContent);
        Console.WriteLine("========== end ==========");
    }
}