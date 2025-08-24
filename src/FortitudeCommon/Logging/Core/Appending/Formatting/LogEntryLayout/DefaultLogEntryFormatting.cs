using FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public class DefaultLogEntryFormatting : ITokenFormattingValidator
{
    public const string DefaultDateFormatting = "YYYY-MM-DD HH:mm:ss.ffffff";
    public const string DefaultTimeFormatting = "HH:mm:ss";

    public const string DefaultTimeMicrosFormatting = "000";

    public ReadOnlySpan<char> ValidateFormattingToken(string tokenName, ReadOnlySpan<char> tokenValue)
    {
        switch (tokenName)
        {
            case $"{nameof(FLogEntryLayoutTokens.TS)}":
            case $"{nameof(FLogEntryLayoutTokens.DATE)}":
            case $"{nameof(FLogEntryLayoutTokens.TIME)}":
            case $"{nameof(FLogEntryLayoutTokens.DATETIME)}":
            case $"{nameof(FLogEntryLayoutTokens.DATEONLY)}":
            case $"{nameof(FLogEntryLayoutTokens.DO)}":
                if (tokenValue.Length == 0)
                {
                    return DefaultDateFormatting.AsSpan();
                }
                break;
            case $"{nameof(FLogEntryLayoutTokens.TO)}":
            case $"{nameof(FLogEntryLayoutTokens.TIMEONLY)}":
                if (tokenValue.Length == 0)
                {
                    return DefaultTimeFormatting.AsSpan();
                }
                break;
            case $"{nameof(FLogEntryLayoutTokens.DATETIME_MICROSECONDS)}":
            case $"{nameof(FLogEntryLayoutTokens.DATE_MICROS)}":
            case $"{nameof(FLogEntryLayoutTokens.TIME_MICROS)}":
            case $"{nameof(FLogEntryLayoutTokens.TS_US)}":
                if (tokenValue.Length == 0)
                {
                    return DefaultTimeMicrosFormatting.AsSpan();
                }
                break;
        }
        return tokenValue;
    }
}
