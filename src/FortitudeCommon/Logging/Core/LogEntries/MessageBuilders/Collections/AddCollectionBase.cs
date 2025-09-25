// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public class AddCollectionBase<TToReturn, TCallerType> : FLogEntryMessageBuilderBase<TToReturn, AddCollectionBase<TToReturn, TCallerType>>, IMessageBuilderAppendChecks<TToReturn>
    where TCallerType : FLogEntryMessageBuilderBase<TToReturn, TCallerType>, TToReturn where TToReturn : class, IFLogMessageBuilder
{
    protected static readonly Action<IStringBuilder?> NoOpOnCompleteHandler = _ => { };

    protected TCallerType CallingMsgBuilder = null!;

    protected IMessageBuilderAppendChecks<TToReturn> MsgChecksBuilder => CallingMsgBuilder;

    public ITheOneString? ContinueOnReceivingStringAppender<T>(T param, [CallerMemberName] string memberName = "") =>
        MsgChecksBuilder.ContinueOnReceivingStringAppender(param, memberName);

    public TToReturn? PostAppendContinue<T>(ITheOneString? justAppended, T param, string memberName) =>
        PostAppendContinueOnMessageEntry(justAppended, param, memberName);

    protected override ITheOneString? PreappendCheckGetStringAppender<T>(T param, [CallerMemberName] string memberName = "") =>
        MsgChecksBuilder.ContinueOnReceivingStringAppender(param, memberName);

    protected override TToReturn? PostAppendContinueOnMessageEntry<T>(ITheOneString? justAppended, T param, string memberName = "")
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
    public static TToReturn PostAppendCheckAndReturn<TToReturn, TCaller, T>(this ITheOneString? justAppended, T param,
        AddCollectionBase<TToReturn, TCaller> postAppendedCollectionBuilder, [CallerMemberName] string memberName = "")
        where TCaller : FLogEntryMessageBuilderBase<TToReturn, TCaller>, TToReturn
        where TToReturn : class, IFLogMessageBuilder =>
        postAppendedCollectionBuilder.PostAppendContinue(justAppended, param, memberName)!;
}
