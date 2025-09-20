// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.CustomFormatting;
using FortitudeCommon.Types.StringsOfPower.Options;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;

public class PrettyJsonTypeFormatting : CompactJsonTypeFormatting
{
    protected const string CmaSpc = ", ";
    protected const string ClnSpc = ": ";
    
    public override string Name => nameof(CompactJsonTypeFormatting);

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
                       .Append(CmaSpc)
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
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
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
        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
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
        if (itemElementType == typeof(char) && CharArrayWritesString) return typeBuilder.Sb.Append(DblQt).ToInternalTypeBuilder(typeBuilder);
        if (itemElementType == typeof(byte) && ByteArrayWritesBase64String) return typeBuilder.Sb.Append(DblQt).ToInternalTypeBuilder(typeBuilder);

        if (!hasItems) return typeBuilder;
        typeBuilder.IncrementIndent();
        
        return (typeBuilder.Sb.Append(SqBrktOpn)
                          .Append(typeBuilder.Master.Settings.NewLineStyle)
                          .Append(typeBuilder.Master.Settings.IndentChar
                                , typeBuilder.Master.Settings.IndentRepeat(typeBuilder.IndentLevel)))
            .ToInternalTypeBuilder(typeBuilder);
    }
    
    // public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems)
    // {
    //     if (elementType == typeof(char) && CharArrayWritesString) return sb.Append(DblQt).ReturnCharCount(1);
    //     if (elementType == typeof(byte) && ByteArrayWritesBase64String) return sb.Append(DblQt).ReturnCharCount(1);
    //     
    //     return sb.Append(SqBrktOpn).ReturnCharCount(1);
    // }
    //
    // public override int CollectionStart(Type elementType, Span<char> destination, int destStartIndex, bool hasItems)
    // {
    //     if (elementType == typeof(char) && CharArrayWritesString) return destination.OverWriteAt(destStartIndex, DblQt);
    //     if (elementType == typeof(byte) && ByteArrayWritesBase64String) return destination.OverWriteAt(destStartIndex, DblQt);
    //     return destination.OverWriteAt(destStartIndex, SqBrktOpn);
    // }
    
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

    public override ITypeMolderDieCast<TB> AddCollectionElementSeparator<TB>(ITypeMolderDieCast<TB> typeBuilder, Type elementType, int nextItemNumber)
    {
        base.AddCollectionElementSeparator(elementType, typeBuilder.Sb, nextItemNumber);
        if(elementType == typeof(byte) && ByteArrayWritesBase64String) return typeBuilder;
        if (elementType == typeof(char) && CharArrayWritesString) return typeBuilder;
        if (typeBuilder.Settings.EnableColumnContentWidthWrap)
        {
            if (typeBuilder.Settings.PrettyCollectionsColumnContentWidthWrap < typeBuilder.Sb.LineContentWidth)
            {
                typeBuilder.Sb.Length -= 1; // remove last Space
                typeBuilder.Sb.Append(typeBuilder.Master.Settings.NewLineStyle)
                           .Append(typeBuilder.Master.Settings.IndentChar
                                 , typeBuilder.Master.Settings.IndentRepeat(typeBuilder.IndentLevel));
            }
        }
        else
        {
            typeBuilder.Sb.Append(typeBuilder.Master.Settings.NewLineStyle)
                       .Append(typeBuilder.Master.Settings.IndentChar
                             , typeBuilder.Master.Settings.IndentRepeat(typeBuilder.IndentLevel));
        }
        return typeBuilder;
    }

    public override ITypeMolderDieCast<TB> FormatCollectionEnd<TB>(ITypeMolderDieCast<TB> typeBuilder, Type itemElementType, int totalItemCount)
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

        typeBuilder.RemoveLastWhiteSpacedCommaIfFound();
        if (totalItemCount > 0)
        {
            typeBuilder.DecrementIndent();
            typeBuilder.Sb.Append(typeBuilder.Master.Settings.NewLineStyle)
                       .Append(typeBuilder.Master.Settings.IndentChar
                             , typeBuilder.Master.Settings.IndentRepeat(typeBuilder.IndentLevel));
        }
        return typeBuilder.Sb.Append(SqBrktCls).ToInternalTypeBuilder(typeBuilder);
    }
}
