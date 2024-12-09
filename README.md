# Hotel Room Availability and Reservation System

## Overview

This application is designed to manage hotel room availability and reservations. It processes data from input files containing hotel and booking details and provides commands to check room availability and search for available date ranges for a specific room type.

The program is intended to demonstrate clean, maintainable code and solve the challenge as outlined in the requirements.

---

## Features

- **Check Room Availability**: Determine the number of rooms available for a specific hotel, room type, and date range.
- **Search Available Date Ranges**: Find periods when specific room types are available within a defined time frame.
- **Support for Overbooking**: Displays negative availability when overbooked.

---

## Installation

1. **Clone the Repository**:
   ```bash
   git clone <repository_url>
   cd <repository_folder>
   ```

2. **Build the Project**:
   Ensure you have the .NET SDK installed. Use the following command:
   ```bash
   dotnet build
   ```

3. **Run Tests**:
   Use the `dotnet test` command to execute all unit tests:
   ```bash
   dotnet test
   ```

---

## Usage

### Running the Program

Run the application using the following command:
```bash
Guestline_hotels_application --hotels hotels.json --bookings bookings.json
```

### Commands

#### **Availability**
Check the availability of a specific room type in a hotel for a given date range.

Example:
```plaintext
Availability(H1, 20240901, SGL)
Availability(H1, 20240901-20240903, DBL)
```

Output:
```plaintext
Number of availability room.
Example: 1
Example: -2
```

#### **Search**
Find available date ranges for a specific room type in a hotel within a specified number of days ahead.

Example:
```plaintext
Search(H1, 365, SGL)
```

Output:
```plaintext
Example result: (20241101-20241103, 2), (20241203-20241210, 1)
```

---

## Assumptions

- Input files follow the structure outlined above.
- Dates are in `yyyyMMdd` format.
- Overbooking is indicated by negative availability values.
- The program assumes valid JSON file paths and formats.
