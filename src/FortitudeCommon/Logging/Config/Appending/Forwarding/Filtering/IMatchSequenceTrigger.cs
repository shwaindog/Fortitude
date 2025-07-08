// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;

public interface IMatchSequenceTrigger
{
    IMatchOperatorExpressionConfig? TriggeredWhenEntry { get; } // If triggered

    IExtractedMessageKeyValues OnTriggerExtract { get; } // next step is to extract values

    IMatchOperatorExpressionConfig? AbortWhenEntry { get; } // next check if sequence should abort

    IMatchOperatorExpressionConfig? CompletedWhenEntry { get; } // next check if this triggerred instance should complete

    IMatchSequenceTrigger? NextTriggerStep { get; } // extracted keys are available to chained 

    IMatchSequenceTrigger? SequenceFinalTrigger { get; }

    ITimeSpanConfig? TimeOut { get; } // On StartSequence this is time it is triggered.  For chained NextTriggerStep sequence
}
