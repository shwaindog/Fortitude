// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public interface IFLogAdditionalParamCollectionAppend : IKeyedCollectionContinuation<IFLogAdditionalFormatterParameterEntry>;

public class AdditionalParamCollectionAppend :
    KeyedCollectionContinuation<IFLogAdditionalFormatterParameterEntry, FLogAdditionalFormatterParameterEntry>
  , IFLogAdditionalParamCollectionAppend
{
    public new IFLogAdditionalParamCollectionAppend Initialize(FLogAdditionalFormatterParameterEntry caller, FLogEntry logEntry)
    {
        base.Initialize(caller, logEntry);

        CallingMsgBuilder = caller;

        return this;
    }
}

public class FinalAdditionalParamCollectionAppend : FinalAppenderCollectionBuilder<IFLogAdditionalFormatterParameterEntry,
    FLogAdditionalFormatterParameterEntry>;

public class FinalFirstPParamCollectionAppend : FinalAppenderCollectionBuilder<IFLogFirstFormatterParameterEntry, FLogFirstFormatterParameterEntry>;
