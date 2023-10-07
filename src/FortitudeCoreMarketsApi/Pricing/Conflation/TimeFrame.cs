namespace FortitudeMarketsApi.Pricing.Conflation
{
    public enum TimeFrame
    {
        Unknown = 0,
        Conflation = -2,
        Tick    = -1,
        OneSecond = 1,
        OneMinute = 60,
        FiveMinutes = 300,
        TenMinutes = 600,
        FifteenMinutes = 900,
        ThirtyMinutes = 1800,
        OneHour = 3600,
        FourHours = 14400,
        OneDay = 86400,
        OneWeek = 432000, // 5 full days in a week
        OneMonth = 1877040 // 4.345 weeks in a month
    }
}
