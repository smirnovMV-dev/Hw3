namespace Hw3.FileProcessor.Services.ProcessorFactory;

public interface IProcessorFactory
{
    public IProcessor Create(string[] args);
}
