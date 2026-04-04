// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable PossibleMultipleEnumeration

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedAddAllEnumerateBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool> =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateBool(fieldName, value.Value, formatString, formatFlags);
    
    public TExt WhenPopulatedAddAllEnumerateBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateBool(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerateNullableBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool?> =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateNullableBool(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerateNullableBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool?>? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateNullableBool(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerate(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerate(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerate<TEnumbl, TFmt>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmt>
        where TFmt : ISpanFormattable? =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerate<TEnumbl, TFmt>(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TEnumbl, TFmt>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : ISpanFormattable? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerate<TEnumbl, TFmt>(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable  =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateNullable(fieldName, value.Value, formatString, formatFlags);
    
    public TExt WhenPopulatedAddAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateNullable(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable   =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateNullable(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateNullable<TEnumbl, TFmtStruct>(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TEnumbl, TRevealBase>(string fieldName, TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TRevealBase : notnull   =>
      value == null ? stb.Mold : WhenPopulatedRevealAllEnumerate(fieldName, value.Value, palantírReveal, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TEnumbl, TRevealBase>(string fieldName, TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.RevealAllEnumerate(value, palantírReveal, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(string fieldName, TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloaked>
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
      value == null 
        ? stb.Mold 
        : WhenPopulatedRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(fieldName, value.Value, palantírReveal, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(string fieldName, TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TRevealBase?
        where TRevealBase : notnull 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.RevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(value, palantírReveal, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloakedStruct?>
        where TCloakedStruct : struct  =>
      value == null ? stb.Mold : WhenPopulatedRevealAllEnumerateNullable(fieldName, value.Value, palantírReveal, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?
        where TCloakedStruct : struct 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.RevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(value, palantírReveal, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable  =>
      value == null ? stb.Mold : WhenPopulatedRevealAllEnumerate(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.RevealAllEnumerate(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TEnumbl, TBearer>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearer>
        where TBearer : IStringBearer?  =>
      value == null ? stb.Mold : WhenPopulatedRevealAllEnumerate<TEnumbl, TBearer>(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerate<TEnumbl, TBearer>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.RevealAllEnumerate<TEnumbl, TBearer>(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable   =>
      value == null ? stb.Mold : WhenPopulatedRevealAllEnumerateNullable(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable?  
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.RevealAllEnumerateNullable(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer =>
      value == null ? stb.Mold : WhenPopulatedRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer  
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.RevealAllEnumerateNullable(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerateString<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<string?>  =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateString(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerateString<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateString(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerateCharSeq<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable  =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateCharSeq(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerateCharSeq<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateCharSeq(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCharSeq>
        where TCharSeq : ICharSequence?  =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateCharSeq<TEnumbl, TCharSeq>(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerateStringBuilder<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<StringBuilder?>  =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateStringBuilder(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerateStringBuilder<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<StringBuilder?>? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateStringBuilder(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerateMatch<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable  =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateMatch(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerateMatch<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateMatch(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

    public TExt WhenPopulatedAddAllEnumerateMatch<TEnumbl, TAny>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TAny>  =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateMatch<TEnumbl, TAny>(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllEnumerateMatch<TEnumbl, TAny>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateMatch<TEnumbl, TAny>(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }
    
    [CallsObjectToString]
    public TExt WhenPopulatedAddAllEnumerateObject<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<object?>  =>
      value == null ? stb.Mold : WhenPopulatedAddAllEnumerateObject(fieldName, value.Value, formatString, formatFlags);
    
    [CallsObjectToString]
    public TExt WhenPopulatedAddAllEnumerateObject<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>? 
    {
      var actualType = value?.GetType() ?? typeof(TEnumbl);
      if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
      if (value != null)
      {
        var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
        ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
        sc.AddAllEnumerateObject(value, formatString, formatFlags);
        var anyItems = sc.ItemCount > 0;
        sc.Complete();
        if (anyItems)
        {
          return stb.AddGoToNext();
        }
      }
      return stb.Mold;
    }

}
