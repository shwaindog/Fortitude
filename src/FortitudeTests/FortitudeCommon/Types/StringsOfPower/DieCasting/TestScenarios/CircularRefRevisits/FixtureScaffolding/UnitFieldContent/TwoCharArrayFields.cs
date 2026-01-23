// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding.UnitFieldContent;


public class TwoCharArrayFields: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString,  IPalantirRevealerFactory<char[]>
{
    public TwoCharArrayFields()
    {
        FirstCharArrayField  = null!;
        SecondCharArrayField = null!;
    }

    public TwoCharArrayFields(char[]? first, char[]? second)
    {
        FirstCharArrayField  = first;
        SecondCharArrayField = second;
    }

    public char[]? FirstCharArrayField { get; set; }
    public char[]? SecondCharArrayField { get; set; }

    public PalantírReveal<char[]> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(SecondCharArrayField)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstCharArrayField)
              , FirstCharArrayField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondCharArrayField)
              , SecondCharArrayField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharArrayField)}:{FirstCharArrayField}, " +
        $"{nameof(SecondCharArrayField)}:{SecondCharArrayField})";
}

public class TwoCharArraysFirstAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
   , IPalantirRevealerFactory<char[]> 
{
    private PalantírReveal<char[]>? cachedRevealer;
    
    public TwoCharArraysFirstAsSimpleCloakedValueContent()
    {
        FirstCharArrayField  = null!;
        SecondCharArrayField = null!;
    }

    public TwoCharArraysFirstAsSimpleCloakedValueContent(char[]? first, char[]? second)
    {
        FirstCharArrayField  = first;
        SecondCharArrayField = second;
    }

    public char[]? FirstCharArrayField { get; set; }
    public char[]? SecondCharArrayField { get; set; }

    public PalantírReveal<char[]> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstCharArrayField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharArrayField)
              , FirstCharArrayField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondCharArrayField)
              , SecondCharArrayField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharArrayField)}:{FirstCharArrayField}, " +
        $"{nameof(SecondCharArrayField)}:{SecondCharArrayField})";
}

public class TwoCharArraysSecondAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<char[]> 
{
    private PalantírReveal<char[]>? cachedRevealer;
    
    public TwoCharArraysSecondAsSimpleCloakedValueContent()
    {
        FirstCharArrayField  = null!;
        SecondCharArrayField = null!;
    }

    public TwoCharArraysSecondAsSimpleCloakedValueContent(char[]? first, char[]? second)
    {
        FirstCharArrayField  = first;
        SecondCharArrayField = second;
    }

    public char[]? FirstCharArrayField { get; set; }
    public char[]? SecondCharArrayField { get; set; }

    public PalantírReveal<char[]> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondCharArrayField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstCharArrayField)
              , FirstCharArrayField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharArrayField)
              , SecondCharArrayField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharArrayField)}:{FirstCharArrayField}, " +
        $"{nameof(SecondCharArrayField)}:{SecondCharArrayField})";
}

public class TwoCharArraysFirstAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<char[]> 
{
    private PalantírReveal<char[]>? cachedRevealer;
    
    public TwoCharArraysFirstAsSimpleCloakedStringContent()
    {
        FirstCharArrayField  = null!;
        SecondCharArrayField = null!;
    }

    public TwoCharArraysFirstAsSimpleCloakedStringContent(char[]? first, char[]? second)
    {
        FirstCharArrayField  = first;
        SecondCharArrayField = second;
    }

    public char[]? FirstCharArrayField { get; set; }
    public char[]? SecondCharArrayField { get; set; }

    public PalantírReveal<char[]> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstCharArrayField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharArrayField)
              , FirstCharArrayField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondCharArrayField)
              , SecondCharArrayField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharArrayField)}:{FirstCharArrayField}, " +
        $"{nameof(SecondCharArrayField)}:{SecondCharArrayField})";
}

public class TwoCharArraysSecondAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<char[]> 
{
    private PalantírReveal<char[]>? cachedRevealer;
    
    public TwoCharArraysSecondAsSimpleCloakedStringContent()
    {
        FirstCharArrayField  = null!;
        SecondCharArrayField = null!;
    }

    public TwoCharArraysSecondAsSimpleCloakedStringContent(char[]? first, char[]? second)
    {
        FirstCharArrayField  = first;
        SecondCharArrayField = second;
    }

    public char[]? FirstCharArrayField { get; set; }
    public char[]? SecondCharArrayField { get; set; }

    public PalantírReveal<char[]> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondCharArrayField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstCharArrayField)
              , FirstCharArrayField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharArrayField)
              , SecondCharArrayField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharArrayField)}:{FirstCharArrayField}, " +
        $"{nameof(SecondCharArrayField)}:{SecondCharArrayField})";
}

public class TwoCharArraysFirstAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<char[]> 
{
    private PalantírReveal<char[]>? cachedRevealer;
    
    public TwoCharArraysFirstAsComplexCloakedValueContent()
    {
        FirstCharArrayField  = null!;
        SecondCharArrayField = null!;
    }

    public TwoCharArraysFirstAsComplexCloakedValueContent(char[]? first, char[]? second)
    {
        FirstCharArrayField  = first;
        SecondCharArrayField = second;
    }

    public char[]? FirstCharArrayField { get; set; }
    public char[]? SecondCharArrayField { get; set; }

    public PalantírReveal<char[]> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstCharArrayField)
           .AsValue (nameof(FirstCharArrayField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharArrayField)
              , FirstCharArrayField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondCharArrayField)
              , SecondCharArrayField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharArrayField)}:{FirstCharArrayField}, " +
        $"{nameof(SecondCharArrayField)}:{SecondCharArrayField})";
}

public class TwoCharArraysSecondAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<char[]> 
{
    private PalantírReveal<char[]>? cachedRevealer;
    
    public TwoCharArraysSecondAsComplexCloakedValueContent()
    {
        FirstCharArrayField  = null!;
        SecondCharArrayField = null!;
    }

    public TwoCharArraysSecondAsComplexCloakedValueContent(char[]? first, char[]? second)
    {
        FirstCharArrayField  = first;
        SecondCharArrayField = second;
    }

    public char[]? FirstCharArrayField { get; set; }
    public char[]? SecondCharArrayField { get; set; }

    public PalantírReveal<char[]> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondCharArrayField)
           .AsValue (nameof(SecondCharArrayField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstCharArrayField)
              , FirstCharArrayField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharArrayField)
              , SecondCharArrayField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharArrayField)}:{FirstCharArrayField}, " +
        $"{nameof(SecondCharArrayField)}:{SecondCharArrayField})";
}

public class TwoCharArraysFirstAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<char[]> 
{
    private PalantírReveal<char[]>? cachedRevealer;
    
    public TwoCharArraysFirstAsComplexCloakedStringContent()
    {
        FirstCharArrayField  = null!;
        SecondCharArrayField = null!;
    }

    public TwoCharArraysFirstAsComplexCloakedStringContent(char[]? first, char[]? second)
    {
        FirstCharArrayField  = first;
        SecondCharArrayField = second;
    }

    public char[]? FirstCharArrayField { get; set; }
    public char[]? SecondCharArrayField { get; set; }

    public PalantírReveal<char[]> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstCharArrayField)
           .AsString (nameof(FirstCharArrayField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharArrayField)
              , FirstCharArrayField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondCharArrayField)
              , SecondCharArrayField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharArrayField)}:{FirstCharArrayField}, " +
        $"{nameof(SecondCharArrayField)}:{SecondCharArrayField})";
}

public class TwoCharArraysSecondAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<char[]> 
{
    private PalantírReveal<char[]>? cachedRevealer;
    
    public TwoCharArraysSecondAsComplexCloakedStringContent()
    {
        FirstCharArrayField  = null!;
        SecondCharArrayField = null!;
    }

    public TwoCharArraysSecondAsComplexCloakedStringContent(char[]? first, char[]? second)
    {
        FirstCharArrayField  = first;
        SecondCharArrayField = second;
    }

    public char[]? FirstCharArrayField { get; set; }
    public char[]? SecondCharArrayField { get; set; }

    public PalantírReveal<char[]> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondCharArrayField)
           .AsString (nameof(SecondCharArrayField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstCharArrayField)
              , FirstCharArrayField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharArrayField)
              , SecondCharArrayField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharArrayField)}:{FirstCharArrayField}, " +
        $"{nameof(SecondCharArrayField)}:{SecondCharArrayField})";
}