namespace Guestline_hotels_application.Validators
{
	public static class InputValidator
	{
        public static bool ValidateInput(string[] arguments)
        {
            if (!arguments.Contains("--hotels") || !arguments.Contains("--bookings"))
            {
                Console.WriteLine("To run the program, you must provide the path to the hotels and booking files, e.g. --hotels hotels.json --bookings bookings.json");
                return false;
            }

            return true;
        }
    }
}