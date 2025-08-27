// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogAdditionalFormatterParameterEntry : FormatParameterEntry<IFLogAdditionalFormatterParameterEntry, FLogAdditionalFormatterParameterEntry>
  , IFLogAdditionalFormatterParameterEntry
{
    public FLogAdditionalFormatterParameterEntry() { }

    public FLogAdditionalFormatterParameterEntry(FLogAdditionalFormatterParameterEntry toClone) : base(toClone) { }

    public IFLogAdditionalParamCollectionAppend AndCollection
    {
        get
        {
            var addParamContinue = (Recycler?.Borrow<AdditionalParamCollectionAppend>() ?? new AdditionalParamCollectionAppend())
                .Initialize(this, LogEntry);
            return addParamContinue;
        }
    }
    
    public IFinalCollectionAppend AndFinalCollectionParam
    {
        get
        {
            NextPostAppendIsLast = true;

            var completeParamCollection =
                (Recycler?.Borrow<FinalAppenderCollectionBuilder<IFLogAdditionalFormatterParameterEntry, FLogAdditionalFormatterParameterEntry>>() ??
                 new FinalAppenderCollectionBuilder<IFLogAdditionalFormatterParameterEntry, FLogAdditionalFormatterParameterEntry>());
            
            completeParamCollection.Initialize(Me, LogEntry);
            
            return completeParamCollection;
        }
    }
    
    public IStringAppenderCollectionBuilder AndFinalCollectionParamThenToAppender => LastParamToStringAppenderCollectionBuilder;
    
    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndMatch<T>(T value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        var toReturn = ReplaceTokenNumber(tempStsa.WriteBuffer).CallExpectContinue(value);
        tempStsa.DecrementRefCount();
        return toReturn;
    }

    public void AndFinalMatchParam<T>(T value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        ReplaceTokenNumber(tempStsa.WriteBuffer).CallEnsureNoMoreTokensAndComplete(value);
        tempStsa.DecrementRefCount();
    }

    public IFLogStringAppender AndFinalMatchParamThenToAppender<T>(T? value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        var toReturn = ReplaceTokenNumber(tempStsa.WriteBuffer).ToStringAppender(value, this);
        tempStsa.DecrementRefCount();
        return toReturn;
    }
    
    protected override FLogAdditionalFormatterParameterEntry ToAdditionalFormatBuilder() => this;
}
