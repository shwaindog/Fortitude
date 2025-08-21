// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using static FortitudeCommon.Logging.Config.Initialization.AsyncQueues.AsyncProcessingType;

namespace FortitudeCommon.Logging.Config.Initialization.AsyncQueues;

public enum AsyncProcessingType
{
    Default
  , AllAsyncDisabled
  , Synchronise
  , SingleBackgroundAsyncThread
  , ConfigDefinedAsyncThreads
  , AsyncUsesThreadPool
  , ConfigEventBusQueues
}

public static class AsyncProcessingTypesExtensions
{
    public static CustomTypeStyler<AsyncProcessingType> AsyncProcessingTypeFormatter
        = FormatAsyncProcessingTypeAppender;

    public static StyledTypeBuildResult FormatAsyncProcessingTypeAppender(this AsyncProcessingType asyncProcessingType, IStyledTypeStringAppender sbc)
    {
        var tb = sbc.StartSimpleValueType(nameof(AsyncProcessingType));
        using (var sb = tb.StartDelimitedStringBuilder())
        {
            switch (asyncProcessingType)
            {
                case Default: sb.Append($"{nameof(Default)}"); break;

                case Synchronise:                 sb.Append($"{nameof(Synchronise)}"); break;
                case SingleBackgroundAsyncThread: sb.Append($"{nameof(SingleBackgroundAsyncThread)}"); break;
                case ConfigDefinedAsyncThreads:   sb.Append($"{nameof(ConfigDefinedAsyncThreads)}"); break;
                case AsyncUsesThreadPool:         sb.Append($"{nameof(AsyncUsesThreadPool)}"); break;
                case ConfigEventBusQueues:        sb.Append($"{nameof(ConfigEventBusQueues)}"); break;

                default: sb.Append($"{nameof(Default)}"); break;
            }
        }

        return tb.Complete();
    }
}