using Guestline_hotels_application.Models;

namespace Guestline_hotels_application.Services.Availability
{
	public interface IAvailabilityService
	{
        void HandleAvailability(string input, List<Hotel> hotels, List<Booking> bookings);
        int CalculateAvailability(Hotel hotel, List<Booking> bookings, string roomType, DateTime startDate, DateTime endDate);
    }
}

