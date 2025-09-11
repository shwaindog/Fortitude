// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.Mutable.Strings.CustomFormatting;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;

public interface IStyledTypeFormatting : ICustomStringFormatter
{
    string Name { get; }

    IStringBuilder AppendTypeOpening<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess;

    IStringBuilder AppendFieldName<TTypeBuilder>(TTypeBuilder typeBuilder, string fieldName) where TTypeBuilder : IStyleTypeBuilderComponentAccess;
    
    IStringBuilder AppendFieldValueSeparator<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess;

    IStringBuilder AppendTypeClosing<TTypeBuilder>(TTypeBuilder typeBuilder) where TTypeBuilder : IStyleTypeBuilderComponentAccess;
    
    IStringBuilder FormatCollectionStart<TTypeBuilder>(TTypeBuilder typeBuilder, Type itemElementType, bool hasItems)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess;
    
    IStringBuilder FormatCollectionEnd<TTypeBuilder>(TTypeBuilder typeBuilder, Type itemElementType, int totalItemCount) 
        where TTypeBuilder : IStyleTypeBuilderComponentAccess;
    
    IStringBuilder AddCollectionElementSeparator<TTypeBuilder>(TTypeBuilder typeBuilder, Type elementType, int nextItemNumber)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess;

    IStringBuilder AddNextFieldSeparator<TTypeBuilder>(TTypeBuilder typeBuilder)
        where TTypeBuilder : IStyleTypeBuilderComponentAccess;
}
