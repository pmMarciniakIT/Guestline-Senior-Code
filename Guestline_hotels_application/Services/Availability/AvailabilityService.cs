using Guestline_hotels_application.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Guestline_hotels_application.Services.Availability;

public partial class AvailabilityService : IAvailabilityService
{
    [GeneratedRegex(@"Availability\((.+?), (.+?), (.+?)\)")]
    private static partial Regex AvailabilityRegex();

    public void HandleAvailability(string input, List<Hotel> hotels, List<Booking> bookings)
    {
        var match = AvailabilityRegex().Match(input);
        if (!match.Success)
        {
            Console.WriteLine("Invalid command format.");
            return;
        }

        var (hotelID, dateRange, roomType) = PrepareValues(match);
        var hotel = hotels.FirstOrDefault(h => h.Id == hotelID);

        if (hotel == null)
        {
            Console.WriteLine("Hotel not found.");
            return;
        }

        var (startDate, endDate) = PrepareDateRange(dateRange);

        var availability = CalculateAvailability(hotel, bookings, roomType, startDate, endDate);
        Console.WriteLine(availability);
    }

    public int CalculateAvailability(Hotel hotel, List<Booking> bookings, string roomType, DateTime startDate, DateTime endDate)
    {
        var rooms = hotel.Rooms.Where(r => r.RoomType == roomType).ToList();
        var totalRooms = rooms.Count;

        var overlappingBookings = bookings
            .Where(b => b.HotelId == hotel.Id && b.RoomType == roomType &&
                        !(b.Departure <= startDate || b.Arrival >= endDate))
            .ToList();

        var bookedRooms = overlappingBookings.Count;

        return totalRooms - bookedRooms;
    }

    private (string hotelID, string dateRange, string roomType) PrepareValues(Match match)
    {
        try
        {
            var hotelID = match.Groups[1].Value;
            var dateRange = match.Groups[2].Value;
            var roomType = match.Groups[3].Value;

            return (hotelID, dateRange, roomType);
        }
        catch
        {
            throw new Exception("The Availability query must be in the format: Availability(<hotelId>, <date or date-range>, <room type>)");
        }
    }

    private (DateTime startDate, DateTime endDate) PrepareDateRange(string dateRange)
    {
        DateTime startDate, endDate;

        try
        {
            if (dateRange.Contains('-'))
            {
                var dates = dateRange.Split('-');
                startDate = DateTime.ParseExact(dates[0], "yyyyMMdd", CultureInfo.InvariantCulture);
                endDate = DateTime.ParseExact(dates[1], "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            else
            {
                startDate = endDate = DateTime.ParseExact(dateRange, "yyyyMMdd", CultureInfo.InvariantCulture);
            }

            return (startDate, endDate);
        }
        catch
        {
            throw new Exception("The Availability query must be in the format: Availability(<hotelId>, <date or date-range>, <room type>)");
        }
    }

}
