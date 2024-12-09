namespace Guestline_hotels_application.Services.Files
{
	public interface IReadFileService
	{
        Task<List<T>> GetElementsAsync<T>(string[] arguments, string argumentType) where T : class, new();
    }
}

