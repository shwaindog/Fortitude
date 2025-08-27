namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public interface IFinalCollectionAppend : IKeyedCollectionFinalAppend;

public class FinalAppenderCollectionBuilder<TIMsgBuilder, TMsgBuilderImpl> : 
    KeyedCollectionFinalAppend<TIMsgBuilder, TMsgBuilderImpl>, IFinalCollectionAppend 
    where TMsgBuilderImpl : FLogEntryMessageBuilderBase<TIMsgBuilder, TMsgBuilderImpl>, TIMsgBuilder where TIMsgBuilder : class, IFLogMessageBuilder;