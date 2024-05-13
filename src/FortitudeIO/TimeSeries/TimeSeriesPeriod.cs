namespace FortitudeIO.TimeSeries;

[Flags]
public enum TimeSeriesPeriod : ushort
{
    None = 0
    , Tick = 0x00_01
    , OneSecond = 0x00_02
    , OneMinute = 0x00_04
    , FiveMinutes = 0x00_08
    , TenMinutes = 0x00_10
    , FifteenMinutes = 0x00_20
    , ThirtyMinutes = 0x00_40
    , OneHour = 0x00_80
    , FourHours = 0x01_00
    , OneDay = 0x02_00
    , OneWeek = 0x04_00
    , OneMonth = 0x08_00
    , OneYear = 0x10_00
    , ConsumerConflated = 0x80_00
}
