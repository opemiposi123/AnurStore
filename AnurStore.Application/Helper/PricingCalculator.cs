namespace AnurStore.Application.Helper;

public static class PricingCalculator
{
    public static decimal CalculatePackSellingPrice(decimal rate, decimal markupPercentage)
    {
        var price = rate + (rate * markupPercentage / 100);
        return RoundToNearestHundred(price);
    }

    public static decimal CalculateUnitSellingPrice(decimal packPrice, int totalUnits)
    {
        if (totalUnits <= 0) throw new ArgumentException("Total units must be greater than zero");
        var unitPrice = packPrice / totalUnits;
        return RoundToNearestTen(unitPrice);
    }

    public static decimal RoundToNearestHundred(decimal number)
    {
        return Math.Round(number / 100, MidpointRounding.AwayFromZero) * 100;
    }

    public static decimal RoundToNearestTen(decimal number)
    {
        decimal remainder = number % 10;
        return (remainder >= 5) ? number - remainder + 10 : number - remainder;
    }
}
