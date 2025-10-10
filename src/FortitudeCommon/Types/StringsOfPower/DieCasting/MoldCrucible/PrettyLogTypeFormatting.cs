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

    public virtual StyleOptions StyleOptions
    {
        get => (StyleOptions)(FormatOptions ??= new StyleOptions());
        set => FormatOptions = value;
    }

    public override ITypeMolderDieCast<TMold> AppendComplexTypeOpening<TMold>(ITypeMolderDieCast<TMold> typeMold, Type complextType
      , string? alternativeName = null)
    {
        typeMold.IncrementIndent();
        return typeMold.Sb.Append(BrcOpn)
                       .Append(typeMold.Master.Settings.NewLineStyle)
                       .Append(typeMold.Master.Settings.IndentChar
                             , typeMold.Master.Settings.IndentRepeat(typeMold.IndentLevel))
                       .ToInternalTypeBuilder(typeMold);
    }

    public override ITypeMolderDieCast<TMold> AppendFieldValueSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold) =>
        typeMold.Sb.Append(ClnSpc).ToInternalTypeBuilder(typeMold);

    public override ITypeMolderDieCast<TMold> AddNextFieldSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold) =>
        typeMold.Sb
                .Append(Cma)
                .Append(typeMold.Master.Settings.NewLineStyle)
                .Append(typeMold.Master.Settings.IndentChar
                      , typeMold.Master.Settings.IndentRepeat(typeMold.IndentLevel))
                .ToInternalTypeBuilder(typeMold);

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

    public override ITypeMolderDieCast<TMold> AppendTypeClosing<TMold>(ITypeMolderDieCast<TMold> typeMold)
    {
        typeMold.Sb.RemoveLastWhiteSpacedCommaIfFound();
        typeMold.DecrementIndent();
        return typeMold.Sb.Append(typeMold.Master.Settings.NewLineStyle)
                       .Append(typeMold.Master.Settings.IndentChar
                             , typeMold.Master.Settings.IndentRepeat(typeMold.IndentLevel))
                       .Append(BrcCls)
                       .ToInternalTypeBuilder(typeMold);
    }


    public override ITypeMolderDieCast<TMold> AppendKeyedCollectionStart<TMold>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , Type keyType, Type valueType)
    {
        base.AppendKeyedCollectionStart(typeMold, keyedCollectionType, keyType, valueType);

        typeMold.IncrementIndent();

        var sb = typeMold.Sb;
        return sb.Append(typeMold.Master.Settings.NewLineStyle)
                 .Append(typeMold.Master.Settings.IndentChar
                       , typeMold.Master.Settings.IndentRepeat(typeMold.IndentLevel))
                 .ToInternalTypeBuilder(typeMold);
    }

    public override ITypeMolderDieCast<TMold> AppendKeyedCollectionEnd<TMold>(ITypeMolderDieCast<TMold> typeMold, Type keyedCollectionType
      , Type keyType, Type valueType, int totalItemCount)
    {
        typeMold.Sb.RemoveLastWhiteSpacedCommaIfFound();
        typeMold.DecrementIndent();
        if (totalItemCount > 0)
            typeMold.Sb.Append(typeMold.Master.Settings.NewLineStyle)
                    .Append(typeMold.Master.Settings.IndentChar
                          , typeMold.Master.Settings.IndentRepeat(typeMold.IndentLevel));
        base.AppendKeyedCollectionEnd(typeMold, keyedCollectionType, keyType, valueType, totalItemCount);
        return typeMold;
    }

    public override ITypeMolderDieCast<TMold> FormatCollectionStart<TMold>(ITypeMolderDieCast<TMold> typeMold, Type itemElementType
      , bool hasItems, Type collectionType)
    {
        if (itemElementType == typeof(char) && StyleOptions.CharArrayWritesString) return typeMold.Sb.Append(DblQt).ToInternalTypeBuilder(typeMold);
        if (itemElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String)
            return typeMold.Sb.Append(DblQt).ToInternalTypeBuilder(typeMold);

        if (!hasItems) return typeMold;
        typeMold.IncrementIndent();

        return typeMold.Sb.Append(SqBrktOpn)
                       .Append(typeMold.Master.Settings.NewLineStyle)
                       .Append(typeMold.Master.Settings.IndentChar
                             , typeMold.Master.Settings.IndentRepeat(typeMold.IndentLevel))
                       .ToInternalTypeBuilder(typeMold);
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

    public override ITypeMolderDieCast<TMold> AddCollectionElementSeparator<TMold>(ITypeMolderDieCast<TMold> typeMold, Type elementType
      , int nextItemNumber)
    {
        base.AddCollectionElementSeparator(elementType, typeMold.Sb, nextItemNumber);
        if (elementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String) return typeMold;
        if (elementType == typeof(char) && StyleOptions.CharArrayWritesString) return typeMold;
        if (typeMold.Settings.PrettyCollectionStyle.IsCollectionContentWidthWrap())
        {
            if (typeMold.Settings.PrettyCollectionsColumnContentWidthWrap < typeMold.Sb.LineContentWidth)
            {
                typeMold.Sb.Length -= 1; // remove last Space
                typeMold.Sb.Append(typeMold.Master.Settings.NewLineStyle)
                        .Append(typeMold.Master.Settings.IndentChar
                              , typeMold.Master.Settings.IndentRepeat(typeMold.IndentLevel));
            }
        }
        else
        {
            typeMold.Sb.Append(typeMold.Master.Settings.NewLineStyle)
                    .Append(typeMold.Master.Settings.IndentChar
                          , typeMold.Master.Settings.IndentRepeat(typeMold.IndentLevel));
        }
        return typeMold;
    }

    public override ITypeMolderDieCast<TMold> FormatCollectionEnd<TMold>(ITypeMolderDieCast<TMold> typeMold, Type itemElementType, int totalItemCount)
    {
        if (itemElementType == typeof(char) && StyleOptions.CharArrayWritesString)
        {
            CollectionEnd(itemElementType, typeMold.Sb, totalItemCount);
            return typeMold;
        }
        if (itemElementType == typeof(byte) && StyleOptions.ByteArrayWritesBase64String)
        {
            CollectionEnd(itemElementType, typeMold.Sb, totalItemCount);
            return typeMold;
        }

        typeMold.Sb.RemoveLastWhiteSpacedCommaIfFound();
        if (totalItemCount > 0)
        {
            typeMold.DecrementIndent();
            typeMold.Sb.Append(typeMold.Master.Settings.NewLineStyle)
                    .Append(typeMold.Master.Settings.IndentChar
                          , typeMold.Master.Settings.IndentRepeat(typeMold.IndentLevel));
        }
        return typeMold.Sb.Append(SqBrktCls).ToInternalTypeBuilder(typeMold);
    }
}
