using Hw3.FileProcessor.Services.ProcessorBase;
using Hw3.FileProcessor.Services.ProcessorFactory;
using Hw3.FileProcessor.Services.SpaceCounter;

namespace Hw3.FileProcessor.Services.DirectoryProcessor;

public interface IDirectoryProcessorService : IProcessor;

internal class DirectoryProcessorService : ProcessorBaseService, IDirectoryProcessorService
{ 
    public DirectoryProcessorService(ISpaceCounterService spaceCounterService)
        : base(spaceCounterService)
    {
    }

    public async Task ProcessAsync(string[] args)
    {
        var directoryPath = args switch
        {
            ["--dir" or "-dir" or "-d", string path] => path,
            _ => null
        };

        if (directoryPath == null || !Directory.Exists(directoryPath))
        {
            return;
        }
        
        await ProcessBaseAsync(Directory.GetFiles(directoryPath));
    }

    public void Process(string[] args)
    {
        var directoryPath = args switch
        {
            ["--dir" or "-dir" or "-d", string path] => path,
            _ => null
        };

        if (directoryPath == null || !Directory.Exists(directoryPath))
        {
            return;
        }

        string[] filesPath = Directory.GetFiles(directoryPath);

        ProcessBase(filesPath);
    }
}