using System.Globalization;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeTests.FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public static class SingleMessageBuildingValues
{
    public const int LargeObjectHeapSize           = 85_000;
    public const int MaxNonLargeObjectHeapCharSize = LargeObjectHeapSize / 2 - 4096; //  UTF16 english chars - two bytes 

    public const string ShortString = "The quick brown fox jumps over the lazy dog.";

    public const string ShortStringSubRangeAsString = "quick brown";

    public const string LargeStringSubRangeAsString =
        "The quick brown fox jumps over the lazy dog.The quick brown fox jumps over the lazy dog.The quick brown fox jumps over the lazy " +
        "dog.The quick brown fox jumps over the lazy dog.The quick brown fox jumps over the lazy dog.The quick brown fox jumps over the lazy dog.";

    public static readonly Range ShortStringSubRange = new(Index.FromStart(4), Index.FromStart(15));
    public static readonly Range LargeStringSubRange = new(Index.FromStart(0), Index.FromStart(264));

    public static string LargeString;

    public static readonly TimeSpan ToFormatTimeSpan = TimeSpan.FromTicks(42_242_424_2);

    public const string TimeSpanStylerComplexAsString = "TimeSpan {Seconds: 42, Milliseconds: 242, Microseconds: 424 }";
    public const string TimeSpanSpanFormatAsString    = "TimeSpan.42.242424";
    
    public static readonly string TimeSpanSpanUnformattedAsString;

    public static readonly DateTime ToFormatDateTime = new(2025, 8, 25, 10, 41, 32);
    public static readonly DateTime ToFormatDateTimeTicks;

    public static readonly char[] SmallCharsArray        = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
    public static readonly Range  SmallCharArraySubRange = new(Index.FromStart(8), Index.FromStart(13));

    public static readonly char[] LargeCharsArray        = new char[Math.Min(SmallCharsArray.Length * 1000, MaxNonLargeObjectHeapCharSize)];
    public static readonly Range  LargeCharArraySubRange = new(Index.FromStart(0), Index.FromStart(257));
    public static readonly string LargeCharsArrayAsString;

    public const double ToFormatNumber = Math.PI;
    
    public static readonly string ToFormatNumberUnformmatedString = ToFormatNumber.ToString(CultureInfo.CurrentCulture);

    public const string Number2DecimalPlaceString = "3.14";
    public const string Number3DecimalPlaceString = "3.142";
    public const string Number4DecimalPlaceString = "3.1416";
    public const string Number5DecimalPlaceString = "3.14159";
    public const string Number6DecimalPlaceString = "3.141593";

    public const string Number2DecimalPlacesLeftAlignString = "3.14          ";
    public const string Number3DecimalPlacesLeftAlignString = "3.142         ";
    public const string Number4DecimalPlacesLeftAlignString = "3.1416        ";
    public const string Number5DecimalPlacesLeftAlignString = "3.14159       ";
    public const string Number6DecimalPlacesLeftAlignString = "3.141593      ";

    public const string Number2DecimalPlacesRightAlignString = "          3.14";
    public const string Number3DecimalPlacesRightAlignString = "         3.142";
    public const string Number4DecimalPlacesRightAlignString = "        3.1416";
    public const string Number5DecimalPlacesRightAlignString = "       3.14159";
    public const string Number6DecimalPlacesRightAlignString = "      3.141593";

    public const string SmallCharArrayAsString         = "0123456789ABCDEF";
    public const string SmallCharArraySubRangeAsString = "89ABC";
    public const string LargeCharArraySubRangeAsString =
        "0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF" +
        "0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0";

    public static readonly StringBuilder SmallStringBuilder = new(ShortString);
    public static readonly StringBuilder LargeStringBuilder = new(MaxNonLargeObjectHeapCharSize + ShortString.Length);

    public static readonly MutableString SmallMutableString = new(ShortString);
    public static readonly MutableString LargeMutableString = new(MaxNonLargeObjectHeapCharSize + ShortString.Length);

    public static readonly CharArrayStringBuilder SmallCharArrayStringBuilder = new(ShortString);
    public static readonly CharArrayStringBuilder LargeCharArrayStringBuilder =
        new((MaxNonLargeObjectHeapCharSize + ShortString.Length).NextPowerOfTwo());

    public const string DateTimeStandardFormatString = "{0:yyyy-MM-dd HH:mm:ss.ffffff}";

    public static string DateStandardFormatString   = "{0:yyyy-MM-dd}";
    public static string TimeLeftAlignFormatString  = "{0,-14:HH:mm:ss}";
    public static string TimeRightAlignFormatString = "{0,14:yyyy-MM-dd}";

    public static readonly string ToFormatDateTimeTicksUnformattedAsString;
    public static readonly string ToFormatDateunformattedAsString;

    public const string ToFormatDateTimeTicksStandardAsString = "2025-08-25 10:42:14.242424";
    public const string ToFormatDateStandardAsString          = "2025-08-25";
    public const string ToFormatTimeLeftAlignAsString         = "10:41:32      ";
    public const string ToFormatTimeRightAlignAsString        = "      10:41:32";

    static SingleMessageBuildingValues()
    {
        for (var i = 0; i < 1000; i++)
        {
            if (LargeStringBuilder.Length < MaxNonLargeObjectHeapCharSize)
            {
                LargeStringBuilder.Append(ShortString);
                LargeMutableString.Append(ShortString);
                LargeCharArrayStringBuilder.Append(ShortString);
            }
            var offset = i * SmallCharsArray.Length;
            for (var j = 0; j < SmallCharsArray.Length && j + offset < LargeCharsArray.Length; j++)
            {
                LargeCharsArray[j + offset] = SmallCharsArray[j];
            }
        }
        LargeString           = LargeStringBuilder.ToString();
        ToFormatDateTimeTicks = ToFormatDateTime + ToFormatTimeSpan;

        ToFormatDateTimeTicksUnformattedAsString = ToFormatDateTimeTicks.ToString(CultureInfo.CurrentCulture);
        ToFormatDateunformattedAsString          = ToFormatDateTime.ToString(CultureInfo.CurrentCulture);
        TimeSpanSpanUnformattedAsString          = ToFormatTimeSpan.ToString();
        
        LargeCharsArrayAsString = new String(LargeCharsArray);
    }
}
