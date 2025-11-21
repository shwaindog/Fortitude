// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;

namespace FortitudeCommon.Types.StringsOfPower.Options;

public enum FieldSingleColumnLayoutFlags : ushort
{
    Default          = 0x00
  , FixedWidth       = 0x01
  , AddEllipsis      = 0x02
  , WhenLogWrapText  = 0x04
  , WrapLastLineText = 0x08
  , AlignedRight     = 0x10
  , AlignedCentred   = 0x20
  , AlignedLeft      = 0x40
  , NextFieldNewLine = 0x80
}

public readonly struct PrettySingleColumnAlignment(ushort contentLength, FieldSingleColumnLayoutFlags columnLayoutFlags)
{
    public ushort ContentLength { get; } = contentLength;

    public FieldSingleColumnLayoutFlags ColumnLayoutFlags { get; } = columnLayoutFlags;

    public static implicit operator PrettySingleColumnAlignment(int contentLength) =>
        new PrettySingleColumnAlignment((ushort)contentLength, FieldSingleColumnLayoutFlags.FixedWidth);
}

public enum PrettyFieldLayoutFlags : short
{
    NotSpecified               = 0x00_00
  , ExplicitRowWrap            = 0x00_01
  , FieldCountWrap             = 0x00_02
  , PadSkippedField            = 0x00_04
  , MinimumWidth               = 0x00_80
  , WhenLogAsTable             = 0x01_00
  , WhenLogNumberResultCount   = 0x02_00
  , WhenLogNumberIndex         = 0x04_00
  , WhenLogIncludeCountSummary = 0x08_00
}

public readonly struct PrettyFieldAlignment
{
    private static readonly ConcurrentDictionary<Type, PrettySingleColumnAlignment[]> CachedTypeAlignments = new();

    public PrettySingleColumnAlignment[]? AlignedColumnContentWidths { get; }

    public int? OverrideIndentLevel { get; }

    public int FieldCountLineWrap { get; } = 0;

    public PrettyFieldLayoutFlags FieldLayoutFlags { get; }

    public PrettyFieldAlignment()
    {
        FieldLayoutFlags           = PrettyFieldLayoutFlags.NotSpecified;
        AlignedColumnContentWidths = null;
    }

    public PrettyFieldAlignment(Type prettyAlignedType,
        PrettyFieldLayoutFlags layoutFlags, PrettySingleColumnAlignment col1, PrettySingleColumnAlignment col2
      , int fieldCountLineWrap = int.MaxValue, int? overrideIndentLevel = null)
    {
        FieldLayoutFlags           = layoutFlags;
        AlignedColumnContentWidths = CachedTypeAlignments.GetOrAdd(prettyAlignedType, _ => [col1, col2]);
        FieldCountLineWrap         = fieldCountLineWrap;
        OverrideIndentLevel        = overrideIndentLevel;
    }

    public PrettyFieldAlignment(Type prettyAlignedType,
        PrettyFieldLayoutFlags layoutFlags, PrettySingleColumnAlignment col1, PrettySingleColumnAlignment col2, PrettySingleColumnAlignment col3
      , int fieldCountLineWrap = int.MaxValue, int? overrideIndentLevel = null)
    {
        FieldLayoutFlags           = layoutFlags;
        AlignedColumnContentWidths = CachedTypeAlignments.GetOrAdd(prettyAlignedType, _ => [col1, col2, col3]);
        FieldCountLineWrap         = fieldCountLineWrap;
        OverrideIndentLevel        = overrideIndentLevel;
    }

    public PrettyFieldAlignment(Type prettyAlignedType,
        PrettyFieldLayoutFlags layoutFlags, PrettySingleColumnAlignment col1, PrettySingleColumnAlignment col2, PrettySingleColumnAlignment col3
      , PrettySingleColumnAlignment col4
      , int fieldCountLineWrap = int.MaxValue, int? overrideIndentLevel = null)
    {
        FieldLayoutFlags           = layoutFlags;
        AlignedColumnContentWidths = CachedTypeAlignments.GetOrAdd(prettyAlignedType, _ => [col1, col2, col3, col4]);
        FieldCountLineWrap         = fieldCountLineWrap;
        OverrideIndentLevel        = overrideIndentLevel;
    }

    public PrettyFieldAlignment(Type prettyAlignedType,
        PrettyFieldLayoutFlags layoutFlags, PrettySingleColumnAlignment col1, PrettySingleColumnAlignment col2, PrettySingleColumnAlignment col3
      , PrettySingleColumnAlignment col4, PrettySingleColumnAlignment col5
      , int fieldCountLineWrap = int.MaxValue, int? overrideIndentLevel = null)
    {
        FieldLayoutFlags           = layoutFlags;
        AlignedColumnContentWidths = CachedTypeAlignments.GetOrAdd(prettyAlignedType, _ => [col1, col2, col3, col4, col5]);
        FieldCountLineWrap         = fieldCountLineWrap;
        OverrideIndentLevel        = overrideIndentLevel;
    }

    public PrettyFieldAlignment(Type prettyAlignedType,
        PrettyFieldLayoutFlags layoutFlags, PrettySingleColumnAlignment col1, PrettySingleColumnAlignment col2, PrettySingleColumnAlignment col3
      , PrettySingleColumnAlignment col4, PrettySingleColumnAlignment col5, PrettySingleColumnAlignment col6
      , int fieldCountLineWrap = int.MaxValue, int? overrideIndentLevel = null)
    {
        FieldLayoutFlags           = layoutFlags;
        AlignedColumnContentWidths = CachedTypeAlignments.GetOrAdd(prettyAlignedType, _ => [col1, col2, col3, col4, col5, col6]);
        FieldCountLineWrap         = fieldCountLineWrap;
        OverrideIndentLevel        = overrideIndentLevel;
    }

    public PrettyFieldAlignment(Type prettyAlignedType,
        PrettyFieldLayoutFlags layoutFlags, PrettySingleColumnAlignment col1, PrettySingleColumnAlignment col2, PrettySingleColumnAlignment col3
      , PrettySingleColumnAlignment col4, PrettySingleColumnAlignment col5, PrettySingleColumnAlignment col6, PrettySingleColumnAlignment col7
      , int fieldCountLineWrap = int.MaxValue, int? overrideIndentLevel = null)
    {
        FieldLayoutFlags           = layoutFlags;
        AlignedColumnContentWidths = CachedTypeAlignments.GetOrAdd(prettyAlignedType, _ => [col1, col2, col3, col4, col5, col6, col7]);
        FieldCountLineWrap         = fieldCountLineWrap;
        OverrideIndentLevel        = overrideIndentLevel;
    }

    public PrettyFieldAlignment(Type prettyAlignedType,
        PrettyFieldLayoutFlags layoutFlags, PrettySingleColumnAlignment col1, PrettySingleColumnAlignment col2, PrettySingleColumnAlignment col3
      , PrettySingleColumnAlignment col4, PrettySingleColumnAlignment col5, PrettySingleColumnAlignment col6, PrettySingleColumnAlignment col7
      , PrettySingleColumnAlignment col8
      , int fieldCountLineWrap = int.MaxValue, int? overrideIndentLevel = null)
    {
        FieldLayoutFlags           = layoutFlags;
        AlignedColumnContentWidths = CachedTypeAlignments.GetOrAdd(prettyAlignedType, _ => [col1, col2, col3, col4, col5, col6, col7, col8]);
        FieldCountLineWrap         = fieldCountLineWrap;
        OverrideIndentLevel        = overrideIndentLevel;
    }

    public PrettyFieldAlignment(Type prettyAlignedType,
        PrettyFieldLayoutFlags layoutFlags, PrettySingleColumnAlignment col1, PrettySingleColumnAlignment col2, PrettySingleColumnAlignment col3
      , PrettySingleColumnAlignment col4, PrettySingleColumnAlignment col5, PrettySingleColumnAlignment col6, PrettySingleColumnAlignment col7
      , PrettySingleColumnAlignment col8, PrettySingleColumnAlignment col9
      , int fieldCountLineWrap = int.MaxValue, int? overrideIndentLevel = null)
    {
        FieldLayoutFlags           = layoutFlags;
        AlignedColumnContentWidths = CachedTypeAlignments.GetOrAdd(prettyAlignedType, _ => [col1, col2, col3, col4, col5, col6, col7, col8, col9]);
        FieldCountLineWrap         = fieldCountLineWrap;
        OverrideIndentLevel        = overrideIndentLevel;
    }

    public PrettyFieldAlignment(Type prettyAlignedType,
        PrettyFieldLayoutFlags layoutFlags, PrettySingleColumnAlignment col1, PrettySingleColumnAlignment col2, PrettySingleColumnAlignment col3
      , PrettySingleColumnAlignment col4, PrettySingleColumnAlignment col5, PrettySingleColumnAlignment col6, PrettySingleColumnAlignment col7
      , PrettySingleColumnAlignment col8, PrettySingleColumnAlignment col9, PrettySingleColumnAlignment col10
      , int fieldCountLineWrap = int.MaxValue, int? overrideIndentLevel = null)
    {
        FieldLayoutFlags = layoutFlags;
        AlignedColumnContentWidths =
            CachedTypeAlignments.GetOrAdd
                (prettyAlignedType, _ =>
                     [col1, col2, col3, col4, col5, col6, col7, col8, col9, col10]);
        FieldCountLineWrap  = fieldCountLineWrap;
        OverrideIndentLevel = overrideIndentLevel;
    }
}
