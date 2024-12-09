using Guestline_hotels_application.Models;
using Guestline_hotels_application.Services.Search;

namespace Guestline_hotels_application_Test
{
    [TestFixture]
    public class SearchTests
	{
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SearchAvailability_ShouldReturnCorrectDateRanges_WhenRoomsAreAvailable()
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
                    Departure = new DateTime(2024, 9, 5)
                }
            };

            var startDate = new DateTime(2024, 9, 1);
            var endDate = new DateTime(2024, 9, 11);
            var sut = new SearchService();

            // Act
            var result = sut.SearchAvailability(hotel, bookings, "SGL", startDate, endDate);

            // Assert
            var expected = new List<SearchResult>
            {
                new SearchResult
                {
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2024, 9, 5),
                    Availability = 1
                },
                new SearchResult
                {
                    StartDate = new DateTime(2024, 9, 5),
                    EndDate = new DateTime(2024, 9, 11),
                    Availability = 2
                }
            };

            Assert.AreEqual(expected.Count, result.Count, "SearchAvailability should return the correct number of date ranges.");
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].StartDate, result[i].StartDate, $"Mismatch in StartDate at index {i}");
                Assert.AreEqual(expected[i].EndDate, result[i].EndDate, $"Mismatch in EndDate at index {i}");
                Assert.AreEqual(expected[i].Availability, result[i].Availability, $"Mismatch in Availability at index {i}");
            }
        }


        [Test]
        public void SearchAvailability_ShouldReturnEmptyList_WhenNoRoomsAreAvailable()
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
                    Departure = new DateTime(2024, 9, 10)
                },
                new Booking
                {
                    HotelId = "H1",
                    RoomType = "DBL",
                    Arrival = new DateTime(2024, 9, 1),
                    Departure = new DateTime(2024, 9, 10)
                }
            };

            var startDate = new DateTime(2024, 9, 1);
            var endDate = new DateTime(2024, 9, 10);
            var sut = new SearchService();

            // Act
            var result = sut.SearchAvailability(hotel, bookings, "DBL", startDate, endDate);

            // Assert
            Assert.IsEmpty(result, "SearchAvailability should return an empty list when no rooms are available.");
        }

    }
}

