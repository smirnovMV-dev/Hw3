using Hw3.FileProcessor.Services.ProcessorFactory;

namespace Hw3.FileProcessor.Services.HelpProcessor;

public interface IHelpProcessorService : IProcessor;

public class HelpProcessorService : IHelpProcessorService
{
    public Task ProcessAsync(string[] args)
    => ShowHelp();

    public void Process(string[] args)
    => ShowHelp();

    private static Task ShowHelp()
    {
        Console.WriteLine("Параметры программы:");
        Console.WriteLine("  -d, -dir, --dir  ->  Обработать директорию");
        Console.WriteLine("  -f, -files, --files  ->  Обработать список файлов, разделитель пробел");

        return Task.CompletedTask;
    }
}
