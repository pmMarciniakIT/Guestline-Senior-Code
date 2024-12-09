using Newtonsoft.Json;

namespace Guestline_hotels_application.Services.Files;

public class ReadFileService : IReadFileService
{
    public async Task<List<T>> GetElementsAsync<T>(string[] arguments, string argumentType) where T : class, new()
    {
        var filePath = GetFileInput(arguments, argumentType);
        var text = await File.ReadAllTextAsync(filePath);

        var result = JsonConvert.DeserializeObject<List<T>>(text, new JsonSerializerSettings
        {
            DateFormatString = "yyyyMMdd"
        }) ?? throw new Exception("File cannot be empty");

        return result;
    }

    private static string GetFileInput(string[] arguments, string argumentInput)
    {
        var argumentIndex = FindIndex(arguments, argumentInput);
        var filePath = GetValue(arguments, ++argumentIndex);

        if (!FileExists(filePath))
        {
            throw new Exception($"Cannot get file from path {filePath}");
        }

        return filePath;
    }


    private static bool FileExists(string filePath)
        => File.Exists(filePath);

    private static int FindIndex(string[] arguments, string argument)
    {
        for (int i = 0; i < arguments.Length - 1; i++)
        {
            if (arguments[i] == argument)
            {
                return i;
            }
        }

        throw new Exception($"Cannot find argument {argument}");
    }

    private static string GetValue(string[] arguments, int index)
    {
        if (index > arguments.Length)
        {
            throw new Exception($"Cannot get starts argument value from index {index}");
        }

        return arguments[index];
    }

}
