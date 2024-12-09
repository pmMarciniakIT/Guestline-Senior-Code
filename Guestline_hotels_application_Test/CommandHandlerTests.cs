using Guestline_hotels_application.Models;
using Guestline_hotels_application.Services.Availability;
using Guestline_hotels_application.Services.Search;

namespace Guestline_hotels_application_Test;

public class CommandHandlerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void HandleAvailability_ShouldPrintCorrectAvailability()
    {
        // Arrange
        var hotels = new List<Hotel>
        {
            new Hotel
            {
                Id = "H1",
                Rooms = new List<Room>
                {
                    new Room { RoomType = "DBL", RoomId = "201" },
                    new Room { RoomType = "DBL", RoomId = "202" }
                }
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
            }
        };

        var input = "Availability(H1, 20240902, DBL)";
        var expectedOutput = "1";

        var sut = new AvailabilityService();

        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);

            // Act
            sut.HandleAvailability(input, hotels, bookings);

            // Assert
            var output = sw.ToString().Trim();
            Assert.AreEqual(expectedOutput, output, "The availability output should match the expected value.");
        }
    }

    [Test]
    public void HandleAvailability_ShouldPrintError_WhenHotelOrRoomNotFound()
    {
        // Arrange
        var hotels = new List<Hotel>
        {
            new Hotel
            {
                Id = "H1",
                Rooms = new List<Room>
                {
                    new Room { RoomType = "DBL", RoomId = "201" },
                    new Room { RoomType = "DBL", RoomId = "202" }
                }
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
            }
        };

        var input = "Availability(H2, 20240902, SGL)";
        var expectedOutput = "Hotel not found.";
        var sut = new AvailabilityService();

        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);

            // Act
            sut.HandleAvailability(input, hotels, bookings);

            // Assert
            var output = sw.ToString().Trim();
            Assert.AreEqual(expectedOutput, output, "Error message should be printed when hotel is not found.");
        }
    }

    [Test]
    public void HandleSearch_ShouldDisplayCorrectAvailability_WhenInputIsValid()
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
        var now = DateTime.UtcNow.Date;

        var bookings = new List<Booking>
        {
            new Booking
            {
                HotelId = "H1",
                RoomType = "SGL",
                Arrival = now,
                Departure = now.AddDays(3)
            }
        };

        var input = "Search(H1, 10, SGL)";
        var hotels = new List<Hotel> { hotel };
        var sut = new SearchService();

        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        sut.HandleSearch(input, hotels, bookings);

        // Assert
        var output = consoleOutput.ToString().Trim();
        Assert.IsTrue(output.Contains($"({now.AddDays(3).ToString("yyyyMMdd")}-{now.AddDays(10).ToString("yyyyMMdd")}, 2)"), "HandleSearch should display correct availability.");
    }

    [Test]
    public void HandleSearch_ShouldDisplayErrorMessage_WhenInputIsInvalid()
    {
        // Arrange
        var input = "Search(H1, SGL)";
        var hotels = new List<Hotel>();
        var bookings = new List<Booking>();
        var sut = new SearchService();

        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        sut.HandleSearch(input, hotels, bookings);

        // Assert
        var output = consoleOutput.ToString().Trim();
        Assert.AreEqual("Invalid command format.", output, "HandleSearch should display an error message for invalid input.");
    }

}
