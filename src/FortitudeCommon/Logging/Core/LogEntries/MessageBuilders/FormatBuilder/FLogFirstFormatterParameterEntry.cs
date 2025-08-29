// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry : FormatParameterEntry<IFLogFirstFormatterParameterEntry, FLogFirstFormatterParameterEntry>
  , IFLogFirstFormatterParameterEntry
{
    public IFLogAdditionalParamCollectionAppend WithParamsCollection
    {
        get
        {
            var addParamsBuilder =
                (Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>() ?? new FLogAdditionalFormatterParameterEntry())
                .Initialize(LogEntry, FormatBuilder, 0, OnComplete, FormatStsa!, FormatTokens);

            var addParamContinue = (Recycler?.Borrow<AdditionalParamCollectionAppend>() ?? new AdditionalParamCollectionAppend())
                .Initialize(addParamsBuilder, LogEntry);
            DecrementRefCount();
            return addParamContinue;
        }
    }

    public IFinalCollectionAppend WithOnlyParamCollection
    {
        get
        {
            NextPostAppendIsLast = true;

            var completeParamCollection =
                (Recycler?.Borrow<FinalFirstPParamCollectionAppend>() ?? new FinalFirstPParamCollectionAppend());

            completeParamCollection.Initialize(Me, LogEntry);

            return completeParamCollection;
        }
    }

    public IStringAppenderCollectionBuilder WithOnlyParamCollectionThenToAppender => LastParamToStringAppenderCollectionBuilder;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithMatchParams<T>(T value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        var toReturn = ToAdditionalFormatBuilder(value);
        tempStsa.DecrementRefCount();
        return toReturn;
    }

    public void WithOnlyMatchParam<T>(T? value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        ReplaceStagingTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    public IFLogStringAppender WithOnlyParamMatchThenToAppender<T>(T value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        var toReturn = ReplaceStagingTokenNumber(tempStsa.WriteBuffer).ToStringAppender(value, this);
        tempStsa.DecrementRefCount();
        return toReturn;
    }

    protected override FLogAdditionalFormatterParameterEntry ToAdditionalFormatBuilder()
    {
        var addParamsBuilder =
            (Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>() ?? new FLogAdditionalFormatterParameterEntry())
            .Initialize(LogEntry, FormatBuilder, 1, OnComplete, FormatStsa!, FormatTokens);
        FormatStsa = null;
        DecrementRefCount();
        return addParamsBuilder;
    }
}
