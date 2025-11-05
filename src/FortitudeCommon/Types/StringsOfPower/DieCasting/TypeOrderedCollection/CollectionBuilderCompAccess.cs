// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;

public class CollectionBuilderCompAccess<TOCMold> : TypeMolderDieCast<TOCMold> where TOCMold : TypeMolder
{
    public bool CollectionInComplexType { get; private set; }

    public CollectionBuilderCompAccess<TOCMold> InitializeOrderCollectionComponentAccess
        (TOCMold externalTypeBuilder, TypeMolder.StyleTypeBuilderPortableState typeBuilderPortableState, bool isComplex)
    {
        Initialize(externalTypeBuilder, typeBuilderPortableState);

        CollectionInComplexType = isComplex && Style.IsNotJson() || WriteAsComplex;
        
        return this;
    }
    
    public void ConditionalCollectionPrefix(Type elementType, bool? hasAny)
    {
        if (CollectionInComplexType)
        {
            StyleFormatter.AppendFieldName( Sb, "$values");
            StyleFormatter.AppendFieldValueSeparator(Sb);
            StyleFormatter.FormatCollectionStart(Sb, elementType, hasAny, TypeBeingBuilt);
        }
    }

    public bool ConditionalCollectionSuffix(Type elementType, int? count)
    {
        if (CollectionInComplexType)
        {
            StyleFormatter.FormatCollectionEnd(Sb, elementType, count);
        }
        return false;
    }
}
