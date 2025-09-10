// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.Mutable.Strings.CustomFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

public class PrettyJsonTypeFormatting : JsEscapingFormatter, IStyledTypeFormatting
{
    public string Name => nameof(CompactJsonTypeFormatting);

    public IStringBuilder AppendTypeOpening<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        typeBuilder.IncrementIndent();
        return typeBuilder.Sb.Append("{")
                          .Append(typeBuilder.OwningAppender.Settings.NewLineStyle)
                          .Append(typeBuilder.OwningAppender.Settings.IndentChar
                                , typeBuilder.OwningAppender.Settings.IndentRepeat(typeBuilder.OwningAppender.IndentLevel));
    }

    public IStringBuilder AppendFieldName<TTypeBuilder>(TTypeBuilder typeBuilder, string fieldName)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append("\"").Append(fieldName).Append("\"");

    public IStringBuilder AppendFieldToValueSeparator<TTypeBuilder>(TTypeBuilder typeBuilder, string fieldName)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append(": ");

    public IStringBuilder AppendTypeClosing<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        typeBuilder.DecrementIndent();
        return typeBuilder.Sb.Append(typeBuilder.OwningAppender.Settings.NewLineStyle)
                   .Append(typeBuilder.OwningAppender.Settings.IndentChar
                         , typeBuilder.OwningAppender.Settings.IndentRepeat(typeBuilder.OwningAppender.IndentLevel))
                   .Append("}");
    }
    
    public IStringBuilder FormatCollectionStart<TTypeBuilder>(TTypeBuilder typeBuilder, Type itemElementType
      , bool hasItems) where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        if (itemElementType == typeof(char) && CharArrayWritesString) return typeBuilder.Sb.Append("\"");
        if (itemElementType == typeof(byte) && ByteArrayWritesBase64String) return typeBuilder.Sb.Append("\"");
        
        typeBuilder.IncrementIndent();

        return typeBuilder.Sb.Append("[")
                          .Append(typeBuilder.OwningAppender.Settings.NewLineStyle)
                          .Append(typeBuilder.OwningAppender.Settings.IndentChar
                                , typeBuilder.OwningAppender.Settings.IndentRepeat(typeBuilder.OwningAppender.IndentLevel));
    }
    
    public override int FormatCollectionStart(Type elementType, IStringBuilder sb, bool hasItems)
    {
        if (elementType == typeof(char) && CharArrayWritesString) return sb.Append("\"").ReturnCharCount(1);
        if (elementType == typeof(byte) && ByteArrayWritesBase64String) return sb.Append("\"").ReturnCharCount(1);
        return sb.Append("[").ReturnCharCount(1);
    }

    public override int FormatCollectionStart(Type elementType, Span<char> destination, int destStartIndex, bool hasItems)
    {
        if (elementType == typeof(char) && CharArrayWritesString) return destination.OverWriteAt(destStartIndex, "\"");
        if (elementType == typeof(byte) && ByteArrayWritesBase64String) return destination.OverWriteAt(destStartIndex, "\"");
        return destination.OverWriteAt(destStartIndex, "[");
    }
    
    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && ByteArrayWritesBase64String) return 0;
        return sb.Append(", ").ReturnCharCount(1);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> charSpan, int atIndex, int nextItemNumber) 
    {
        if (collectionElementType == typeof(char) && CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && ByteArrayWritesBase64String) return 0;
        return charSpan.OverWriteAt(atIndex, ", ");
    }

    public IStringBuilder AddCollectionElementSeparator<TTypeBuilder, TItem>(TTypeBuilder typeBuilder, TItem lastItem, int nextItemNumber)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        base.AddCollectionElementSeparator(typeof(TItem), typeBuilder.Sb, nextItemNumber);
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
        return typeBuilder.Sb;
    }

    public IStringBuilder AddNextFieldSeparator<TTypeBuilder, TItem>(TTypeBuilder typeBuilder, TItem lastItem, int nextItemNumber)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess 
    {
        base.AddCollectionElementSeparator(typeof(TItem), typeBuilder.Sb, nextItemNumber);
        return typeBuilder.Sb;
    }
    
    public IStringBuilder FormatCollectionEnd<TTypeBuilder>(TTypeBuilder typeBuilder, Type itemElementType, int totalItemCount)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        if (itemElementType == typeof(char) && CharArrayWritesString)
        {
            FormatCollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount);
            return typeBuilder.Sb;
        }
        if (itemElementType == typeof(byte) && ByteArrayWritesBase64String)
        {
            FormatCollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount);
            return typeBuilder.Sb;
        }
        
        typeBuilder.DecrementIndent();
        typeBuilder.Sb.Append(typeBuilder.OwningAppender.Settings.NewLineStyle)
                   .Append(typeBuilder.OwningAppender.Settings.IndentChar
                         , typeBuilder.OwningAppender.Settings.IndentRepeat(typeBuilder.OwningAppender.IndentLevel));
        
        return typeBuilder.Sb.Append("]");
    }
}
