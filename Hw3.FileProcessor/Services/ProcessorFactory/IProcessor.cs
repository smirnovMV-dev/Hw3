namespace Hw3.FileProcessor.Services.ProcessorFactory;

public interface IProcessor
{
    Task ProcessAsync(string[] args);

    void Process(string[] args);
}
