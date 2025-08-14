// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogAdditionalFormatterParameterEntry : FormatParameterEntry<FLogAdditionalFormatterParameterEntry>
  , IFLogAdditionalFormatterParameterEntry
{
    public FLogAdditionalFormatterParameterEntry() { }

    public FLogAdditionalFormatterParameterEntry(FLogAdditionalFormatterParameterEntry toClone) : base(toClone) { }


    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndMatch<T>(T value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        var toReturn = ReplaceTokenNumber(tempStsa.WriteBuffer).ExpectContinue(value);
        tempStsa.DecrementRefCount();
        return toReturn;
    }

    public void AndFinalMatchParam<T>(T value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        ReplaceTokenNumber(tempStsa.WriteBuffer).EnsureNoMoreTokensAndComplete(value);
        tempStsa.DecrementRefCount();
    }

    public IFLogStringAppender AfterFinalMatchParamToStringAppender<T>(T? value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        var toReturn = ReplaceTokenNumber(tempStsa.WriteBuffer).ToStringAppender(value, this);
        tempStsa.DecrementRefCount();
        return toReturn;
    }

    public override FLogAdditionalFormatterParameterEntry Clone() =>
        Recycler?.Borrow<FLogAdditionalFormatterParameterEntry>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new FLogAdditionalFormatterParameterEntry(this);
}
