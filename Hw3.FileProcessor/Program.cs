using Hw3.FileProcessor.Services.DirectoryProcessor;
using Hw3.FileProcessor.Services.FilesProcessor;
using Hw3.FileProcessor.Services.HelpProcessor;
using Hw3.FileProcessor.Services.ProcessorFactory;
using Hw3.FileProcessor.Services.SpaceCounter;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        ConfigureServices(services);        

        using var serviceProvider = services.BuildServiceProvider();

        try
        {
            var factory = serviceProvider.GetRequiredService<IProcessorFactory>();

            var processor = factory.Create(args);

            Console.WriteLine("Синхронно - последовательно:");
            processor.Process(args);
            
            Console.WriteLine("Асинхронно - параллельно:");
            await processor.ProcessAsync(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IDirectoryProcessorService, DirectoryProcessorService>();
        services.AddTransient<IFilesProcessorService, FilesProcessorService>();
        services.AddTransient<IHelpProcessorService, HelpProcessorService>();

        services.AddTransient<IProcessorFactory, ProcessorFactory>();
        
        services.AddTransient<ISpaceCounterService, SpaceCounterService>();
    }
}