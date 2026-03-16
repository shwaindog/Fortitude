// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public interface ICollectionMoldWriteState : IMoldWriteState
{
    bool IsSimple { get; }
}

public class CollectionMoldWriteState<TOCMold> : MoldWriteState<TOCMold>, ICollectionMoldWriteState where TOCMold : TypeMolder
{

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
    
    public TrackedInstanceMold? TrackInnerCollectionValueMold<TCollection>(TCollection collection, Type elementType, FormatFlags flags)
    {
        var collType = collection?.GetType() ?? typeof(TCollection);
        
        var buildTypeSameAsCollectionType = Mold.BuildingInstanceEquals(collection);
        if (buildTypeSameAsCollectionType || collType == TypeBeingBuilt)
        {
            InnerSameAsOuterType =  true;
            
            flags |= LogSuppressTypeNames | NoRevisitCheck;
        }
        // var collTypeFullName = collType.FullName ?? "";
        // var elementTypeFullName = elementType.IfNullableGetUnderlyingTypeOrThis().FullName ?? "";
        // if(Settings.LogSuppressDisplayCollectionNames.Any(s => collTypeFullName.StartsWith(s))
        // && Settings.LogSuppressDisplayCollectionElementNames.Any(s => elementTypeFullName.StartsWith(s)))
        // {
        //     flags |= LogSuppressTypeNames;
        // }

        if (collection != null && !buildTypeSameAsCollectionType && !collType.IsValueType)
        {
            return Master.GetTrackedInstanceMold
                (collection, flags.RemoveEmbeddedContentFlags(), AsSimple | WrittenAsFlags.AsCollection
                 , CreateMoldFormatFlags.RemoveInstanceTrackingFlags());
        }
        
        return null;
    }

    public TrackedInstanceMold? ConditionalCollectionPrefix<TCollection>(TCollection collection, Type elementType, bool? hasAny, FormatFlags formatFlags)
    {
        formatFlags = formatFlags.RemoveEmbeddedContentFlags();
        TrackedInstanceMold? valueMold              = null;
        
        if (collection is not null and not Type)
        {
            var actualType = collection.GetType();
            if (!actualType.IsValueType) { valueMold = TrackInnerCollectionValueMold(collection, elementType, formatFlags); }
            if(valueMold == null)
            {
                var collType = collection.GetType(); 
                var buildTypeSameAsCollectionType = Mold.BuildingInstanceEquals(collection);
                
                if (collType == TypeBeingBuilt)
                {
                    InnerSameAsOuterType =  true;
            
                    formatFlags |= LogSuppressTypeNames | NoRevisitCheck;
                }
                if (!buildTypeSameAsCollectionType)
                {
                    var shouldShowInnerCollectionName = Settings.ShouldDisplayCollectionTypeName(actualType);
                    if (shouldShowInnerCollectionName)
                    {
                        var previousWroteOpen     = WroteTypeOpen;
                        var previousWroteTypeName = WroteTypeName;
                        WroteTypeOpen = false;
                        WroteTypeName = false;
                        Sf.StartSimpleTypeOpening(collection, this, AsSimple | WrittenAsFlags.AsCollection, formatFlags);
                        Sf.FinishSimpleTypeOpening(collection, this, AsSimple | WrittenAsFlags.AsCollection, formatFlags);
                        WroteTypeOpen = previousWroteOpen;
                        WroteTypeName = previousWroteTypeName;
                    }
                }
            }
        } else if (collection is Type collType)
        {
            var buildTypeSameAsCollectionType = false;    
            if (collType == TypeBeingBuilt)
            {
                InnerSameAsOuterType =  true;

                buildTypeSameAsCollectionType =  true;
                formatFlags                   |= LogSuppressTypeNames | NoRevisitCheck;
            }
            if(!buildTypeSameAsCollectionType)
            {
                var shouldShowInnerCollectionName = Settings.ShouldDisplayCollectionTypeName(collType);
                if (shouldShowInnerCollectionName)
                {
                    var previousWroteOpen     = WroteTypeOpen;
                    var previousWroteTypeName = WroteTypeName;
                    WroteTypeOpen = false;
                    WroteTypeName = false;
                    Sf.StartSimpleTypeOpening(collection, this, AsSimple | WrittenAsFlags.AsCollection, formatFlags);
                    Sf.FinishSimpleTypeOpening(collection, this, AsSimple | WrittenAsFlags.AsCollection, formatFlags);
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
    
}
