// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.Mutable.Strings.CustomFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

public class CompactJsonTypeFormatting : JsEscapingFormatter, IStyledTypeFormatting
{
    public string Name => nameof(CompactJsonTypeFormatting);

    public IStringBuilder AppendTypeOpening<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append("{");

    public IStringBuilder AppendFieldName<TTypeBuilder>(TTypeBuilder typeBuilder, string fieldName)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append("\"").Append(fieldName).Append("\"");
    
    public IStringBuilder AppendFieldValueSeparator<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append(":");
    
    public IStringBuilder AddNextFieldSeparator<TTypeBuilder>(TTypeBuilder typeBuilder)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess => typeBuilder.Sb.Append(",");

    public IStringBuilder AppendTypeClosing<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append("}");

    public IStringBuilder FormatCollectionStart<TTypeBuilder>(TTypeBuilder typeBuilder, Type itemElementType
      , bool hasItems) where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        base.CollectionStart(itemElementType, typeBuilder.Sb, hasItems);
        return typeBuilder.Sb;
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

    public IStringBuilder AddCollectionElementSeparator<TTypeBuilder>(TTypeBuilder typeBuilder, Type elementType, int nextItemNumber)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        base.AddCollectionElementSeparator(elementType, typeBuilder.Sb, nextItemNumber);
        return typeBuilder.Sb;
    }

    public IStringBuilder FormatCollectionEnd<TTypeBuilder>(TTypeBuilder typeBuilder, Type itemElementType
      , int totalItemCount) where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        base.CollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount);
        return typeBuilder.Sb;
    }
}
