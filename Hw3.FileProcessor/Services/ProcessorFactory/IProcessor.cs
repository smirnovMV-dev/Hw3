namespace Hw3.FileProcessor.Services.ProcessorFactory;

public interface IProcessor
{
    Task ProcessAsync(string[] args);

    Task Process(string[] args);
}
