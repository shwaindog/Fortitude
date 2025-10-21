// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class PrettyLogTypeFormatting : CompactLogTypeFormatting
{
    protected const string DblQt      = "\"";
    protected const char   DblQtChar  = '"';
    protected const string BrcOpn     = "{";
    protected const char   BrcOpnChar = '{';
    protected const string BrcCls     = "}";
    protected const char   BrcClsChar = '}';
    protected const string Cma        = ",";
    protected const string Cln        = ":";

    public override PrettyLogTypeFormatting Initialize(StyleOptions styleOptions)
    {
        Options = styleOptions;

        return this;
    }

    public override string Name => nameof(CompactJsonTypeFormatting);

    public override IStringBuilder AppendComplexTypeOpening(IStringBuilder sb, Type complextType
      , string? alternativeName = null)
    {
        StyleOptions.IndentLevel++;
        return sb.Append(BrcOpn)
                 .Append(StyleOptions.NewLineStyle)
                 .Append(StyleOptions.IndentChar
                       , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
    }

    public override IStringBuilder AppendFieldValueSeparator(IStringBuilder sb) => sb.Append(ClnSpc);

    public override IStringBuilder AddNextFieldSeparator(IStringBuilder sb) =>
        sb.Append(Cma)
          .Append(StyleOptions.NewLineStyle)
          .Append(StyleOptions.IndentChar
                , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));

    public override int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel)
    {
        var nlPadding      = options.IndentSize * indentLevel;
        var bufferSize     = nlPadding + 1 + options.NewLineStyle.Length;
        var nextFieldStart = stackalloc char[bufferSize].ResetMemory();
        nextFieldStart[0] = ',';
        nextFieldStart.Append(options.NewLineStyle);
        var spacesFrom                                                  = nextFieldStart.PopulatedLength();
        for (var i = spacesFrom; i < bufferSize; i++) nextFieldStart[i] = ' ';
        sb.InsertAt(nextFieldStart, atIndex);
        return bufferSize;
    }

    public override IStringBuilder AppendTypeClosing(IStringBuilder sb)
    {
        var lastNonWhiteSpace = sb.RemoveLastWhiteSpacedCommaIfFound();
        StyleOptions.IndentLevel--;
        return lastNonWhiteSpace != BrcOpnChar 
            ? sb.Append(StyleOptions.NewLineStyle)
                .Append(StyleOptions.IndentChar
                      , StyleOptions.IndentRepeat(StyleOptions.IndentLevel))
                .Append(BrcCls)
            : sb.Append(BrcCls);
    }


    public override IStringBuilder AppendKeyedCollectionStart(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType)
    {
        base.AppendKeyedCollectionStart(sb, keyedCollectionType, keyType, valueType);

        StyleOptions.IndentLevel++;

        return sb.Append(StyleOptions.NewLineStyle)
                 .Append(StyleOptions.IndentChar
                       , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
    }

    public override IStringBuilder AppendKeyedCollectionEnd(IStringBuilder sb, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount)
    {
        sb.RemoveLastWhiteSpacedCommaIfFound();
        StyleOptions.IndentLevel--;
        if (totalItemCount > 0)
            sb.Append(StyleOptions.NewLineStyle)
              .Append(StyleOptions.IndentChar
                    , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        return base.AppendKeyedCollectionEnd(sb, keyedCollectionType, keyType, valueType, totalItemCount);
    }

    public override IStringBuilder FormatCollectionStart(IStringBuilder sb, Type itemElementType
      , bool hasItems, Type collectionType)
    {
        if (itemElementType == typeof(char) && StyleOptions.CharArrayWritesString) return sb.Append(DblQt);
        if (itemElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String) return sb.Append(DblQt);

        if (!hasItems) return sb;
        StyleOptions.IndentLevel++;

        return sb.Append(SqBrktOpn)
                 .Append(StyleOptions.NewLineStyle)
                 .Append(StyleOptions.IndentChar
                       , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && StyleOptions.CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String) return 0;
        return sb.Append(CmaSpc).ReturnCharCount(1);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && StyleOptions.CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String) return 0;
        return destSpan.OverWriteAt(atIndex, CmaSpc);
    }

    public override IStringBuilder AddCollectionElementSeparator(IStringBuilder sb, Type elementType
      , int nextItemNumber)
    {
        base.AddCollectionElementSeparator(elementType, sb, nextItemNumber);
        if (elementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String) return sb;
        if (elementType == typeof(char) && StyleOptions.CharArrayWritesString) return sb;
        if (StyleOptions.PrettyCollectionStyle.IsCollectionContentWidthWrap())
        {
            if (StyleOptions.PrettyCollectionsColumnContentWidthWrap < sb.LineContentWidth)
            {
                sb.Length -= 1; // remove last Space
                sb.Append(StyleOptions.NewLineStyle)
                  .Append(StyleOptions.IndentChar
                        , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        else
        {
            sb.Append(StyleOptions.NewLineStyle)
              .Append(StyleOptions.IndentChar
                    , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        return sb;
    }

    public override IStringBuilder FormatCollectionEnd(IStringBuilder sb, Type itemElementType, int totalItemCount)
    {
        if (itemElementType == typeof(char) && StyleOptions.CharArrayWritesString)
        {
            return CollectionEnd(itemElementType, sb, totalItemCount).ToStringBuilder(sb);
        }
        if (itemElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String)
        {
            return CollectionEnd(itemElementType, sb, totalItemCount).ToStringBuilder(sb);
        }

        sb.RemoveLastWhiteSpacedCommaIfFound();
        if (totalItemCount > 0)
        {
            StyleOptions.IndentLevel--;
            sb.Append(StyleOptions.NewLineStyle)
              .Append(StyleOptions.IndentChar
                    , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        return sb.Append(SqBrktCls);
    }
}
