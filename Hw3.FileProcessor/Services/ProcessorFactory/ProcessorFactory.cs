using Hw3.FileProcessor.Services.HelpProcessor;
using Hw3.FileProcessor.Services.DirectoryProcessor;
using Hw3.FileProcessor.Services.FilesProcessor;

namespace Hw3.FileProcessor.Services.ProcessorFactory;

public class ProcessorFactory : IProcessorFactory
{
    private readonly IDirectoryProcessorService _directoryProcessorService;
    private readonly IFilesProcessorService _filesProcessorService;
    private readonly IHelpProcessorService _helpProcessorService;

    public ProcessorFactory(
        IDirectoryProcessorService directoryProcessorService,
        IFilesProcessorService filesProcessorService,
        IHelpProcessorService helpProcessorService)
    {
        _directoryProcessorService = directoryProcessorService;
        _filesProcessorService = filesProcessorService;
        _helpProcessorService = helpProcessorService;
    }

    public IProcessor Create(string[] args)
    {
        return args switch
        {
            ["--dir" or "-dir" or "-d", string] => _directoryProcessorService,
            ["--files" or "-files" or "-f", .. string[]] => _filesProcessorService,
            _ => _helpProcessorService
        };
    }
}
