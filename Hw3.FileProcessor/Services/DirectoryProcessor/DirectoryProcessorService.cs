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

        using var semaphoreSlim = new SemaphoreSlim(1, 2);

        var tasks = files.Select(async file =>
        {
            var filePath = Path.GetFileName(file);

            await semaphoreSlim.WaitAsync();
            try
            {
                int spacesInFile = await _spaceCounterService.CountSpacesAsync(file);                
                Interlocked.Add(ref totalSpaces, spacesInFile);

                Console.WriteLine($"Файл: {filePath} | Пробелов: {spacesInFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка файла {filePath}: {ex.Message}");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        });

        await Task.WhenAll(tasks);

        Console.WriteLine($"Обработка завершена! Всего пробелов: {totalSpaces}");
    }
}