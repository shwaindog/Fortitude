// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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

    public IStringBuilder AppendFieldToValueSeparator<TTypeBuilder>(TTypeBuilder typeBuilder, string fieldName)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append(":");

    public IStringBuilder AppendTypeClosing<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess =>
        typeBuilder.Sb.Append("}");

    public IStringBuilder FormatCollectionStart<TTypeBuilder>(TTypeBuilder typeBuilder, Type itemElementType
      , bool hasItems) where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        base.FormatCollectionStart(itemElementType, typeBuilder.Sb, hasItems);
        return typeBuilder.Sb;
    }

    public IStringBuilder AddCollectionElementSeparator<TTypeBuilder, TItem>(TTypeBuilder typeBuilder, TItem lastItem, int nextItemNumber)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess => typeBuilder.Sb.Append(",");

    public IStringBuilder AddNextFieldSeparator<TTypeBuilder, TItem>(TTypeBuilder typeBuilder, TItem lastItem, int nextItemNumber)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess => typeBuilder.Sb.Append(", ");

    public IStringBuilder FormatCollectionEnd<TTypeBuilder>(TTypeBuilder typeBuilder, Type itemElementType
      , int totalItemCount) where TTypeBuilder : IStyleTypeBuilderComponentAccess
    {
        base.FormatCollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount);
        return typeBuilder.Sb;
    }
}
