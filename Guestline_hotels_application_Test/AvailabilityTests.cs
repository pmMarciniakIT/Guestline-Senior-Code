using Guestline_hotels_application.Models;
using Guestline_hotels_application.Services.Availability;

namespace Guestline_hotels_application_Test;

[TestFixture]
public class AvailabilityTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CalculateAvailability_ShouldReturnCorrectAvailability()
    {
        // Arrange
        var hotel = new Hotel
        {
            Id = "H1",
            Rooms = new List<Room>
            {
                new Room { RoomType = "SGL", RoomId = "101" },
                new Room { RoomType = "SGL", RoomId = "102" }
            }
        };

        var bookings = new List<Booking>
        {
            new Booking
            {
                HotelId = "H1",
                RoomType = "SGL",
                Arrival = new DateTime(2024, 9, 1),
                Departure = new DateTime(2024, 9, 3)
            }
        };

        var startDate = new DateTime(2024, 9, 1);
        var endDate = new DateTime(2024, 9, 2);
        var sut = new AvailabilityService();

        // Act
        var availability = sut.CalculateAvailability(hotel, bookings, "SGL", startDate, endDate);

        // Assert
        Assert.AreEqual(1, availability, "Availability should be 1 for the given date range.");
    }

    [Test]
    public void CalculateAvailability_ShouldReturnNegativeAvailability_WhenOverbooked()
    {
        // Arrange
        var hotel = new Hotel
        {
            Id = "H1",
            Rooms = new List<Room>
        {
            new Room { RoomType = "DBL", RoomId = "201" },
            new Room { RoomType = "DBL", RoomId = "202" }
        }
        };

        var bookings = new List<Booking>
    {
        new Booking
        {
            HotelId = "H1",
            RoomType = "DBL",
            Arrival = new DateTime(2024, 9, 1),
            Departure = new DateTime(2024, 9, 3)
        },
        new Booking
        {
            HotelId = "H1",
            RoomType = "DBL",
            Arrival = new DateTime(2024, 9, 1),
            Departure = new DateTime(2024, 9, 3)
        },
        new Booking
        {
            HotelId = "H1",
            RoomType = "DBL",
            Arrival = new DateTime(2024, 9, 1),
            Departure = new DateTime(2024, 9, 3)
        }
    };

        var startDate = new DateTime(2024, 9, 1);
        var endDate = new DateTime(2024, 9, 2);
        var sut = new AvailabilityService();

        // Act
        var availability = sut.CalculateAvailability(hotel, bookings, "DBL", startDate, endDate);

        // Assert
        Assert.AreEqual(-1, availability, "Availability should be negative when overbooked.");
    }
}