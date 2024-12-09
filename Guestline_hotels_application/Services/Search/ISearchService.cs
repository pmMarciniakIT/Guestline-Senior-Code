using Guestline_hotels_application.Models;

namespace Guestline_hotels_application.Services.Search
{
	public interface ISearchService
	{
        void HandleSearch(string input, List<Hotel> hotels, List<Booking> bookings);
        List<SearchResult> SearchAvailability(Hotel hotel, List<Booking> bookings, string roomType, DateTime startDate, DateTime endDate);
    }
}

