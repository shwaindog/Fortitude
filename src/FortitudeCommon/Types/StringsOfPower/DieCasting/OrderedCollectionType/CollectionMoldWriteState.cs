// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public interface ICollectionMoldWriteState : IMoldWriteState, IHasCollectionItems
{
    bool IsSimple { get; }

    TrackedInstanceMold? ConditionalCollectionPrefix<TCollection>(TCollection collection, Type elementType, bool? hasAny, CreateContext createContext);

    void ConditionalCollectionSuffix(TrackedInstanceMold? trackedInstanceMold, Type elementType, int? matchCount
      , int? countCollectionAvailable, string? formatString, FormatFlags formatFlags);
    
    Type? DisplayAsType { get; set; }
    
    new int ItemCount { get; set; }
    
    public bool BeforeFirstElement(Type elementType);
}

public class CollectionMoldWriteState<TOCMold> : MoldWriteState<TOCMold>, ICollectionMoldWriteState where TOCMold : TypeMolder
{
    public string? BeforeFirstItemFieldName { get; set; }

    public CollectionMoldWriteState<TOCMold> InitializeOrderCollectionComponentAccess
        (TOCMold externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WrittenAsFlags writeMethod)
    {
        var createFmtFlags = typeBuilderPortableState.CreateFormatFlags;
        var style = typeBuilderPortableState.Master.Settings.Style;
        writeMethod = typeBuilderPortableState.MoldGraphVisit.IsARevisit 
                   || createFmtFlags.DoesNotHaveContentAllowComplexType() || (writeMethod.SupportsMultipleFields() && style.IsLog())
            ? writeMethod
            : writeMethod.ToNoFieldEquivalent();
        
        Initialize(externalTypeBuilder, typeBuilderPortableState, writeMethod);
        
        return this;
    }

    public virtual bool IsSimple => true;

    public int ItemCount { get; set; }

    public Type? DisplayAsType { get; set; }
    
    public TrackedInstanceMold? TrackInnerCollectionValueMold<TCollection>(TCollection collection, Type elementType, CreateContext createContext)
    {
        var flags    = createContext.FormatFlags;
        var collType = collection?.GetType() ?? typeof(TCollection);
        
        var buildTypeSameAsCollectionType = Mold.BuildingInstanceEquals(collection);
        if (InnerSameAsOuterType ||  buildTypeSameAsCollectionType || collType == TypeBeingBuilt)
        {
            InnerSameAsOuterType =  true;
            
            flags |= LogSuppressTypeNames | NoRevisitCheck;
        }

        if (collection != null && !buildTypeSameAsCollectionType && !collType.IsValueType)
        {
            createContext = createContext with { FormatFlags = createContext.FormatFlags.RemoveInstanceTrackingFlags() };
            return Master.GetTrackedInstanceMold
                (collection, flags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                 , createContext);
        }
        
        return null;
    }

    public TrackedInstanceMold? ConditionalCollectionPrefix<TCollection>(TCollection collection, Type elementType, bool? hasAny, CreateContext createContext)
    {
        if (ItemCount == 0)
        {
            if (BeforeFirstItemFieldName != null && hasAny != true)
            {
                SkipBody = true;
                return null;
            }
            BeforeFirstElement(elementType);
            if (SkipBody) return null;
        }
        createContext = createContext with { FormatFlags = createContext.FormatFlags.RemoveEmbeddedContentFlags() };
        var formatFlags = createContext.FormatFlags;
        TrackedInstanceMold? valueMold   = null;
        
        var displayType = createContext.DisplayAsType ?? GetDisplayType(collection);
        // if (hasAny == true && collection is not null and not Type)
        if (collection is not null and not Type)
        {
            if (!displayType.IsValueType)
            {
                valueMold     = TrackInnerCollectionValueMold(collection, elementType, createContext);
            }
            if(valueMold == null)
            {
                if (InnerSameAsOuterType || displayType == TypeBeingBuilt || Mold.BuildingInstanceEquals(collection))
                {
                    InnerSameAsOuterType =  true;
            
                    formatFlags |= LogSuppressTypeNames | NoRevisitCheck;
                }
                if (!InnerSameAsOuterType)
                {
                    var shouldShowInnerCollectionName = Settings.ShouldDisplayCollectionTypeName(displayType);
                    if (shouldShowInnerCollectionName)
                    {
                        var previousWroteOpen     = WroteTypeOpen;
                        var previousWroteTypeName = WroteTypeName;
                        WroteTypeOpen = false;
                        WroteTypeName = false;
                        Sf.StartSimpleTypeOpening(displayType, this, AsSimple | WrittenAsFlags.AsCollection, formatFlags);
                        Sf.FinishSimpleTypeOpening(displayType, this, AsSimple | WrittenAsFlags.AsCollection, formatFlags);
                        WroteTypeOpen = previousWroteOpen;
                        WroteTypeName = previousWroteTypeName;
                    }
                }
            }
        } 
        // else if (hasAny == true && collection is Type)
        else if (collection is Type)
        {    
            if (InnerSameAsOuterType || displayType == TypeBeingBuilt)
            {
                InnerSameAsOuterType =  true;

                formatFlags                   |= LogSuppressTypeNames | NoRevisitCheck;
            }
            if(!InnerSameAsOuterType)
            {
                var shouldShowInnerCollectionName = Settings.ShouldDisplayCollectionTypeName(displayType);
                if (shouldShowInnerCollectionName)
                {
                    var previousWroteOpen     = WroteTypeOpen;
                    var previousWroteTypeName = WroteTypeName;
                    WroteTypeOpen = false;
                    WroteTypeName = false;
                    Sf.StartSimpleTypeOpening(displayType, this, AsSimple | WrittenAsFlags.AsCollection, formatFlags);
                    Sf.FinishSimpleTypeOpening(displayType, this, AsSimple | WrittenAsFlags.AsCollection, formatFlags);
                    WroteTypeOpen = previousWroteOpen;
                    WroteTypeName = previousWroteTypeName;
                }
            }
        }
        // always open and close here
        if (valueMold == null || valueMold.ShouldSuppressBody == false)
        {
            StyleFormatter.AppendOpenCollection(this, elementType, hasAny, LogSuppressTypeNames);
        }
        return valueMold;
    }

    public void ConditionalCollectionSuffix(TrackedInstanceMold? trackedInstanceMold, Type elementType, int? matchCount
      , int? countCollectionAvailable, string? formatString, FormatFlags formatFlags)
    {
        formatFlags = formatFlags.RemoveEmbeddedContentFlags();
        if (Mold is OrderedCollectionMold<TOCMold> ocMold)
        {
            ocMold.ResultCount = matchCount ?? 0;
        }
        if (trackedInstanceMold == null || trackedInstanceMold.ShouldSuppressBody == false)
        {
            StyleFormatter.AppendCloseCollection(this, matchCount ?? 0, elementType, countCollectionAvailable ?? 0, formatString, formatFlags);
        }
        if (trackedInstanceMold != null)
        {
            trackedInstanceMold.Complete();
        }
    }

    public virtual bool BeforeFirstElement(Type collectionElement)
    {
        if (BeforeFirstItemFieldName != null)
        {
            this.FieldNameJoin(BeforeFirstItemFieldName);
            Mold.StartTypeOpening(CreateMoldFormatFlags.RemoveEmbeddedContentFlags());
            Mold.FinishTypeOpening(CreateMoldFormatFlags.RemoveEmbeddedContentFlags());
            var shouldContinue = Mold.ShouldShowBody;
            if (shouldContinue)
            {
                StyleFormatter.AppendOpenCollection(this, collectionElement, true );
                BeforeFirstItemFieldName = null;
            }
            return !shouldContinue;
        }
        return false;
    }

    public override void StateReset()
    {
        ItemCount                = 0;
        BeforeFirstItemFieldName = null;
        DisplayAsType            = null;
        base.StateReset();
    }
}
