namespace Hw3.FileProcessor.Services.SpaceCounter;

internal interface ISpaceCounterService
{
    Task<int> CountSpacesAsync(string filePath);
}
