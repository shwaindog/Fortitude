// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public class AddCollectionBase<TToReturn, TCallerType> : FLogEntryMessageBuilderBase<TToReturn, AddCollectionBase<TToReturn, TCallerType>>, IMessageBuilderAppendChecks<TToReturn>
    where TCallerType : FLogEntryMessageBuilderBase<TToReturn, TCallerType>, TToReturn where TToReturn : class, IFLogMessageBuilder
{
    protected static readonly Action<IStringBuilder?> NoOpOnCompleteHandler = _ => { };

    protected TCallerType CallingMsgBuilder = null!;

    protected IMessageBuilderAppendChecks<TToReturn> MsgChecksBuilder => CallingMsgBuilder;

    public IStyledTypeStringAppender? ContinueOnReceivingStringAppender<T>(T param, [CallerMemberName] string memberName = "") =>
        MsgChecksBuilder.ContinueOnReceivingStringAppender(param, memberName);

    public TToReturn? PostAppendContinue<T>(IStyledTypeStringAppender? justAppended, T param, string memberName) =>
        PostAppendContinueOnMessageEntry(justAppended, param, memberName);

    protected override IStyledTypeStringAppender? PreappendCheckGetStringAppender<T>(T param, [CallerMemberName] string memberName = "") =>
        MsgChecksBuilder.ContinueOnReceivingStringAppender(param, memberName);

    protected override TToReturn? PostAppendContinueOnMessageEntry<T>(IStyledTypeStringAppender? justAppended, T param, string memberName = "")
    {
        var toReturn = MsgChecksBuilder.PostAppendContinue(justAppended, param, memberName);
        DecrementRefCount();
        return toReturn!;
    }

    public void Initialize(TCallerType caller, FLogEntry logEntry)
    {
        base.Initialize(logEntry, NoOpOnCompleteHandler);

        CallingMsgBuilder = caller;
    }
}

public static class FLogAddCollectionBaseExtensions
{
    public static TToReturn PostAppendCheckAndReturn<TToReturn, TCaller, T>(this IStyledTypeStringAppender? justAppended, T param,
        AddCollectionBase<TToReturn, TCaller> postAppendedCollectionBuilder, [CallerMemberName] string memberName = "")
        where TCaller : FLogEntryMessageBuilderBase<TToReturn, TCaller>, TToReturn
        where TToReturn : class, IFLogMessageBuilder =>
        postAppendedCollectionBuilder.PostAppendContinue(justAppended, param, memberName)!;
}
