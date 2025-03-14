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

        public decimal RoundToNearestTen(decimal number)
        {
            decimal remainder = number % 10;
            decimal value = (remainder >= 5) ? number - remainder + 10 : number - remainder;

            return value;
        }

        public  decimal CalculateUnitCostPrice(decimal costPrice, int totalUnit) 
        {
            return costPrice / totalUnit; 
        }

        public  decimal RoundToNearestHundred(decimal number)
        {
            return Math.Ceiling(number / 100) * 100;
        }


        //public  decimal RoundToNearestTen(decimal number)
        //{
        //    // Find the remainder when divided by 10
        //    decimal remainder = number % 10;

        //    // Determine if we should round up or down
        //    if (remainder >= 5)
        //    {
        //        return number - remainder + 10; // Round up
        //    }
        //    else
        //    {
        //        return number - remainder; // Round down
        //    }
        //}

    }
}
