using Hw3.FileProcessor.Services.ProcessorFactory;
using Hw3.FileProcessor.Services.SpaceCounter;

namespace Hw3.FileProcessor.Services.DirectoryProcessor;

public interface IDirectoryProcessorService : IProcessor;

internal class DirectoryProcessorService : IDirectoryProcessorService
{
    private readonly ISpaceCounterService _spaceCounterService;

    public DirectoryProcessorService(ISpaceCounterService spaceCounterService)
    {
        _spaceCounterService = spaceCounterService;
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

        string[] files = Directory.GetFiles(directoryPath);
        int totalSpaces = 0;

        foreach (var file in files)
        {
            var filePath = Path.GetFullPath(file);
            var spacesInFile = await _spaceCounterService.CountSpacesAsync(filePath);
            totalSpaces += spacesInFile;
            Console.WriteLine($"Файл: {filePath} | Пробелов: {spacesInFile}");
        }

        Console.WriteLine($"Обработка завершена! Всего пробелов: {totalSpaces}");
    }
}