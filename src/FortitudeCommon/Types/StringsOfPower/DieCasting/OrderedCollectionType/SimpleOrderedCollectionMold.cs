// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public class SimpleOrderedCollectionMold : OrderedCollectionMold<SimpleOrderedCollectionMold>
{
    public SimpleOrderedCollectionMold InitializeSimpleOrderedCollectionBuilder(object instanceOrContainer
      , Type typeBeingBuilt
      , ISecretStringOfPower master
      , Type typeVisitedAs
      , string? typeName
      , int remainingGraphDepth
      , VisitResult moldGraphVisit
      , WrittenAsFlags writeMethodType
      , CallerContext callerContext
      , CreateContext createContext)
    {
        InitializeOrderedCollectionBuilder
            (instanceOrContainer, typeBeingBuilt, master, typeVisitedAs, typeName
           , remainingGraphDepth, moldGraphVisit, writeMethodType, callerContext
           , createContext with { FormatFlags = createContext.FormatFlags | FormatFlags.AsCollection });

        return this;
    }

    protected override void SourceBuilderComponentAccess(WrittenAsFlags writeMethod)
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.Master.Recycler;
        MoldStateField =
            recycler
                .Borrow<CollectionMoldWriteState<SimpleOrderedCollectionMold>>()
                .InitializeOrderCollectionComponentAccess(this, PortableState, writeMethod);
    }
}
