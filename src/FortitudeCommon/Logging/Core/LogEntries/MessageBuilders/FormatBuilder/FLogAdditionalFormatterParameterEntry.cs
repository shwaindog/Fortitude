// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogAdditionalFormatterParameterEntry :
    FormatParameterEntry<IFLogAdditionalFormatterParameterEntry, FLogAdditionalFormatterParameterEntry>
  , IFLogAdditionalFormatterParameterEntry
{
    public IFLogAdditionalParamCollectionAppend AndCollection
    {
        [MustUseReturnValue("Use Add* to add a collection to the format parameters")]
        get
        {
            var addParamContinue = (Recycler?.Borrow<AdditionalParamCollectionAppend>() ?? new AdditionalParamCollectionAppend())
                .Initialize(this, LogEntry);
            return addParamContinue;
        }
    }

    public IFinalCollectionAppend AndFinalParamCollection
    {
        [MustUseReturnValue("Use Add* to add a collection to the format parameters")]
        get
        {
            NextPostAppendIsLast = true;

            var completeParamCollection =
                Recycler?.Borrow<FinalAdditionalParamCollectionAppend>() ?? new FinalAdditionalParamCollectionAppend();

            completeParamCollection.Initialize(Me, LogEntry);

            return completeParamCollection;
        }
    }

    public IStringAppenderCollectionBuilder AndFinalParamCollectionThenToAppender => LastParamToStringAppenderCollectionBuilder;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndMatch<T>(T value)
    {
        var tempStsa = Temp;
        AppendMatchSelect(value, tempStsa);
        var toReturn = ReplaceStagingTokenNumber(tempStsa.WriteBuffer).CallExpectContinue(value);
        tempStsa.DecrementRefCount();
        return toReturn;
    }

    public void AndFinalMatchParam<T>(T value)
    {
        var tempStsa = Temp;
        AppendMatchSelect(value, tempStsa);
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer).CallEnsureNoMoreTokensAndComplete(value);
        tempStsa.DecrementRefCount();
    }

    public IFLogStringAppender AndFinalMatchParamThenToAppender<T>(T? value)
    {
        var tempStsa = Temp;
        AppendMatchSelect(value, tempStsa);
        var toReturn = ReplaceStagingTokenNumber(tempStsa.WriteBuffer).ToStringAppender(value, this);
        tempStsa.DecrementRefCount();
        return toReturn;
    }

    protected override FLogAdditionalFormatterParameterEntry ToAdditionalFormatBuilder() => this;
}
