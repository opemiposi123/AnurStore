namespace AnurStore.Application.Helper
{
    public class Helper
    {
        public static string GetGreetingMessage()
        {
            var currentHour = DateTime.Now.Hour;

            if (currentHour >= 5 && currentHour < 12)
            {
                return "Good Morning";
            }
            else if (currentHour >= 12 && currentHour < 17)
            {
                return "Good Afternoon";
            }
            else if (currentHour >= 17 && currentHour < 21)
            {
                return "Good Evening";
            }
            else
            {
                return "Good Night";
            }
        }

        public static int GetCurrentYear()
        {
            return DateTime.Now.Year;
        }
    }
}
