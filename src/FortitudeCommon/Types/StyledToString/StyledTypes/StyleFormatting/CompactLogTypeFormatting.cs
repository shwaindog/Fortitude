// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.Mutable.Strings.CustomFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

public class CompactLogTypeFormatting : DefaultStringFormatter, IStyledTypeFormatting
{
    public string Name => nameof(CompactLogTypeFormatting);


    public IStringBuilder AppendTypeOpening<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append("{");

    public IStringBuilder AppendFieldName<TTypeBuilder>(TTypeBuilder typeBuilder, string fieldName)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append(fieldName);

    public IStringBuilder AppendFieldValueSeparator<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append(": ");

    public IStringBuilder AddNextFieldSeparator<TTypeBuilder>(TTypeBuilder typeBuilder)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess => typeBuilder.Sb.Append(", ");

    public IStringBuilder AppendTypeClosing<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append("}");

    public IStringBuilder FormatCollectionStart<TTypeBuilder>(TTypeBuilder typeBuilder, Type itemElementType
      , bool hasItems) where TTypeBuilder : IStyleTypeBuilderComponentAccess 
    {
        base.CollectionStart(itemElementType, typeBuilder.Sb, hasItems);
        return typeBuilder.Sb;
    }

    public IStringBuilder FormatCollectionNext<TTypeBuilder, TItem>(TTypeBuilder typeBuilder,  TItem toFormat, int itemAt
      , string? formatString = null)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess where TItem : ISpanFormattable
    {
        base.CollectionNextItem(toFormat, itemAt, typeBuilder.Sb);
        return typeBuilder.Sb;
    }

    public IStringBuilder AddCollectionElementSeparator<TTypeBuilder>(TTypeBuilder typeBuilder, Type elementType, int nextItemNumber)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        if (elementType == typeof(byte))
        {
            return typeBuilder.Sb.Append(nextItemNumber % 4 == 0 ? " " : "");
        }
        return typeBuilder.Sb.Append(", ");
    }
    
    public IStringBuilder FormatCollectionEnd<TTypeBuilder>(TTypeBuilder typeBuilder, Type itemElementType, int totalItemCount)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        base.CollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount);
        return typeBuilder.Sb;
    }
}
