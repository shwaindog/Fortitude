// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

public class CollectionBuilderCompAccess<TExt> : InternalStyledTypeBuilderComponentAccess<TExt> where TExt : StyledTypeBuilder
{
    public bool CollectionInComplexType { get; private set; }

    public CollectionBuilderCompAccess<TExt> InitializeOrderCollectionComponentAccess
        (TExt externalTypeBuilder, StyledTypeBuilder.StyleTypeBuilderPortableState typeBuilderPortableState, bool isComplex)
    {
        Initialize(externalTypeBuilder, typeBuilderPortableState);

        CollectionInComplexType = 
            isComplex && typeBuilderPortableState.OwningAppender.Style.AllowsUnstructured() || externalTypeBuilder.ExistingRefId > 0;
        
        return this;
    }
    
    public void ConditionalCollectionPrefix(Type elementType, bool hasAny)
    {
        if (CollectionInComplexType)
        {
            Sb.Append("$values: [");
        }
    }

    public bool ConditionalCollectionSuffix(Type elementType, int count)
    {
        if (CollectionInComplexType)
        {
            Sb.Append("]");
            return true;
        }
        return false;
    }
}
