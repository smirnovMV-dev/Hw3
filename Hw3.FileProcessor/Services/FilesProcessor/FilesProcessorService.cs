using Hw3.FileProcessor.Services.ProcessorFactory;
using Hw3.FileProcessor.Services.SpaceCounter;

namespace Hw3.FileProcessor.Services.FilesProcessor;

public interface IFilesProcessorService : IProcessor;

internal class FilesProcessorService : IFilesProcessorService
{
    private readonly ISpaceCounterService _spaceCounterService;

    public FilesProcessorService(ISpaceCounterService spaceCounterService)
    {
        _spaceCounterService = spaceCounterService;
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

        int totalSpaces = 0;

        foreach (var filePath in filePaths)
        {
            if (!File.Exists(filePath))
            {
                continue;
            }

            int spacesInFile = await _spaceCounterService.CountSpacesAsync(filePath);
            totalSpaces += spacesInFile;

            Console.WriteLine($"Файл: {filePath} | Пробелов: {spacesInFile}");
        }

        Console.WriteLine($"Обработка завершена! Всего пробелов: {totalSpaces}");
    }
}
