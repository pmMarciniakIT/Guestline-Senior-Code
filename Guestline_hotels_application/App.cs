using Guestline_hotels_application.Models;
using Guestline_hotels_application.Services.Availability;
using Guestline_hotels_application.Services.Files;
using Guestline_hotels_application.Services.Search;
using Guestline_hotels_application.Validators;

namespace Guestline_hotels_application
{
	public class App
	{
        private readonly IAvailabilityService _availabilityService;
        private readonly IReadFileService _readFileService;
        private readonly ISearchService _searchService;

        public App(IAvailabilityService availabilityService, IReadFileService readFileService, ISearchService searchService)
        {
            _availabilityService = availabilityService;
            _readFileService = readFileService;
            _searchService = searchService;
        }
        public async Task RunAsync(string[] args)
		{
            if (!InputValidator.ValidateInput(args))
            {
                return;
            }

            var hotels = await _readFileService.GetElementsAsync<Hotel>(args, "--hotels");
            var bookings = await _readFileService.GetElementsAsync<Booking>(args, "--bookings");

            Console.WriteLine("The files have been read successfully. Use the commands: Availability or Search. Sending an empty message will terminate the program.");

            string input;

            while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
            {
                if (input.StartsWith("Availability"))
                {
                    _availabilityService.HandleAvailability(input, hotels, bookings);
                }
                else if (input.StartsWith("Search"))
                {
                    _searchService.HandleSearch(input, hotels, bookings);
                }
                else
                {
                    Console.WriteLine("Invalid command.");
                }
            }
        }
	}
}

