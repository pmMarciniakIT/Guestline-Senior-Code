using Guestline_hotels_application.Models;
using System.Text.RegularExpressions;

namespace Guestline_hotels_application.Services.Search;

public partial class SearchService : ISearchService
{
    [GeneratedRegex(@"Search\((.+?), (.+?), (.+?)\)", RegexOptions.IgnoreCase)]
    private static partial Regex SearchRegex();

    public void HandleSearch(string input, List<Hotel> hotels, List<Booking> bookings)
    {
        var match = SearchRegex().Match(input);
        if (!match.Success)
        {
            Console.WriteLine("Invalid command format.");
            return;
        }

        var (hotelId, daysAhead, roomType) = PrepareValues(match);

        var hotel = hotels.FirstOrDefault(h => h.Id == hotelId);
        if (hotel == null)
        {
            Console.WriteLine("Hotel not found.");
            return;
        }

        if(!hotel.Rooms.Where(r => r.RoomType == roomType).Any())
        {
            Console.WriteLine($"Hotel {hotelId} does not have a room of this type.");
            return;
        }

        var startDate = DateTime.UtcNow.Date;
        var endDate = startDate.AddDays(daysAhead);

        var availabilityRanges = SearchAvailability(hotel, bookings, roomType, startDate, endDate);
        if (availabilityRanges.Count > 0)
        {
            Console.WriteLine(string.Join(", ", availabilityRanges.Select(a => $"({a.StartDate:yyyyMMdd}-{a.EndDate:yyyyMMdd}, {a.Availability})"))); 
            return;
        }

        Console.WriteLine();   
    }

    public List<SearchResult> SearchAvailability(Hotel hotel, List<Booking> bookings, string roomType, DateTime startDate, DateTime endDate)
    {
        List<SearchResult> availableRanges = new();

        var totalRooms = hotel.Rooms.Count(r => r.RoomType == roomType);

        int? currentAvailability = null;

        var currentStart = startDate;
        for (var currentDay = startDate; currentDay < endDate; currentDay = currentDay.AddDays(1))
        {
            var bookedRooms = bookings
                .Where(b => b.HotelId == hotel.Id && b.RoomType == roomType)
                .Count(b => b.Arrival < currentDay.AddDays(1) && b.Departure > currentDay);

            var availableRooms = totalRooms - bookedRooms;

            if (currentAvailability == null) 
            {
                currentAvailability = availableRooms;
                currentStart = currentDay;
            }
            else if (currentAvailability != availableRooms)
            {
                if (currentAvailability > 0)
                {
                    availableRanges.Add(new SearchResult
                    {
                        StartDate = currentStart,
                        EndDate = currentDay,
                        Availability = currentAvailability.Value
                    });
                }

                currentAvailability = availableRooms;
                currentStart = currentDay;
            }
        }

        if (currentAvailability != null && currentAvailability > 0)
        {
            availableRanges.Add(new SearchResult
            {
                StartDate = currentStart,
                EndDate = endDate,
                Availability = currentAvailability.Value
            });
        }

        return availableRanges;
    }

    private (string hotelId, int daysAhead, string roomType) PrepareValues(Match match)
    {
        try
        {
            var hotelID = match.Groups[1].Value;
            var daysAhead = int.Parse(match.Groups[2].Value);
            var roomType = match.Groups[3].Value;

            return (hotelID, daysAhead, roomType);
        }
        catch
        {
            throw new Exception("The Search query must be in the format: Search(<hotelId>, <date ahead>, <room type>)");
        }
    }
}
