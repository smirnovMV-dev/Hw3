namespace Hw3.FileProcessor.Services.SpaceCounter;

internal sealed class SpaceCounterService : ISpaceCounterService
{
    public async Task<int> CountSpacesAsync(string filePath)
    {
        int spaceCount = 0;

        try
        {
            using var reader = new StreamReader(filePath);

            char[] buffer = new char[4096];
            int bytesRead;

            while ((bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    if (buffer[i] == ' ')
                    {
                        spaceCount++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке {filePath}: {ex.Message}");
        }

        return spaceCount;
    }

    public int CountSpaces(string filePath)
    {
        int spaceCount = 0;

        try
        {
            using var reader = new StreamReader(filePath);

            char[] buffer = new char[4096];
            int bytesRead;

            while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    if (buffer[i] == ' ')
                    {
                        spaceCount++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке {filePath}: {ex.Message}");
        }

        return spaceCount;
    }
}
