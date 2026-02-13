// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public interface ICollectionMolderDieCast : ITypeMolderDieCast
{
    bool IsSimple { get; }
}

public class CollectionBuilderCompAccess<TOCMold> : TypeMolderDieCast<TOCMold>, ICollectionMolderDieCast where TOCMold : TypeMolder
{

    public CollectionBuilderCompAccess<TOCMold> InitializeOrderCollectionComponentAccess
        (TOCMold externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WrittenAsFlags writeMethod, bool isSimple)
    {
        writeMethod = typeBuilderPortableState.MoldGraphVisit.IsARevisit 
                   || writeMethod.SupportsMultipleFields() && typeBuilderPortableState.Master.Style.IsLog()
                      ? writeMethod
                      : writeMethod.ToNoFieldEquivalent();
        
        Initialize(externalTypeBuilder, typeBuilderPortableState, writeMethod);
        IsSimple = isSimple;
        
        return this;
    }

    public bool IsSimple { get; private set; }

    public void ConditionalCollectionPrefix(Type elementType, bool? hasAny)
    {
        // if (SupportsMultipleFields)
        // {
        //     StyleFormatter.AppendInstanceValuesFieldName( TypeBeingBuilt, CreateMoldFormatFlags);
        //     if (hasAny == true)
        //     {
        //         StyleFormatter.StartFormatCollectionOpen(this, elementType, hasAny, TypeBeingBuilt, FormatFlags.LogSuppressTypeNames);
        //         StyleFormatter.FinishFormatCollectionOpen(this, elementType, hasAny, TypeBeingBuilt, FormatFlags.LogSuppressTypeNames);
        //     }
        // }
        
        // always open and close here
        StyleFormatter.StartFormatCollectionOpen(this, elementType, true, TypeBeingBuilt, FormatFlags.LogSuppressTypeNames);
        StyleFormatter.FinishFormatCollectionOpen(this, elementType, true, TypeBeingBuilt, FormatFlags.LogSuppressTypeNames);
    }

    // public bool ConditionalCollectionSuffix(Type elementType, int? count, string? formatString, FormatFlags formatFlags)
    public void ConditionalCollectionSuffix(Type elementType, int? matchCount, int? countCollectionAvailable, string? formatString
      , FormatFlags formatFlags)
    {
        if (Mold is OrderedCollectionMold<TOCMold> ocMold)
        {
            ocMold.ResultCount = matchCount ?? 0;
        }
        // always close
        StyleFormatter.FormatCollectionEnd(this, matchCount, elementType, countCollectionAvailable ?? 0, formatString, formatFlags);
    }
}
