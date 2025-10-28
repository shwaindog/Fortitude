// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FileSystem;

public class LogEntryPathResolver : RecyclableObject, IDisposable
{
    private DateTimePathTokenValidator?         fileSystemDefaultDateFormatter;
    private CharSpanReturningLogEntryFormatter? formattedPathWriter;
    private FLogEntryFormatter?                 pathFormatter;

    public void Dispose()
    {
        DecrementRefCount();
    }

    public LogEntryPathResolver Initialize(IRecycler recycler, string combinedFullConfigFilePath)
    {
        fileSystemDefaultDateFormatter ??= new DateTimePathTokenValidator();

        formattedPathWriter = recycler.Borrow<CharSpanReturningLogEntryFormatter>().Initialize(1024);
        pathFormatter =
            recycler.Borrow<FLogEntryFormatter>()
                    .Initialize(combinedFullConfigFilePath, formattedPathWriter, fileSystemDefaultDateFormatter);

        return this;
    }

    public DateTimePartFlags GetPathDateTimePartFlags()
    {
        var flagsSoFar = DateTimePartFlags.None;
        for (var i = 0; i < pathFormatter!.TemplateParts.Count; i++)
        {
            var part = pathFormatter!.TemplateParts[i];
            if (part is ITokenReplacedTemplatePart logEntryTemplatePart)
                if (logEntryTemplatePart.TokenName.IsLogEntryDateTimeTokenName())
                {
                    var formatString = logEntryTemplatePart.FormatString;
                    flagsSoFar |= formatString.TimePartFlagsFromDateTimeFormatString();
                }
        }
        return flagsSoFar;
    }

    public ReadOnlySpan<char> ResolvePathFor(IFLogEntry entry)
    {
        formattedPathWriter!.Clear();
        pathFormatter!.ApplyFormatting(entry);
        return formattedPathWriter!.AsSpan;
    }

    public override void StateReset()
    {
        formattedPathWriter?.DecrementRefCount();
        formattedPathWriter = null;

        pathFormatter?.DecrementRefCount();
        pathFormatter = null;


        base.StateReset();
    }

    public class DateTimePathTokenValidator : ITokenFormattingValidator
    {
        public const string DefaultDateFormatting = "YYYY-MM-DD";
        public const string DefaultTimeFormatting = "HH";

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
                    if (tokenValue.Length == 0) return DefaultDateFormatting.AsSpan();
                    break;
                case $"{nameof(FLogEntryLayoutTokens.TO)}":
                case $"{nameof(FLogEntryLayoutTokens.TIMEONLY)}":
                    if (tokenValue.Length == 0) return DefaultTimeFormatting.AsSpan();
                    break;
                case $"{nameof(FLogEntryLayoutTokens.DATETIME_MICROSECONDS)}":
                case $"{nameof(FLogEntryLayoutTokens.DATE_MICROS)}":
                case $"{nameof(FLogEntryLayoutTokens.TIME_MICROS)}":
                case $"{nameof(FLogEntryLayoutTokens.TS_US)}":
                    if (tokenValue.Length == 0) return DefaultTimeMicrosFormatting.AsSpan();
                    break;
            }
            return tokenValue;
        }
    }
}
