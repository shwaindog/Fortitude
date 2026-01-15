// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestScenarios.CircularRefRevisits.FixtureScaffolding;


public class TwoStringFields: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString,  IPalantirRevealerFactory<string>
{
    public TwoStringFields()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringFields(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(SecondStringField)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsFirstAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
   , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsFirstAsSimpleCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsFirstAsSimpleCloakedValueContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstStringField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsSecondAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsSecondAsSimpleCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsSecondAsSimpleCloakedValueContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondStringField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsFirstAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsFirstAsSimpleCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsFirstAsSimpleCloakedStringContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstStringField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsSecondAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsSecondAsSimpleCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsSecondAsSimpleCloakedStringContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondStringField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsFirstAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsFirstAsComplexCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsFirstAsComplexCloakedValueContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstStringField)
           .AsValue (nameof(FirstStringField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsSecondAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsSecondAsComplexCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsSecondAsComplexCloakedValueContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondStringField)
           .AsValue (nameof(SecondStringField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsFirstAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsFirstAsComplexCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsFirstAsComplexCloakedStringContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstStringField)
           .AsString (nameof(FirstStringField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringsSecondAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<string> 
{
    private PalantírReveal<string>? cachedRevealer;
    
    public TwoStringsSecondAsComplexCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringsSecondAsComplexCloakedStringContent(string? first, string? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public string? FirstStringField { get; set; }
    public string? SecondStringField { get; set; }

    public PalantírReveal<string> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondStringField)
           .AsString (nameof(SecondStringField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

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


public class TwoCharSequenceFields<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    public TwoCharSequenceFields()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequenceFields(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAddCharSeq
               ($"CloakedRevealer{nameof(SecondCharSequenceField)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddCharSeq
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesFirstAsSimpleCloakedValueContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesFirstAsSimpleCloakedValueContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesFirstAsSimpleCloakedValueContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstCharSequenceField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddCharSeq
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesSecondAsSimpleCloakedValueContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesSecondAsSimpleCloakedValueContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesSecondAsSimpleCloakedValueContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondCharSequenceField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwCharSequencesFirstAsSimpleCloakedStringContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwCharSequencesFirstAsSimpleCloakedStringContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwCharSequencesFirstAsSimpleCloakedStringContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstCharSequenceField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddCharSeq
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesSecondAsSimpleCloakedStringContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesSecondAsSimpleCloakedStringContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesSecondAsSimpleCloakedStringContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(cloaked)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesFirstAsComplexCloakedValueContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesFirstAsComplexCloakedValueContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesFirstAsComplexCloakedValueContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsValue (nameof(FirstCharSequenceField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddCharSeq
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesSecondAsComplexCloakedValueContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesSecondAsComplexCloakedValueContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesSecondAsComplexCloakedValueContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsValue (nameof(SecondCharSequenceField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesFirstAsComplexCloakedStringContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesFirstAsComplexCloakedStringContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesFirstAsComplexCloakedStringContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsString (nameof(FirstCharSequenceField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAddCharSeq
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}

public class TwoCharSequencesSecondAsComplexCloakedStringContent<TCharSeq>: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<TCharSeq> 
    where TCharSeq : class, ICharSequence
{
    private PalantírReveal<TCharSeq>? cachedRevealer;
    
    public TwoCharSequencesSecondAsComplexCloakedStringContent()
    {
        FirstCharSequenceField  = null!;
        SecondCharSequenceField = null!;
    }

    public TwoCharSequencesSecondAsComplexCloakedStringContent(TCharSeq? first, TCharSeq? second)
    {
        FirstCharSequenceField  = first;
        SecondCharSequenceField = second;
    }

    public TCharSeq? FirstCharSequenceField { get; set; }
    public TCharSeq? SecondCharSequenceField { get; set; }

    public PalantírReveal<TCharSeq> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(cloaked)
           .AsString (nameof(SecondCharSequenceField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq
               (nameof(FirstCharSequenceField)
              , FirstCharSequenceField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondCharSequenceField)
              , SecondCharSequenceField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstCharSequenceField)}:{FirstCharSequenceField}, " +
        $"{nameof(SecondCharSequenceField)}:{SecondCharSequenceField})";
}


public class TwoStringBuilderFields: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString,  IPalantirRevealerFactory<StringBuilder>
{
    public TwoStringBuilderFields()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuilderFields(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => (cloaked, tos) =>
        tos.StartComplexType(cloaked)
           .Field.AlwaysAdd
               ($"CloakedRevealer{nameof(SecondStringField)}"
              , cloaked, ValueFormatString)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringBuildersFirstAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
   , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsSimpleCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersFirstAsSimpleCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstStringField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringBuildersSecondAsSimpleCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsSimpleCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersSecondAsSimpleCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondStringField)
           .AsValue (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringBuildersFirstAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsSimpleCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersFirstAsSimpleCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(FirstStringField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringBuildersSecondAsSimpleCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsSimpleCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersSecondAsSimpleCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartSimpleContentType(SecondStringField)
           .AsString (cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringBuildersFirstAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsComplexCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersFirstAsComplexCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstStringField)
           .AsValue (nameof(FirstStringField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringBuildersSecondAsComplexCloakedValueContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsComplexCloakedValueContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersSecondAsComplexCloakedValueContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondStringField)
           .AsValue (nameof(SecondStringField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringBuildersFirstAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersFirstAsComplexCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersFirstAsComplexCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(FirstStringField)
           .AsString (nameof(FirstStringField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal
               (nameof(FirstStringField)
              , FirstStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysAdd
               (nameof(SecondStringField)
              , SecondStringField
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}

public class TwoStringBuildersSecondAsComplexCloakedStringContent: IStringBearer, ISupportFormattingFlags, ISupportsValueFormatString
  , IPalantirRevealerFactory<StringBuilder> 
{
    private PalantírReveal<StringBuilder>? cachedRevealer;
    
    public TwoStringBuildersSecondAsComplexCloakedStringContent()
    {
        FirstStringField  = null!;
        SecondStringField = null!;
    }

    public TwoStringBuildersSecondAsComplexCloakedStringContent(StringBuilder? first, StringBuilder? second)
    {
        FirstStringField  = first;
        SecondStringField = second;
    }

    public StringBuilder? FirstStringField { get; set; }
    public StringBuilder? SecondStringField { get; set; }

    public PalantírReveal<StringBuilder> CreateRevealer => cachedRevealer ??= (cloaked, tos) =>
        tos.StartComplexContentType(SecondStringField)
           .AsString (nameof(SecondStringField), cloaked, ValueFormatString, FormattingFlags)
           .Complete();

    public Delegate CreateRevealerDelegate => CreateRevealer;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd
               (nameof(FirstStringField)
              , FirstStringField
              , ValueFormatString, FormattingFlags)
           .Field.AlwaysReveal
               (nameof(SecondStringField)
              , SecondStringField, CreateRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
    
    public string? ValueFormatString { get; set; }
    public FormatFlags FormattingFlags { get; set; }
    
    public override string ToString() => 
        $"{GetType().CachedCSharpNameNoConstraints()}(" +
        $"{nameof(FirstStringField)}:{FirstStringField}, " +
        $"{nameof(SecondStringField)}:{SecondStringField})";
}