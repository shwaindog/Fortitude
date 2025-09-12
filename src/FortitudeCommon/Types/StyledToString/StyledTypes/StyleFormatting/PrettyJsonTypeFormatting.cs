// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.Mutable.Strings.CustomFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

public class PrettyJsonTypeFormatting : CompactJsonTypeFormatting
{
    protected const string CmaSpc = ", ";
    protected const string ClnSpc = ": ";
    
    public override string Name => nameof(CompactJsonTypeFormatting);

    public override IStyleTypeBuilderComponentAccess<TB> AppendComplexTypeOpening<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, string? typeName = null)
    {
        typeBuilder.IncrementIndent();
        return (typeBuilder.Sb.Append(BrcOpn)
                          .Append(typeBuilder.OwningAppender.Settings.NewLineStyle)
                          .Append(typeBuilder.OwningAppender.Settings.IndentChar
                                , typeBuilder.OwningAppender.Settings.IndentRepeat(typeBuilder.OwningAppender.IndentLevel)))
            .ToInternalTypeBuilder(typeBuilder);
    }

    public override IStyleTypeBuilderComponentAccess<TB> AppendFieldValueSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder) =>
        typeBuilder.Sb.Append(ClnSpc).ToInternalTypeBuilder(typeBuilder);

    public override IStyleTypeBuilderComponentAccess<TB> AddNextFieldSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder) => 
        typeBuilder.Sb.Append(CmaSpc).ToInternalTypeBuilder(typeBuilder);

    public override IStyleTypeBuilderComponentAccess<TB> AppendTypeClosing<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder)
    {
        typeBuilder.DecrementIndent();
        return (typeBuilder.Sb.Append(typeBuilder.OwningAppender.Settings.NewLineStyle)
                   .Append(typeBuilder.OwningAppender.Settings.IndentChar
                         , typeBuilder.OwningAppender.Settings.IndentRepeat(typeBuilder.OwningAppender.IndentLevel))
                   .Append(BrcCls))
            .ToInternalTypeBuilder(typeBuilder);;
    }
    
    public override IStyleTypeBuilderComponentAccess<TB> FormatCollectionStart<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type itemElementType
      , bool hasItems, Type collectionType)
    {
        if (itemElementType == typeof(char) && CharArrayWritesString) return typeBuilder.Sb.Append(DblQt).ToInternalTypeBuilder(typeBuilder);
        if (itemElementType == typeof(byte) && ByteArrayWritesBase64String) return typeBuilder.Sb.Append(DblQt).ToInternalTypeBuilder(typeBuilder);
        
        typeBuilder.IncrementIndent();

        return (typeBuilder.Sb.Append(SqBrktOpn)
                          .Append(typeBuilder.OwningAppender.Settings.NewLineStyle)
                          .Append(typeBuilder.OwningAppender.Settings.IndentChar
                                , typeBuilder.OwningAppender.Settings.IndentRepeat(typeBuilder.OwningAppender.IndentLevel)))
            .ToInternalTypeBuilder(typeBuilder);
    }
    
    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems)
    {
        if (elementType == typeof(char) && CharArrayWritesString) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(byte) && ByteArrayWritesBase64String) return sb.Append(DblQt).ReturnCharCount(1);
        return sb.Append(SqBrktOpn).ReturnCharCount(1);
    }

    public override int CollectionStart(Type elementType, Span<char> destination, int destStartIndex, bool hasItems)
    {
        if (elementType == typeof(char) && CharArrayWritesString) return destination.OverWriteAt(destStartIndex, DblQt);
        if (elementType == typeof(byte) && ByteArrayWritesBase64String) return destination.OverWriteAt(destStartIndex, DblQt);
        return destination.OverWriteAt(destStartIndex, SqBrktOpn);
    }
    
    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && ByteArrayWritesBase64String) return 0;
        return sb.Append(CmaSpc).ReturnCharCount(1);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> charSpan, int atIndex, int nextItemNumber) 
    {
        if (collectionElementType == typeof(char) && CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && ByteArrayWritesBase64String) return 0;
        return charSpan.OverWriteAt(atIndex, CmaSpc);
    }

    public override IStyleTypeBuilderComponentAccess<TB> AddCollectionElementSeparator<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type elementType, int nextItemNumber)
    {
        base.AddCollectionElementSeparator(elementType, typeBuilder.Sb, nextItemNumber);
        if (typeBuilder.Settings.EnableColumnContentWidthWrap)
        {
            if (typeBuilder.Settings.PrettyCollectionsColumnContentWidthWrap < typeBuilder.Sb.LineContentWidth)
            {
                typeBuilder.Sb.Length -= 1; // remove last Space
                typeBuilder.Sb.Append(typeBuilder.OwningAppender.Settings.NewLineStyle)
                           .Append(typeBuilder.OwningAppender.Settings.IndentChar
                                 , typeBuilder.OwningAppender.Settings.IndentRepeat(typeBuilder.OwningAppender.IndentLevel));
            }
        }
        return typeBuilder;
    }

    public override IStyleTypeBuilderComponentAccess<TB> FormatCollectionEnd<TB>(IStyleTypeBuilderComponentAccess<TB> typeBuilder, Type itemElementType, int totalItemCount)
    {
        if (itemElementType == typeof(char) && CharArrayWritesString)
        {
            CollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount);
            return typeBuilder;
        }
        if (itemElementType == typeof(byte) && ByteArrayWritesBase64String)
        {
            CollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount);
            return typeBuilder;
        }
        
        typeBuilder.DecrementIndent();
        typeBuilder.Sb.Append(typeBuilder.OwningAppender.Settings.NewLineStyle)
                   .Append(typeBuilder.OwningAppender.Settings.IndentChar
                         , typeBuilder.OwningAppender.Settings.IndentRepeat(typeBuilder.OwningAppender.IndentLevel));
        
        return typeBuilder.Sb.Append(SqBrktCls).ToInternalTypeBuilder(typeBuilder);
    }
}
