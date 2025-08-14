// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public partial class FLogFirstFormatterParameterEntry : FormatParameterEntry<FLogFirstFormatterParameterEntry>, IFLogFirstFormatterParameterEntry
{
    public FLogFirstFormatterParameterEntry() { }

    public FLogFirstFormatterParameterEntry(FLogFirstFormatterParameterEntry toClone) : base(toClone)
    {
    }
    
    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    public IFLogAdditionalFormatterParameterEntry? WithMatchParams<T>(T value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        var toReturn = ToAdditionalFormatBuilder(value);
        tempStsa.DecrementRefCount();
        return toReturn;
    }

    public void WithOnlyMatchParam<T>(T? value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        ReplaceTokenNumber(tempStsa.WriteBuffer);
        tempStsa.DecrementRefCount();
    }

    public IFLogStringAppender AfterOnlyParamMatchToStringAppender<T>(T value)
    {
        var tempStsa = TempStyledTypeAppender;
        AppendMatchSelect(value, tempStsa);
        var toReturn = ReplaceTokenNumber(tempStsa.WriteBuffer).ToStringAppender(value, this);
        tempStsa.DecrementRefCount();
        return toReturn;
    }
    
    public override FLogFirstFormatterParameterEntry Clone() =>
        Recycler?.Borrow<FLogFirstFormatterParameterEntry>().CopyFrom(this, CopyMergeFlags.FullReplace)
     ?? new FLogFirstFormatterParameterEntry(this);
}
