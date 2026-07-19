using Hw3.FileProcessor.Services.ProcessorFactory;
using Hw3.FileProcessor.Services.SpaceCounter;
using static System.Net.WebRequestMethods;

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

        using var semaphoreSlim = new SemaphoreSlim(1, 2);

        var tasks = filePaths.Select(async filePath =>
        {
            await semaphoreSlim.WaitAsync();
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
            finally
            {
                semaphoreSlim.Release();
            }
        });

        await Task.WhenAll(tasks);

        Console.WriteLine($"Обработка завершена! Всего пробелов: {totalSpaces}");
    }
}
