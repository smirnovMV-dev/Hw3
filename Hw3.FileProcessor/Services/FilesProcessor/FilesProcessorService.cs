using Hw3.FileProcessor.Services.ProcessorBase;
using Hw3.FileProcessor.Services.ProcessorFactory;
using Hw3.FileProcessor.Services.SpaceCounter;

namespace Hw3.FileProcessor.Services.FilesProcessor;

public interface IFilesProcessorService : IProcessor;

internal class FilesProcessorService : ProcessorBaseService, IFilesProcessorService
{
    public FilesProcessorService(ISpaceCounterService spaceCounterService)
        : base(spaceCounterService)
    { 
    }

    public async Task ProcessAsync(string[] args)
    {
        string[] filePaths = args switch
        {
            ["--files" or "-files" or "-f", .. string[] paths] => paths,
            _ => [] 
        };

        if (filePaths.Length == 0)
        {
            return;
        }

        await ProcessBaseAsync(filePaths);
    }

    public void Process(string[] args)
    {
        string[] filesPath = args switch
        {
            ["--files" or "-files" or "-f", .. string[] paths] => paths,
            _ => []
        };

        if (filesPath.Length == 0)
        {
            return;
        }

        ProcessBase(filesPath);
    }
}
