// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public class CollectionBuilderCompAccess<TOCMold> : TypeMolderDieCast<TOCMold> where TOCMold : TypeMolder
{

    public CollectionBuilderCompAccess<TOCMold> InitializeOrderCollectionComponentAccess
        (TOCMold externalTypeBuilder, TypeMolder.MoldPortableState typeBuilderPortableState, WriteMethodType writeMethod)
    {
        writeMethod = typeBuilderPortableState.ExistingRefId > 0 
                   || writeMethod.SupportsMultipleFields() && typeBuilderPortableState.Master.Style.IsLog()
                      ? writeMethod
                      : writeMethod.ToNoFieldEquivalent();
        
        Initialize(externalTypeBuilder, typeBuilderPortableState, writeMethod);
        
        return this;
    }
    
    public void ConditionalCollectionPrefix(Type elementType, bool? hasAny)
    {
        if (SupportsMultipleFields)
        {
            StyleFormatter.AppendFieldName( Sb, "$values");
            StyleFormatter.AppendFieldValueSeparator();
            if(hasAny == true)
                StyleFormatter.FormatCollectionStart(this, elementType, hasAny, TypeBeingBuilt);
        }
    }

    public bool ConditionalCollectionSuffix(Type elementType, int? count, string? formatString, FormatFlags formatFlags)
    {
        if (StyleTypeBuilder is OrderedCollectionMold<TOCMold> ocMold)
        {
            ocMold.ResultCount = count ?? 0;
        }
        if (SupportsMultipleFields)
        {
            if (!count.HasValue)
            {
                StyleFormatter.AppendFormattedNull( Sb, formatString, formatFlags);
            }
            else
            {
                StyleFormatter.FormatCollectionEnd(this, count, elementType, count, formatString);
            }
        }
        return false;
    }
}
