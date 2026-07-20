using Hw3.FileProcessor.Services.SpaceCounter;
using System.Diagnostics;

namespace Hw3.FileProcessor.Services.ProcessorBase;

internal abstract class ProcessorBaseService
{
    private readonly ISpaceCounterService _spaceCounterService;

    protected ProcessorBaseService(ISpaceCounterService spaceCounterService)
    {
        _spaceCounterService = spaceCounterService;
    }

    protected async Task ProcessBaseAsync(string[] filesPath)
    {
        var totalSpaces = 0;

        Stopwatch stopwatch = Stopwatch.StartNew();

        var tasks = filesPath.Select(async filePath =>
        {
            try
            {
                var spacesInFile = await _spaceCounterService.CountSpacesAsync(filePath);
                totalSpaces += spacesInFile;

                Console.WriteLine($"Файл: {filePath} | Пробелов: {spacesInFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка файла {filePath}: {ex.Message}");
            }
        });

        await Task.WhenAll(tasks);

        stopwatch.Stop();

        Console.WriteLine($"Обработка завершена! Всего пробелов: {totalSpaces}");
        Console.WriteLine($"Время выполнения: {stopwatch.Elapsed.TotalMilliseconds:F2} мс");
    }

    protected void ProcessBase(string[] filesPath)
    {
        var totalSpaces = 0;

        Stopwatch stopwatch = Stopwatch.StartNew();

        foreach (var filePath in filesPath)
        {
            try
            {
                var spacesInFile = _spaceCounterService.CountSpaces(filePath);
                totalSpaces += spacesInFile;

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
