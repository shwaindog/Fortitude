// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Nodes;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class PrettyJsonTypeFormatting : CompactJsonTypeFormatting
{
    protected const string CmaSpc = ", ";
    protected const string ClnSpc = ": ";
    
    public override string Name => nameof(CompactJsonTypeFormatting);

    public override PrettyJsonTypeFormatting Initialize(StyleOptions styleOptions)
    {
        base.Initialize(styleOptions);

        return this;
    }

    public override ITypeMolderDieCast<TB> AppendComplexTypeOpening<TB>(ITypeMolderDieCast<TB> typeBuilder, Type complextType, string? alternativeName = null)
    {
        typeBuilder.IncrementIndent();
        return (typeBuilder.Sb.Append(BrcOpn)
                          .Append(typeBuilder.Master.Settings.NewLineStyle)
                          .Append(typeBuilder.Master.Settings.IndentChar
                                , typeBuilder.Master.Settings.IndentRepeat(typeBuilder.IndentLevel)))
            .ToInternalTypeBuilder(typeBuilder);
    }

    public override ITypeMolderDieCast<TB> AppendFieldValueSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder) =>
        typeBuilder.Sb.Append(ClnSpc).ToInternalTypeBuilder(typeBuilder);
    
    public override ITypeMolderDieCast<TB> AddNextFieldSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder)
    {
        return
            typeBuilder.Sb
                       .Append(Cma)
                       .Append(typeBuilder.Master.Settings.NewLineStyle)
                       .Append(typeBuilder.Master.Settings.IndentChar
                             , typeBuilder.Master.Settings.IndentRepeat(typeBuilder.IndentLevel))
                       .ToInternalTypeBuilder(typeBuilder);
    }
    
    public override int InsertFieldSeparatorAt(IStringBuilder sb, int atIndex, StyleOptions options, int indentLevel)
    {
        var nlPadding = options.IndentSize * indentLevel;
        var bufferSize = nlPadding + 1 + options.NewLineStyle.Length;
        var nextFieldStart = stackalloc char[bufferSize].ResetMemory();
        nextFieldStart[0] = ',';
        nextFieldStart.Append(options.NewLineStyle);
        var spacesFrom = nextFieldStart.PopulatedLength();
        for (int i = spacesFrom; i < bufferSize; i++)
        {
            nextFieldStart[i] = ' ';
        }
        sb.InsertAt(nextFieldStart, atIndex);
        return bufferSize;
    }

    public override ITypeMolderDieCast<TB> AppendTypeClosing<TB>(ITypeMolderDieCast<TB> typeBuilder)
    {
        typeBuilder.Sb.RemoveLastWhiteSpacedCommaIfFound();
        typeBuilder.DecrementIndent();
        return (typeBuilder.Sb.Append(typeBuilder.Master.Settings.NewLineStyle)
                   .Append(typeBuilder.Master.Settings.IndentChar
                         , typeBuilder.Master.Settings.IndentRepeat(typeBuilder.IndentLevel))
                   .Append(BrcCls))
            .ToInternalTypeBuilder(typeBuilder);
    }


    public override ITypeMolderDieCast<TB> AppendKeyedCollectionStart<TB>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType
      , Type keyType, Type valueType) 
    {
        base.AppendKeyedCollectionStart(typeBuilder, keyedCollectionType, keyType, valueType);
        
        typeBuilder.IncrementIndent();
        
        var sb = typeBuilder.Sb;
        return sb.Append(typeBuilder.Master.Settings.NewLineStyle)
            .Append(typeBuilder.Master.Settings.IndentChar
                  , typeBuilder.Master.Settings.IndentRepeat(typeBuilder.IndentLevel))
            .ToInternalTypeBuilder(typeBuilder);
    }

    public override ITypeMolderDieCast<TB> AppendKeyedCollectionEnd<TB>(ITypeMolderDieCast<TB> typeBuilder, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount)
    {
        typeBuilder.Sb.RemoveLastWhiteSpacedCommaIfFound();
        typeBuilder.DecrementIndent();
        if (totalItemCount > 0)
        {
            typeBuilder.Sb.Append(typeBuilder.Master.Settings.NewLineStyle)
                       .Append(typeBuilder.Master.Settings.IndentChar
                             , typeBuilder.Master.Settings.IndentRepeat(typeBuilder.IndentLevel));
        }
        base.AppendKeyedCollectionEnd(typeBuilder, keyedCollectionType, keyType, valueType, totalItemCount);
        return typeBuilder;
    }
    
    public override ITypeMolderDieCast<TB> FormatCollectionStart<TB>(ITypeMolderDieCast<TB> typeBuilder, Type itemElementType
      , bool hasItems, Type collectionType)
    {
        if (!hasItems) return typeBuilder;
        CollectionStart(itemElementType, typeBuilder.Sb, hasItems);
        return typeBuilder;
    }
    
    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems)
    {
        StyleOptions.IndentLevel++;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return sb.Append(BrcOpn).ReturnCharCount(1);
        var originalLen = sb.Length;
        sb.Append(SqBrktOpn)
                  .Append(StyleOptions.NewLineStyle)
                  .Append(StyleOptions.IndentChar
                        , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        
        return sb.Length - originalLen;
    }

    public override int CollectionStart(Type elementType, Span<char> destSpan, int destStartIndex, bool hasItems) 
    {
        StyleOptions.IndentLevel++;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return destSpan.OverWriteAt(destStartIndex, DblQt);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return destSpan.OverWriteAt(destStartIndex, DblQt);
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return destSpan.OverWriteAt(destStartIndex, BrcOpn);
        var addedChars = destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
        addedChars += destSpan.OverWriteAt(destStartIndex + addedChars, StyleOptions.NewLineStyle);
        addedChars += destSpan.OverWriteRepatAt(destStartIndex + addedChars, StyleOptions.IndentChar
                                         , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        
        return addedChars;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return 0;
        var originalLen = sb.Length;
        sb.Append(Cma);
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
        return sb.Length - originalLen;
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> destSpan, int atIndex, int nextItemNumber) 
    {
        if (collectionElementType == typeof(char) && JsonOptions.CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String) return 0;
        var addedChars = destSpan.OverWriteAt(atIndex, Cma);
        if (StyleOptions.PrettyCollectionStyle.IsCollectionContentWidthWrap())
        {
            if (StyleOptions.PrettyCollectionsColumnContentWidthWrap < destSpan.FindLineLengthFrom(atIndex))
            {
                addedChars += destSpan.OverWriteAt(atIndex, StyleOptions.NewLineStyle);
                addedChars += destSpan.OverWriteRepatAt(atIndex, StyleOptions.IndentChar
                                                  , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
            }
        }
        else
        {
            addedChars += destSpan.OverWriteAt(atIndex, StyleOptions.NewLineStyle);
            addedChars += destSpan.OverWriteRepatAt(atIndex, StyleOptions.IndentChar
                                                  , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        return addedChars;
    }

    public override ITypeMolderDieCast<TB> AddCollectionElementSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder, Type elementType, int nextItemNumber)
    {
        AddCollectionElementSeparator(elementType, typeBuilder.Sb, nextItemNumber);
        return typeBuilder;
    }

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount) 
    {
        StyleOptions.IndentLevel--;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return sb.Append(DblQt).ReturnCharCount(1);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            var addedChars = CompleteBase64Sequence(sb);
            return sb.Append(DblQt).ReturnCharCount(1 + addedChars);
        }
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return sb.Append(BrcCls).ReturnCharCount(1);
        var originalLen = sb.Length;
        sb.RemoveLastWhiteSpacedCommaIfFound();
        if (itemsCount > 0)
        {
            sb.Append(StyleOptions.NewLineStyle)
                       .Append(StyleOptions.IndentChar
                             , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        return sb.Append(SqBrktCls).ReturnCharCount(sb.Length - originalLen);
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int index, int itemsCount)
    {
        StyleOptions.IndentLevel--;
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        if (elementType == typeof(char) && JsonOptions.CharArrayWritesString) return destSpan.OverWriteAt(index, DblQt);
        if (elementType == typeof(byte) && JsonOptions.ByteArrayWritesBase64String)
        {
            var charsAdded = CompleteBase64Sequence(destSpan, index);
            return destSpan.OverWriteAt(index + charsAdded, DblQt) + charsAdded;
        }
        if (elementType == typeof(KeyValuePair<string, JsonNode>)) return destSpan.OverWriteAt(index, BrcCls);
        var addedChars = destSpan.RemoveLastWhiteSpacedCommaIfFound(index);
        if (itemsCount > 0)
        {
            addedChars += destSpan.OverWriteAt(index + addedChars, StyleOptions.NewLineStyle);
            addedChars += destSpan.OverWriteRepatAt(index + addedChars, StyleOptions.IndentChar
                                                  , StyleOptions.IndentRepeat(StyleOptions.IndentLevel));
        }
        return addedChars;
    }

    public override ITypeMolderDieCast<TB> FormatCollectionEnd<TB>(ITypeMolderDieCast<TB> typeBuilder, Type itemElementType, int totalItemCount)
    {
        CollectionEnd(itemElementType, typeBuilder.Sb, totalItemCount);
        return typeBuilder;
    }
}
