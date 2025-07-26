// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
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
    public static Action<AsyncProcessingType, IStyledTypeStringAppender> AsyncProcessingTypeFormatter
        = FormatAsyncProcessingTypeAppender;

    public static void FormatAsyncProcessingTypeAppender(this AsyncProcessingType asyncProcessingType, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.BackingStringBuilder;

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
}