using Hw3.FileProcessor.Services.ProcessorFactory;
using Hw3.FileProcessor.Services.SpaceCounter;
using System.Diagnostics;

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

        using var semaphoreSlim = new SemaphoreSlim(1, 3);

        Stopwatch stopwatch = Stopwatch.StartNew();

        var tasks = files.Select(async file =>
        {
            var filePath = Path.GetFullPath(file);

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

        stopwatch.Stop();

        Console.WriteLine($"Обработка завершена! Всего пробелов: {totalSpaces}");
        Console.WriteLine($"Время выполнения: {stopwatch.Elapsed.TotalMilliseconds:F2} мс");
    }

    public async Task Process(string[] args)
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

        Stopwatch stopwatch = Stopwatch.StartNew();

        foreach (var file in files)
        {        
            var filePath = Path.GetFullPath(file);
            try
            {
                int spacesInFile = await _spaceCounterService.CountSpacesAsync(filePath);
                Interlocked.Add(ref totalSpaces, spacesInFile);

                Console.WriteLine($"Файл: {filePath} | Пробелов: {spacesInFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка файла {filePath}: {ex.Message}");
            }
        }

        stopwatch.Stop();

        Console.WriteLine($"Обработка завершена! Всего пробелов: {totalSpaces}");
        Console.WriteLine($"Время выполнения: {stopwatch.Elapsed.TotalMilliseconds:F2} мс");
    }
}